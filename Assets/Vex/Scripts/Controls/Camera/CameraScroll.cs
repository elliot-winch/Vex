using UnityEngine;

public class CameraScroll : CameraControlScheme
{
    public float sensitivity;
    public float distanceSensitivity = 1f;
    public float minScroll;
    public float maxScroll;

    protected override void UpdateCamera()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            ScrollBy(Input.mouseScrollDelta.y * sensitivity);
        }
    }

    public void ScrollBy(float distance)
    {
        distance *= Mathf.Abs(distanceSensitivity * transform.position.y);
        Vector3 newPos = transform.position + (transform.forward * distance);
        
        //Using z value as camera is rotated 90deg so y is z and z is y
        if (MathsUtility.Between(newPos.y, minScroll, maxScroll))
        {
            transform.position = newPos;
        }
    }
}
