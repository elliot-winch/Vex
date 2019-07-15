using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : OccupyingGamePiece
{
    //TODO: determine time from animation
    public IEnumerator PlayAnimationNamed(string triggerName, float time)
    {
        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(time);
    }

    protected override void OnBeginMove()
    {
        base.OnBeginMove();

        animator.SetBool("isMoving", true);
    }

    protected override void OnEndMove()
    {
        base.OnEndMove();

        animator.SetBool("isMoving", false);
    }

    protected override void OnReceiveBall(BallTransferInfo info)
    {
        base.OnReceiveBall(info);

        info.ball.GetComponent<TrackTarget>().target = ballCarryTransform;
    }
}
