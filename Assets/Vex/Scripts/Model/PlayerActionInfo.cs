using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vex
{
    public abstract class PlayerActionInfo
    {
        public Value Cost;
        public Player Player;

        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public abstract GameAction NewActionInstance();
    }

    public class MeleeActionInfo : PlayerActionInfo
    {
        public Value Attack;

        public override GameAction NewActionInstance()
        {
            return new MeleeAttack(Player, Attack);
        }
    }

    public class BuffHealthActionInfo : PlayerActionInfo
    {
        public Value PercentageBuff;

        public override GameAction NewActionInstance()
        {
            return new BuffHealth(Player, PercentageBuff);
        }
    }
}
