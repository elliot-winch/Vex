using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : CameraControlScheme
{
    public float sensitivity;
    public KeyCode rotateLeft;
    public KeyCode rotateRight;

    private Vector3 zpp;
    private bool pressedLastFrame;

    protected override void UpdateCamera()
    {
        bool pressedThisFrame = false;

        if (Input.GetKey(rotateLeft))
        {
            RotateBy(-sensitivity);

            pressedThisFrame = true;
        }

        if (Input.GetKey(rotateRight))
        {
            RotateBy(sensitivity);

            pressedThisFrame = true;
        }

        if(pressedThisFrame != pressedLastFrame)
        {
            zpp = ZeroPlanePoint();
            pressedLastFrame = pressedThisFrame;
        }
    }

    public void RotateBy(float dr)
    {
        transform.RotateAround(zpp, Vector3.up, dr);
    }
}
