using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vex
{
    public class MeleeAttack : GameAction
    {
        public Player Target;

        private Player mAgent;
        private Value mAttack;

        [SerializeField]
        private int mAttackValue;

        public MeleeAttack(Player agent, Value attack) : base()
        {
            mAgent = agent;
            mAttack = attack;
        }

        public override GameActionExecutionResult CanExecute()
        {
            GameActionExecutionResult result = base.CanExecute();

            if(result.Success == false)
            {
                return result;
            }

            if(mAgent == null)
            {
                result.FailureReason = "MeleeAttack requires an Agent";
                result.Success = false;
            }

            if (Target == null)
            {
                result.FailureReason = "MeleeAttack requires a Target";
                result.Success = false;
            }

            return result;
        }

        protected override void OnExecute()
        {
            mAttackValue = mAttack.CurrentValue;
            Target.Health.AddModifier(this, new LumpSumModifier( - mAttackValue));
        }

        public override string ProduceExecuteLogString()
        {
            return string.Format("{0} has attacked {1} for {2} damage", mAgent.Name, Target.Name, mAttackValue);
        }
    }
}
