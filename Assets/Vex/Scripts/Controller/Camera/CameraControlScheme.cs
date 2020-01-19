using UnityEngine;

[RequireComponent(typeof(Camera))]
public abstract class CameraControlScheme : MonoBehaviour
{
    protected new Camera camera;

    protected static LockDown lockDown = new LockDown();

    private Plane zeroPlane = new Plane(Vector3.up, Vector3.zero);

    protected virtual void Awake()
    {
        camera = GetComponent<Camera>();
    }

    protected virtual void Update()
    {
        if (lockDown.IsUnlocked)
        {
            UpdateCamera();
        }
    }

    protected abstract void UpdateCamera();

    protected Vector2 ZeroPlanePoint()
    {
        Ray r = new Ray(transform.position, transform.forward);

        zeroPlane.Raycast(r, out float distance);

        Vector3 point = r.GetPoint(distance);

        return new Vector2(point.x, point.z);
    }
}
