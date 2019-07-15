using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccupyingGamePiece : GamePiece
{
    [SerializeField] protected Transform modelTransform;
    [SerializeField] protected float moveAnimtionTime;

    //Modifies back end
    public virtual void SetTile(Tile tile)
    {
        if (CurrentTile != null)
        {
            CurrentTile.Occupying = null;
        }

        CurrentTile = tile;

        if (CurrentTile != null)
        {
            CurrentTile.Occupying = this;
        }
    }

    //Modifies back end and front end
    public void PlaceOnTile(Tile tile)
    {
        SetTile(tile);
        modelTransform.position = tile.modelTransform.position;
    }

    public void FaceTile(Tile t, float offset = 0f)
    {
        modelTransform.LookAt(new Vector3(
            t.modelTransform.position.x,
            modelTransform.position.y,
            t.modelTransform.position.z
        ));
    }

    protected virtual void OnPassOverTile(Tile t)
    {
        //Pick up ball
        if (t.Ball != null)
        {
            t.GiveBallTo(this, BallTransferInfo.Type.PickUp);
        }
    }
    protected virtual void OnBeginMove() { }
    protected virtual void OnEndMove() { }
   
    public IEnumerator Move(List<INavigableTile> path)
    {
        if (path == null)
        {
            //Invalid
            yield break;
        }

        var pathQueue = new Queue(path);
        var curTile = CurrentTile;

        SetTile(path.Last() as Tile);

        OnBeginMove();

        //Last tile is destination
        while (pathQueue.Count > 0)
        {
            var nextTile = pathQueue.Dequeue() as Tile;

            OnPassOverTile(curTile);

            FaceTile(nextTile);

            float time = 0f;

            while (time < moveAnimtionTime)
            {
                time += Time.deltaTime;
                modelTransform.position = Vector3.Lerp(curTile.modelTransform.position, nextTile.modelTransform.position, time / moveAnimtionTime);

                yield return null;
            }

            modelTransform.position = nextTile.modelTransform.position;

            curTile = nextTile;
        }

        OnEndMove();
    }
}
