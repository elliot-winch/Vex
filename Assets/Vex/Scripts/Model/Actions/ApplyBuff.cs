using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vex
{
    public class ApplyBuff : GameAction
    {
        public Player Target { get; private set; }
        public int BuffPercentage { get; private set; }

        public ApplyBuff(string name, Player player, int cost, Player target, int buffPercentage) : base(name)
        {
            Target = target;
            BuffPercentage = buffPercentage;
        }

        public override void Execute()
        {
            base.Execute();

            //Target.Attack.AddAction(this, new PercentageModifier(BuffPercentage, roundedUp: false));
        }
    }
}
