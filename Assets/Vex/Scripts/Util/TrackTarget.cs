using UnityEngine;

/// <summary>
/// Used to have this transform track another
/// </summary>
public class TrackTarget : MonoBehaviour
{
    public Transform target;

    [Header("Options")]
    public bool trackPostion = true;
    public bool trackRotation = true;

    [Header("Offsets")]
    public Vector3 worldPositionOffset;
    public Vector3 localPositionOffset;

    public Quaternion rotationOffset = Quaternion.identity;

    private Vector3 prev;

    public bool IsTargetValid
    {
        get
        {
            if(target != null)
            {
                return target.gameObject.activeInHierarchy == true;
            }
            else
            {
                return false;
            }
        }
    }

    void LateUpdate()
    {
        if (IsTargetValid)
        {
            if (trackPostion)
            {
                Vector3 newPos = target.position + worldPositionOffset;

                newPos += target.transform.right * localPositionOffset.x;
                newPos += target.transform.up * localPositionOffset.y;
                newPos += target.transform.forward * localPositionOffset.z;

                transform.position = newPos;
            }

            if (trackRotation)
            {
                transform.rotation = target.rotation * rotationOffset;
            }
        }
    }
}
