using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : OccupyingGamePiece
{
    public int scoreValue;
    public List<Tile> ScoringTiles { get; private set; }

    private List<LineRenderer> border = new List<LineRenderer>();

    public override void SetTile(Tile tile)
    {
        base.SetTile(tile);

        //For now, just the tiles around the goal
        ScoringTiles = Game.Current.Map.FloodFill(CurrentTile, 1, true);

        border.ForEach(x => Destroy(x.gameObject));
        border.Clear();

        border = Game.Current.Map.DrawBorder(ScoringTiles);
    }

    public override bool CanReceiveBall(BallTransferInfo info)
    {
        return base.CanReceiveBall(info) 
            && info.sendingPiece != null
            && ScoringTiles.Contains(info.sendingPiece.CurrentTile);
    }

    protected override void OnReceiveBall(BallTransferInfo info)
    {
        base.OnReceiveBall(info);

        if(info.sendingPiece is Player player)
        {
            Game.Current.Score(player.Team, this.scoreValue, info.ball);
        }
    }
}
