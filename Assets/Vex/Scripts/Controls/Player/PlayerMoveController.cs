using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : PlayerFreeController<Tile>
{
    [SerializeField] private LineRenderer outlinePrefab;

    private MoveAction playerMoveAction;

    private List<Tile> targetTiles = new List<Tile>();
    private Tile currentTile;

    private List<LineRenderer> outlines = new List<LineRenderer>();
    private List<LineRenderer> borderRenderers = new List<LineRenderer>();

    public override void Refresh()
    {
        if(currentTile == null)
        {
            currentTile = action.Player.CurrentTile;
        }

        base.Refresh();

        if(action.CanExecute() == false)
        {
            return; //TODO: cannot move UI
        }

        playerMoveAction = action as MoveAction;

        if(playerMoveAction == null)
        {
            return;
        }

        outlines = targetTiles.Select(t =>
        {
            var outline = Instantiate(outlinePrefab);
            outline.transform.position = new Vector3(t.modelTransform.position.x, outline.transform.position.y, t.modelTransform.position.z);
            return outline;
        }).ToList();

        int remainingActions = action.Player.CurrentActionPoints - (playerMoveAction.SpentMoves / playerMoveAction.movementRange);
        int remainingMovesInSplitAction = playerMoveAction.movementRange - (playerMoveAction.SpentMoves % playerMoveAction.movementRange);

        for (int i = 0; i < remainingActions; i++)
        {
            int range = (playerMoveAction.movementRange * i) + remainingMovesInSplitAction;
            AddBorder(range);
        }
    }

    private void AddBorder(int range)
    {
        var tiles = Game.Current.Map.FloodFill(currentTile, range, false);

        borderRenderers.AddRange(Game.Current.Map.DrawBorder(tiles));
    }

    public override void Clear()
    {
        base.Clear();

        outlines.DestroyAll();
        borderRenderers.DestroyAll();
    }

    protected override LineRenderer DrawPath(Tile tile)
    {
        var pathToTile = Game.Current.Map.GetPath(currentTile, tile)?.ValidPath;

        var fullPath = new List<INavigableTile>();
        fullPath.Add(action.Player.CurrentTile);
        fullPath.AddRange(playerMoveAction.Path);
        fullPath.AddRange(pathToTile);

        return Game.Current.Map.DrawPath(fullPath);
    }

    public override void OnLeftClick(Tile clickable)
    {
        base.OnLeftClick(clickable);

        AddTargetTile(clickable);

        playerMoveAction.Execute();

        ResetTargets();
    }

    public override void OnRightClick(Tile clickable)
    {
        base.OnRightClick(clickable);

        if (targetTiles.Contains(clickable))
        {
            RemoveTargetTile(clickable);
        }
        else
        {
            AddTargetTile(clickable);
        }
    }

    public void ResetTargets()
    {
        targetTiles.Clear();
        playerMoveAction.Path.Clear();
        currentTile = action.Player.CurrentTile;
    }

    private void AddTargetTile(Tile clickable)
    {
        targetTiles.Add(clickable);

        playerMoveAction.Path = CalculatePath();

        currentTile = clickable;

        Refresh();

        possibleTargets = playerMoveAction.GetPossibleTargets(currentTile, playerMoveAction.RemainingMoves);
    }

    private void RemoveTargetTile(Tile clickable)
    {
        targetTiles.Remove(clickable);

        playerMoveAction.Path = CalculatePath();

        if(currentTile == clickable)
        {
            currentTile = targetTiles.Last();
        }

        Refresh();

        possibleTargets = playerMoveAction.GetPossibleTargets(currentTile, playerMoveAction.RemainingMoves);
    }

    private List<INavigableTile> CalculatePath()
    {
        var path = new List<INavigableTile>();

        var current = action.Player.CurrentTile;

        for(int i = 0; i < targetTiles.Count; i++)
        {
            var p = Game.Current.Map.GetPath(current, targetTiles[i])?.ValidPath;
            
            if(p != null)
            {
                path.AddRange(p);
            }
            else
            {
                return null;
            }

            current = targetTiles[i];
        }

        return path;
    }
}
