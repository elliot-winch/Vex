using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GamePiece : MonoBehaviour
{
    //TODO: proper IDs
    public string ID;
    [SerializeField] protected Transform ballCarryTransform;

    public Tile CurrentTile { get; protected set; }

    public Ball Ball { get; private set; }

    protected virtual void Awake() { }

    public bool CanTransferBall(GamePiece receivingPiece, BallTransferInfo.Type type)
    {
        return CanTransferBall(new BallTransferInfo()
        {
            sendingPiece = this,
            ball = Ball,
            receivingPiece = receivingPiece,
            type = type
        });
    }

    public bool CanTransferBall(BallTransferInfo info)
    {
        return CanGiveBall(info) && info.receivingPiece.CanReceiveBall(info);
    }

    public virtual bool CanReceiveBall(BallTransferInfo info)
    {
        return info.ball != null && Ball == null;
    }

    public void ReceiveBall(BallTransferInfo info)
    {
        if (CanReceiveBall(info))
        {
            Ball = info.ball;
            Ball.CurrentCarrier = this;

            OnReceiveBall(info);
        }
    }

    public virtual bool CanGiveBall(BallTransferInfo info)
    {
        return info.receivingPiece != null;
    }

    public BallTransferInfo GiveBallTo(GamePiece receivingPiece, BallTransferInfo.Type type)
    {
        var info = new BallTransferInfo()
        {
            receivingPiece = receivingPiece,
            type = type,
            ball = Ball,
            sendingPiece = this
        };

        if(CanGiveBall(info) && (info.receivingPiece?.CanReceiveBall(info) == true))
        {
            info.receivingPiece.ReceiveBall(info);

            OnGiveBallTo(info);

            Ball = null;
        }

        return info;
    }

    protected virtual void OnGiveBallTo(BallTransferInfo receivingPiece) { }
    protected virtual void OnReceiveBall(BallTransferInfo info) { }
}
