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
        private string mActionName;

        public string ActionName => mActionName;

        public GameAction(string name)
        {
            mActionName = name;
        }

        public virtual GameActionExecutionResult CanExecute()
        {
            return new GameActionExecutionResult()
            {
                Success = true,
                FailureReason = ""
            };
        }

        public virtual void Execute()
        {
            GameLog.Get.Add(this);
        }

        //toDO: long lasting effects
        //public virtual void Cancel() { }
    }
}
