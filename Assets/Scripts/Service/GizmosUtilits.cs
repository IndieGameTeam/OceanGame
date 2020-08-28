using UnityEngine;

namespace GameServices
{
    public static class GizmosUtilits
    {
        public static void DrawEllipse(Vector3 point, float radiusX, float radiusY)
        {
            DrawEllipse(point, Vector3.forward, Vector3.up, radiusX, radiusY);
        }
        public static void DrawEllipse(Vector3 point, Vector3 forward, Vector3 up, float radiusX, float radiusY)
        {
            int segments = 30;
            float angle = 0f;

            Quaternion rot = Quaternion.LookRotation(forward, up);

            Vector3 lastPoint = Vector3.zero;
            Vector3 thisPoint = Vector3.zero;

            for (int i = 0; i < segments + 1; i++)
            {
                thisPoint.x = Mathf.Sin(Mathf.Deg2Rad * angle) * radiusX;
                thisPoint.z = Mathf.Cos(Mathf.Deg2Rad * angle) * radiusY;

                if (i > 0)
                {
                    Gizmos.DrawLine(rot * lastPoint + point, rot * thisPoint + point);
                }

                lastPoint = thisPoint;
                angle += 360f / segments;
            }
        }
    }
}
