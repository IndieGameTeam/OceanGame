using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    public float waterPlaneY = 0;

    public GameObject explosionParticle;
    public GameObject waterSplashParticle;

    public GameObject shadow;
    public AnimationCurve shadowAlphaCurve;

    private Rigidbody _rigidbody;
    private Renderer _shadowRenderer;

    private const string shadowColorPropertyName = "_Color";

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _shadowRenderer = shadow.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (shadow != null)
        {
            shadow.transform.position = new Vector3(transform.position.x, waterPlaneY, transform.position.z);
        }

        if (transform.position.y < waterPlaneY)
        {
            CreateParticle(2.5F, waterSplashParticle, transform.position);
            gameObject.SetActive(false);
        }

        Color shadowColor = Color.black;

        shadowColor.a = shadowAlphaCurve.Evaluate(transform.position.y);

        _shadowRenderer.material.SetColor(shadowColorPropertyName, shadowColor);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CreateParticle(2.5F, explosionParticle, collision.contacts[0].point);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void CreateParticle(float duration, GameObject particle, Vector3 point)
    {
        GameObject particleInstance = Instantiate(particle, point, Quaternion.identity);
        Destroy(particleInstance, duration);
    }
}
