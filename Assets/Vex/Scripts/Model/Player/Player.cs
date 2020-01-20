using System.Collections.Generic;

namespace Vex
{
    public class Player
    {
        public string Name;

        public Value Health;
        public Value MovementRange;
        public Value ThrowingRange;

        public Value CurrentAP;
        public Value TotalActions;

        public List<PlayerActionInfo> AvailableActions { get; private set; } = new List<PlayerActionInfo>();

        public void AddAvailableAction(PlayerActionInfo info)
        {
            info.SetPlayer(this);
            AvailableActions.Add(info);
        }
    }
}