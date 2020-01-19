using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vex;

public class Test19Jan : MonoBehaviour
{
    private void Awake()
    {
        GameLog newLog = new GameLog();

        Player michael = new Player
        {
            Name = "Michael",
            CurrentAP = new ActionableValue(1),
            AvailableActions = new List<PlayerActionInfo>()
            {
                new MeleeActionInfo()
                {
                    Cost = new ActionableValue(1),
                    Attack = new ActionableValue(2)
                }
            }
        };

        Player sarah = new Player
        {
            Name = "Sarah",
            Health = new ActionableValue(10)
        };

        Player fran = new Player
        {
            Name = "Fran",
            CurrentAP = new ActionableValue(1)
        };

        MeleeAttack michaelMeleeAttack = michael.AvailableActions[0].NewActionInstance() as MeleeAttack;

        michaelMeleeAttack.Target = sarah;

        michaelMeleeAttack.Execute();

        GameLog.Get.DebugLog();
        Debug.Log(sarah.Health.CurrentValue);
    }
}
