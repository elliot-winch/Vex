using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerController
{
    void SetMouseActive(bool active);
    void Refresh();
    void Clear();

    //Temp?
    string Name { get; }
}

public abstract class PlayerController<T> : MouseSelect<T>, IPlayerController where T : GamePiece
{
    public string controllerName;

    public string Name => controllerName;

    [SerializeField] protected PlayerAction action;

    protected List<GamePiece> possibleTargets;

    protected virtual void Awake() { }
    protected virtual void Start() { }

    public virtual void Refresh()
    {
        Clear();
        possibleTargets = action.GetPossibleTargets();
    }

    public virtual void Clear() { }
}
