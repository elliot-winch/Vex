using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAction : PlayerAction
{
    public int throwingRange = 10;

    public GamePiece Target { get; set; }

    public override bool CanExecute()
    {
        return base.CanExecute()
            && Target != null;
    }

    protected override void OnExecute()
    {
        base.OnExecute();

        var info = Player.GiveBallTo(Target, BallTransferInfo.Type.ThrowCatch);

        if(Target is Player targetPlayer)
        {
            DualAnimation(targetPlayer, "throw", "catch");
        }

        Target = null;
    }

    public override List<GamePiece> GetPossibleTargets()
    {
        var tiles = Game.Current.Map.FloodFill(Player.CurrentTile, throwingRange, true);

        //Able to receive check
        var occupiedTiles = tiles.Where(x => x.Occupying != null);

        var validOccupants = occupiedTiles
            .Select(x => x.Occupying)
            .Where(x => Player.CanTransferBall(x, BallTransferInfo.Type.ThrowCatch));

        var validNonOccupied = tiles
            .Except(occupiedTiles)
            .Where(x => Player.CanTransferBall(x, BallTransferInfo.Type.ThrowCatch));

        return validOccupants
            .Union(validNonOccupied)
            .ToList();
    }

    private bool CanThrowBall(GamePiece receivingPiece)
    {
        //In range
        bool val = HexTileMap.Distance(receivingPiece.CurrentTile, Player.CurrentTile) <= throwingRange;

        //In line of sight
        val &= Game.Current.Map.InLineOfSight(receivingPiece.CurrentTile, Player.CurrentTile);

        return val;
    }
}
