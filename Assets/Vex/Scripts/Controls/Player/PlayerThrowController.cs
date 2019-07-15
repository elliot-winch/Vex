using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowController : PlayerFreeController<GamePiece>
{
    [SerializeField] private LineRenderer throwPathPrefab;
    
    //private LineRenderer outline;
    private List<LineRenderer> borderRenderers = new List<LineRenderer>();
    private List<LineRenderer> outlineRenderers = new List<LineRenderer>();

    public override void Refresh()
    {
        base.Refresh();

        outlineRenderers = Game.Current.Map.IndividualOutline(possibleTargets.OfType<OccupyingGamePiece>().Select(x => x.CurrentTile));

        borderRenderers = Game.Current.Map.DrawBorder(possibleTargets.Select(x => x.CurrentTile).ToList());
    }
    
    public override void Clear()
    {
        base.Clear();

        borderRenderers.DestroyAll();
        outlineRenderers.DestroyAll();
    }

    protected override LineRenderer DrawPath(Tile tile)
    {
        var lr = Instantiate(throwPathPrefab);

        lr.SetPositions(new Vector3[]
        {
            action.Player.CurrentTile.modelTransform.position,
            tile.modelTransform.position
        });

        return lr;
    }

    public override GamePiece Redirect(GamePiece clickable)
    {
        var clicked = base.Redirect(clickable);

        if(clicked is Tile tile && possibleTargets.Contains(tile.Occupying))
        {
            clicked = tile.Occupying;
        }

        return clicked;
    }

    public override void OnLeftClick(GamePiece clickable)
    {
        base.OnLeftClick(clickable);

        if(action is ThrowAction throwAction)
        {
            throwAction.Target = clickable;
        }

        action.Execute();
    }
}
