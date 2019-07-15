using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPitch : CameraControlScheme
{
    public float sensitivity;
    public KeyCode up;
    public KeyCode down;
    public float minAngle;
    public float maxAngle;

    protected override void UpdateCamera()
    {
        if (Input.GetKey(up))
        {
            PitchBy(-sensitivity);
        }

        if (Input.GetKey(down))
        {
            PitchBy(sensitivity);
        }
    }

    public void PitchBy(float angle)
    {
        var rot = new Vector3(angle, 0f, 0f) + transform.localRotation.eulerAngles;

        if (MathsUtility.Between(rot.x, minAngle, maxAngle))
        {
            transform.localRotation = Quaternion.Euler(rot);
        }
    }
}
