using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vex
{
    public class Player
    {
        public string Name;

        public ActionableValue Health;
        public ActionableValue MovementRange;
        public ActionableValue ThrowingRange;

        public ActionableValue CurrentAP;
        public ActionableValue TotalActions;

        public List<PlayerActionInfo> AvailableActions;
    }
}