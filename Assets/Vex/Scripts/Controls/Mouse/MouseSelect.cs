using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MouseSelect<T> : MonoBehaviour where T : MonoBehaviour
{
    public LayerMask layer;
    public float maxDist = Mathf.Infinity;
    public float doubleClickTime = 0.2f;

    private T current;
    private Coroutine clickCo;
    private bool active;

    public virtual void OnMouseOverBegin(T clickable) { }
    public virtual void OnMouseOverEnd(T clickable) { }
    public virtual void OnLeftClick(T clickable) { }
    public virtual void OnLeftDoubleClick(T clickable) { }
    public virtual void OnRightClick(T clickable) { }
    public virtual void OnRightDoubleClick(T clickable) { }
    //Allows for a subclass to select something else based on the current clickable
    public virtual T Redirect(T clickable) { return clickable; }

    public virtual void SetMouseActive(bool active)
    {
        this.active = active;
        current = null;
    }

    private void Update()
    {
        if(active == false)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            current = null;
            //mouse pointer is over UI
            return;
        }

        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, maxDist, layer.value);

        T t = hitInfo.transform?.GetComponentInParent<T>();

        //MouseOver
        if(t != current)
        {
            OnMouseOverEnd(Redirect(t));

            OnMouseOverBegin(Redirect(t));
        }

        current = t;

        //Clicks
        if (t != null && clickCo == null)
        {
            if (Input.GetMouseButtonDown(0))
            {           
                clickCo = StartCoroutine(Click(0, current, OnLeftClick, OnLeftDoubleClick));
            }

            if (Input.GetMouseButtonDown(1))
            {
                clickCo = StartCoroutine(Click(1, current, OnRightClick, OnRightDoubleClick));
            }
        }
    }

    private IEnumerator Click(int mouseIndex, T curr, Action<T> click, Action<T> doubleClick)
    {
        yield return null;

        float doubleClickTimer = 0f;

        while (doubleClickTimer < doubleClickTime)
        {
            if (Input.GetMouseButtonDown(mouseIndex) && curr == current)
            {          
                doubleClick?.Invoke(Redirect(curr));
                clickCo = null;
                yield break;
            }

            doubleClickTimer += Time.deltaTime;

            yield return null;
        }

        click?.Invoke(Redirect(curr));
        clickCo = null;
    }
}
