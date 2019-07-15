public struct BallTransferInfo
{
    public enum Type
    {
        Initial,
        PickUp,
        ThrowCatch,
        //Intercept
    }

    public GamePiece sendingPiece;
    public GamePiece receivingPiece;
    public Ball ball;
    public Type type;
}