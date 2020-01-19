using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vex
{
    public class GameLog
    {
        public static GameLog Get { get; private set; }

        public List<GameAction> Log { get; private set; } = new List<GameAction>();

        public GameLog()
        {
            Get = this;
        }

        public void Add(GameAction action)
        {
            Log.Add(action);
        }

        public void DebugLog()
        {
            foreach(GameAction action in Log)
            {
                Debug.Log(JsonUtility.ToJson(action));
            }
        }
    }
}
