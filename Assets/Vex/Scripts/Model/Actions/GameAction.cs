using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vex
{
    public struct GameActionExecutionResult
    {
        public bool Success;
        public string FailureReason;
    }

    /// <summary>
    /// A PlayerAction represents a Realised Action of a Player as part of the Game
    /// </summary>
    [Serializable]
    public abstract class GameAction
    {
        [SerializeField]
        private string mExecuteLog;
        [SerializeField]
        private string mRetractLog;

        [SerializeField]
        private bool mHasExecuted;
        [SerializeField]
        private bool mHasRetracted;

        public virtual GameActionExecutionResult CanExecute()
        {
            return new GameActionExecutionResult()
            {
                Success = mHasExecuted == false,
                FailureReason = "GameActions can only be executed once!"
            };
        }

        public virtual GameActionExecutionResult CanRetract()
        {
            GameActionExecutionResult result = new GameActionExecutionResult()
            {
                Success = true
            };

            if(mHasExecuted == false)
            {
                result.Success = false;
                result.FailureReason = "GameActions can only be Retracted once they have been Executed!";
                return result;
            }

            if (mHasRetracted)
            {
                result.Success = false;
                result.FailureReason = "GameActions can only be Retracted once!";
                return result;
            }

            return result;
        }

        public void Execute()
        {
            GameActionExecutionResult result = CanExecute();

            if (result.Success == false)
            {
                Debug.LogError(result.FailureReason);
                return;
            }

            OnExecute();

            mExecuteLog = ProduceExecuteLogString();
            mHasExecuted = true;

            GameLog.Get.Add(this);
        }

        public void Retract()
        {
            GameActionExecutionResult result = CanRetract();

            if (result.Success == false)
            {
                Debug.LogError(result.FailureReason);
                return;
            }

            OnRetract();

            mRetractLog = ProduceRetractLogString();
            mHasRetracted = true;

            GameLog.Get.Add(this);
        }

        protected abstract void OnExecute();
        public abstract string ProduceExecuteLogString();

        protected virtual void OnRetract() { }
        public virtual string ProduceRetractLogString() { return null; }
    }
}
