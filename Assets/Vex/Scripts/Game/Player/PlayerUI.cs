using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Player : OccupyingGamePiece
{
    [Header("UI Info")]
    [SerializeField] LineRenderer outlinePrefab;

    public bool Animating { get; private set; }

    private List<LineRenderer> outline = new List<LineRenderer>();
    private Coroutine currentActionAnimation;

    public void Focus()
    {
        SetUIController("Move");

        SetOutline(true);

        UIManager.Instance.GameCamera.GetComponent<CameraPan>()
            .LerpTo(new Vector2(modelTransform.position.x, modelTransform.position.z), 0.1f);

        UIManager.Instance.ActionsBar.Refresh(controllers.Select(x =>
        {
            var ent = new DynamicButtonEvent();
            ent.AddListener(() => SetUIController(x));

            return new DynamicButton()
            {
                label = x.Name,
                onPressed = ent
            };
        }).ToList());
    }

    public void Unfocus()
    {
        SetUIController("");

        SetOutline(false);
    }

    public void SetUIController(string name)
    {
        SetUIController(controllers?.FirstOrDefault(x => x.Name == name));
    }

    public void SetUIController(IPlayerController controller)
    {
        currentController?.Clear();
        currentController?.SetMouseActive(false);

        currentController = controller;

        currentController?.Refresh();
        currentController?.SetMouseActive(true);
    }

    private void SetOutline(bool show)
    {
        if (show)
        {
            outline.DestroyAll();
            outline = Game.Current.Map.Outline(CurrentTile, outlinePrefab);
        }
        else
        {
            outline.DestroyAll();
        }
    }

    public void SetUIActive(bool show)
    {
        SetOutline(show);

        currentController?.SetMouseActive(show);

        if (show)
        {
            currentController?.Refresh();
        }
        else
        {
            currentController?.Clear();
        }
    }

    public void StartAnimation(IEnumerator coroutine, Action onComplete = null)
    {
        if(currentActionAnimation != null)
        {
            StopCoroutine(currentActionAnimation);
        }

        currentActionAnimation = StartCoroutine(StartAnimationCo(coroutine, onComplete));
    }

    protected IEnumerator StartAnimationCo(IEnumerator coroutine, Action onComplete = null)
    {
        Animating = true;

        SetUIActive(false);

        yield return StartCoroutine(coroutine);

        Animating = false;

        SetUIActive(true);

        onComplete?.Invoke();

        currentActionAnimation = null;
    }
}
