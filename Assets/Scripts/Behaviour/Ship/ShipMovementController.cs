using UnityEngine;
using GameServices.Math;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipMovementController : MonoBehaviour
{
    [System.Serializable]
    public class Vector3AnimationCurve
    {
        public AnimationCurve XAxis = new AnimationCurve();
        public AnimationCurve YAxis = new AnimationCurve();
        public AnimationCurve ZAxis = new AnimationCurve();

        public Vector3 Evaluate(float time)
        {
            return new Vector3()
            {
                x = XAxis.Evaluate(time),
                y = YAxis.Evaluate(time),
                z = ZAxis.Evaluate(time)
            };
        }
    }

    public float damping = 0.01F;
    public float minSpeed = 0.5F;
    public float maxSpeed = 2.0F;
    public float sideRollAngle = 10F;

    public Vector3AnimationCurve shipPitching;
    public PIDController shipPID;

    private float y = 0;
    private float z = 0;
    private float speed = 0F;

    private Vector3 _defaultForwardDirection = Vector3.forward;

    private Rigidbody _rigidbody;


    private void Start()
    {
        speed = (minSpeed + maxSpeed) * 0.5F;
        shipPitching.XAxis.postWrapMode = WrapMode.Loop;
        shipPitching.YAxis.postWrapMode = WrapMode.Loop;
        shipPitching.ZAxis.postWrapMode = WrapMode.Loop;

        _rigidbody = GetComponent<Rigidbody>();
        _defaultForwardDirection = transform.forward;
    }

    private void FixedUpdate()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        y = ScreenClamp(y + inputDirection.x);
        z += shipPID.PID(z, inputDirection.x * sideRollAngle);

        speed = Mathf.Lerp(minSpeed, maxSpeed, 0.5F + inputDirection.z * 0.5F);

        Quaternion forward = Quaternion.LookRotation(_defaultForwardDirection);
        Quaternion rotation = Quaternion.Euler(forward * (shipPitching.Evaluate(Time.time) + new Vector3(0, y, z)));

        transform.rotation = rotation;
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, transform.forward * speed, damping * Time.fixedTime);
    }

    private float ScreenClamp(float angle)
    {
        float minAngle = -90;
        float maxAngle = 90;

        float t = -1.5F;
        float halfWidth = Screen.width * 0.5F;
        float horizontal = (Camera.main.WorldToScreenPoint(transform.position).x - halfWidth) / halfWidth * 1.15F;

        horizontal = Mathf.Clamp01(Mathf.Abs(horizontal - horizontal * t) + t) * Mathf.Sign(horizontal);

        if (horizontal < 0)
        {
            minAngle *= (1F - Mathf.Abs(horizontal));
        }
        else if (horizontal > 0)
        {
            maxAngle *= (1F - horizontal);
        }

        return Mathf.Clamp(angle, minAngle, maxAngle);
    }
}
