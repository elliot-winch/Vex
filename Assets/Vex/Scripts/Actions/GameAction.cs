using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction : MonoBehaviour
{
    [SerializeField] new string name;
    public string Name => name;
    public LockDown ExecuteLock { get; private set; } = new LockDown();

    public void Execute()
    {
        if (CanExecute())
        {
            OnExecute();
        }
    }

    public virtual bool CanExecute() { return ExecuteLock.IsUnlocked; }
    protected abstract void OnExecute();

    public abstract List<GamePiece> GetPossibleTargets();
}

public abstract class PlayerAction : GameAction
{
    public int baseCost;

    public Player Player { get; set; }

    public virtual int CalculateCost() { return baseCost; }

    public override bool CanExecute()
    {
        return base.CanExecute()
            && Player != null
            && Player.CurrentActionPoints >= CalculateCost();
    }

    protected override void OnExecute()
    {
        Player.CurrentActionPoints -= CalculateCost();
    }

    protected void DualAnimation(Player target, string currentAnimationTriggerName, string targetAnimationTriggerName)
    {
        MultiWait mw = new MultiWait(2)
        {
            OnComplete = () =>
            {
                Player.CheckTurnEnd();
            }
        };

        Player.FaceTile(target.CurrentTile);
        Player.StartAnimation(Player.PlayAnimationNamed(currentAnimationTriggerName, 1.5f), mw.CouldComplete);

        target.FaceTile(Player.CurrentTile);
        target.StartAnimation(target.PlayAnimationNamed(targetAnimationTriggerName, 1.5f), mw.CouldComplete);
    }
}
