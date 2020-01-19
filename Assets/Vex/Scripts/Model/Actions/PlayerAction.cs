using System;

/*
namespace Vex
{
    /// <summary>
    /// A PlayerAction represents a Realised Action of a Player as part of the Game
    /// </summary>
    [Serializable]
    public abstract class PlayerAction : GameAction
    {
        public Player Player { get; private set; }
        public int Cost { get; private set; }

        public PlayerAction(string name, Player player, int cost) : base(name)
        {
            Player = player;
            Cost = cost;
        }

        public override bool CanExecute(out string reasonForFailure)
        {
            reasonForFailure = "";
            bool canExecuteBase = base.CanExecute(out string baseReasonForFailure);

            if(canExecuteBase == false)
            {
                reasonForFailure = baseReasonForFailure;
                return false;
            }

            int cost = CalculateCost();
            int currentAP = Player.CurrentAP.CurrentValue;

            if (currentAP >= cost)
            {
                reasonForFailure = string.Format("{0} only has {1} AP. {2} requires {3} AP", Player.Name, currentAP, ActionName, cost);
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            Player.CurrentAP.AddAction(this, new LumpSumModifier(CalculateCost()));
        }

        public virtual int CalculateCost()
        {
            return Cost;
        }
    }
}
*/
