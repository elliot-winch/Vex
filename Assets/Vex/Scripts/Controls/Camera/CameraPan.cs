using UnityEngine;
using System.Collections;

public class CameraPan : CameraControlScheme
{
    public float sensitivity;
    public float sensitivityToCameraHeight = 1f;
    public KeyCode forward;
    public KeyCode back;
    public KeyCode left;
    public KeyCode right;
    public float lerpThreshold = 0.02f;

    private Coroutine lerping;
    private string panLockID;

    protected override void UpdateCamera()
    {
        if(lerping != null)
        {
            return;
        }

        float dx = 0f;
        float dz = 0f;

        if (Input.GetKey(forward))
        {
            dz += sensitivity;
        }
        if (Input.GetKey(back))
        {
            dz -= sensitivity;
        }
        if (Input.GetKey(left))
        {
            dx -= sensitivity;
        }
        if (Input.GetKey(right))
        {
            dx += sensitivity;
        }

        if(dx != 0 || dz != 0)
        {
            HeightModifiedPan(dx, dz);
        }
    }

    public void HeightModifiedPan(float dx, float dz)
    {
        var distanceModifier = Mathf.Abs(transform.position.y * sensitivityToCameraHeight);
        dx *= distanceModifier;
        dz *= distanceModifier;

        PanBy(dx, dz);
    }

    public void PanBy(Vector2 d)
    {
        PanBy(d.x, d.y);
    }

    public void PanBy(float dx, float dz)
    { 
        transform.position += dx * new Vector3(transform.right.x, 0f, transform.right.z).normalized + dz * new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
    }

    public void LerpTo(Vector2 xzPosition, float scalar)
    {
        if(lerping != null)
        {
            StopCoroutine(lerping);
        }

        lockDown.Unlock(panLockID);

        panLockID = System.Guid.NewGuid().ToString();

        lockDown.Lock(panLockID);

        lerping = StartCoroutine(LerpToCoroutine(xzPosition, scalar));
    }

    private IEnumerator LerpToCoroutine(Vector2 xzPosition, float scalar)
    {
        Vector2 vec = xzPosition - ZeroPlanePoint();
        Vector3 dest = transform.position + new Vector3(vec.x, 0f, vec.y);

        while(Vector3.Distance(transform.position, dest) > lerpThreshold)
        {
            transform.position = Vector3.Lerp(transform.position, dest, scalar);

            yield return null;
        }

        transform.position = dest;

        lockDown.Unlock(panLockID);

        lerping = null;
    }
}
