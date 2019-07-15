using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : OccupyingGamePiece
{
    //Inspector variables
    [SerializeField] List<IPlayerController> controllers;
    [SerializeField] List<PlayerAction> actions;
    [SerializeField] protected Animator animator;

    //Public variables
    public Action OnTurnComplete;

    //Properties
    public int CurrentActionPoints { get; set; }
    public Team Team { get; set; }
    public bool TurnIsComplete { get; private set; }

    //Private variables
    private IPlayerController currentController;

    protected override void Awake()
    {
        base.Awake();

        InitStats();

        controllers = GetComponentsInChildren<IPlayerController>().ToList();

        actions.ForEach(x =>
        {
            x.Player = this;
        });
    }

    public virtual void BeginPhase()
    {
        CurrentActionPoints = actionsPerTurn;
        TurnIsComplete = false;
    }

    public void CheckTurnEnd()
    {
        if (CurrentActionPoints <= 0)
        {
            TurnIsComplete = true;
            OnTurnComplete?.Invoke();      
        }
    }

    public virtual void EndPhase()
    {

    }
}
