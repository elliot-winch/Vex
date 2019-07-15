using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : PlayerAction
{
    public int movementRange = 6;

    public List<INavigableTile> Path { get; set; } = new List<INavigableTile>();

    public int MaxMoves => Player.CurrentActionPoints * movementRange;
    public int SpentMoves => Path.Count;
    public int RemainingMoves => MaxMoves - SpentMoves;

    public override bool CanExecute()
    {
        return base.CanExecute()
            && Path != null;
    }

    public override int CalculateCost()
    {
        return base.CalculateCost() + Mathf.CeilToInt(Path.Count / (float)movementRange);
    }

    protected override void OnExecute()
    {
        base.OnExecute();

        Player.StartAnimation(Player.Move(Path), () => Player.CheckTurnEnd());

        Path.Clear();
    }

    public override List<GamePiece> GetPossibleTargets()
    {
        return GetPossibleTargets(Player.CurrentTile, MaxMoves);
    }

    public List<GamePiece> GetPossibleTargets(Tile center, int range)
    {
        return Game.Current.Map.FloodFill(center, range, false).Cast<GamePiece>().ToList();
    }
}
