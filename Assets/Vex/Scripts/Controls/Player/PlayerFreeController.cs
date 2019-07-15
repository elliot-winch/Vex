using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerFreeController<T> : PlayerController<T> where T : GamePiece
{
    [SerializeField] private GameObject cursorPrefab;

    private GameObject cursor;
    private LineRenderer pathLine;

    protected override void Awake()
    {
        base.Awake();

        cursor = Instantiate(cursorPrefab);

        HideCursor();
    }

    public override void Clear()
    {
        base.Clear();

        HideCursor();
    }

    protected virtual void DisplayCursorAt(Tile tile)
    {
        cursor.SetActive(true);
        cursor.transform.position = tile.modelTransform.position;

        pathLine = DrawPath(tile);
    }

    protected virtual void HideCursor()
    {
        cursor.SetActive(false);

        if (pathLine != null)
        {
            Destroy(pathLine.gameObject);
        }
    }

    protected virtual LineRenderer DrawPath(Tile tile)
    {
        return null;
    }

    protected virtual bool IsValidTarget(T clickable)
    {
        return clickable != null && possibleTargets.Contains(clickable);
    }

    public override void OnMouseOverBegin(T clickable)
    {
        base.OnMouseOverBegin(clickable);

        if (IsValidTarget(clickable))
        {
            DisplayCursorAt(clickable.CurrentTile);
        }
    }

    public override void OnMouseOverEnd(T clickable)
    {
        base.OnMouseOverEnd(clickable);

        HideCursor();
    }
}
