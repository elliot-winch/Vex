using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vex
{
    public class MeleeAttack : GameAction
    {
        public Player Target;

        private ActionableValue mAttack;

        public MeleeAttack(string name, ActionableValue attack) : base(name)
        {
            mAttack = attack;
        }

        public override GameActionExecutionResult CanExecute()
        {
            GameActionExecutionResult result = base.CanExecute();

            if(result.Success == false)
            {
                return result;
            }

            if (Target != null)
            {
                result.FailureReason = "MeleeAttack requires Target";
                result.Success = false;
            }

            return result;
        }

        public override void Execute()
        {
            base.Execute();

            Target.Health.AddModifier(new LumpSumModifier( - mAttack.CurrentValue), this);
        }
    }
}
