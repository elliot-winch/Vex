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
            CurrentAP = new Value(1)
        };

        michael.AddAvailableAction(new BuffHealthActionInfo()
        {
            Cost = new Value(1),
            PercentageBuff = new Value(10)
        });

        Player sarah = new Player
        {
            Name = "Sarah",
            Health = new Value(10)
        };

        Player fran = new Player
        {
            Name = "Fran",
            CurrentAP = new Value(1)
        };

        var michaelBuffHealth = michael.AvailableActions[0].NewActionInstance() as BuffHealth;

        michaelBuffHealth.Target = sarah;

        michaelBuffHealth.Execute();

        Debug.Log(sarah.Health.CurrentValue);

        michaelBuffHealth.Retract();

        Debug.Log(sarah.Health.CurrentValue);

        GameLog.Get.DebugLog();

    }
}
