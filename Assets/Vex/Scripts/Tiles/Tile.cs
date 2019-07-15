using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile : GamePiece, INavigableTile
{
    public Transform modelTransform;
    public float moveCost = 1f;

    [HideInInspector]
    public HexTileMap map;
    public Vector2Int axialCoordinate;

    private GamePiece occupying;
    public GamePiece Occupying
    {
        get => occupying;
        set
        {
            if(occupying != null)
            {
                passable.Unlock(occupying.ID);
            }

            occupying = value;
            
            if(occupying != null)
            {
                passable.Lock(occupying.ID);
            }
        }
    }

    private LockDown passable = new LockDown();
    private LineRenderer outline;

    protected override void Awake()
    {
        base.Awake();

        CurrentTile = this;
    }

    #region INavigableTile
    public bool IsUnlocked => passable.IsUnlocked;

    public float MoveIntoCost => moveCost;

    public Vector3 Position => HexTileMap.AxialToCube(axialCoordinate);

    public List<Tile> NeighbouringTiles => Neighbours.Cast<Tile>().ToList();
    public IEnumerable<INavigableTile> Neighbours { get; set; }
    #endregion

    protected override void OnReceiveBall(BallTransferInfo info)
    {
        base.OnReceiveBall(info);

        info.ball.GetComponent<TrackTarget>().target = null;
        info.ball.transform.position = modelTransform.position;
    }
}

