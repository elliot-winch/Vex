using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vex
{
    public abstract class PlayerActionInfo
    {
        public ActionableValue Cost;

        public abstract GameAction NewActionInstance();
    }

    public class MeleeActionInfo : PlayerActionInfo
    {
        public ActionableValue Attack;

        public override GameAction NewActionInstance()
        {
            return new MeleeAttack("Punch", Attack);
        }
    }
}
