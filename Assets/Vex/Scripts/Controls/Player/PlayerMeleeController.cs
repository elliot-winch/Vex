using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeController : PlayerFreeController<Player>
{
    [SerializeField] private LineRenderer outlinePrefab;

    private List<LineRenderer> outlineRenderers = new List<LineRenderer>();

    public override void Refresh()
    {
        base.Refresh();

        outlineRenderers = Game.Current.Map.IndividualOutline(possibleTargets.Select(x => x.CurrentTile));
    }

    public override void Clear()
    {
        base.Clear();

        outlineRenderers.DestroyAll();
    }

    public override void OnLeftClick(Player clickable)
    {
        base.OnLeftClick(clickable);

        if(action is MeleeAttack melee)
        {
            melee.Target = clickable;
        }

        action.Execute();
    }
}