using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : PlayerAction
{
    public int meleeAttackRange = 1;
    public int baseAttack = 3;

    public ModifiableValue Attack { get; private set; }

    public Player Target { get; set; }

    private void Awake()
    {
        Attack = new ModifiableValue(baseAttack);
    }

    public override bool CanExecute()
    {
        return base.CanExecute()
            && Target != null;
    }

    protected override void OnExecute()
    {
        base.OnExecute();

        if(Attack.CurrentValue > Target.Defense.CurrentValue)
        {
            Target.AdjustHealth(Attack.CurrentValue - Target.Defense.CurrentValue);
        }

        if (Target is Player targetPlayer)
        {
            DualAnimation(targetPlayer, "melee", "recoil");
        }
    }

    public override List<GamePiece> GetPossibleTargets()
    {
        var tiles = Game.Current.Map.FloodFill(Player.CurrentTile, meleeAttackRange, true);

        tiles.Remove(Player.CurrentTile);

        return tiles
            .Select(x => x.Occupying)
            .NonNull()
            .OfType<Player>()
            .Cast<GamePiece>()
            .ToList();
    }
}
