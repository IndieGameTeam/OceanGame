using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;

    public bool UseX = true;
    public bool UseY = true;
    public bool UseZ = true;

    public Transform target = null;

    private void LateUpdate()
    {
        if(target != null)
        {
            Vector3 point = transform.position;

            if (UseX) 
            {
                point.x = target.position.x + offset.x;
            }

            if(UseY)
            {
                point.y = target.position.y + offset.y;
            }

            if(UseZ)
            {
                point.z = target.position.z + offset.z;
            }

            transform.position = point;
        }
    }
}
