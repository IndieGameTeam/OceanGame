using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class AsteroidSpawner : MonoBehaviour
{
    public float height = 5F;

    public Vector3 minSpread = Vector3.zero;
    public Vector3 maxSpread = Vector3.one;

    public Asteroid asteroidPrototype;

    private bool spawning = false;
    private Coroutine spawningCoroutine;
    private ObjectPool objectsController;
    private ShipUnit target;


    public void GameProgressChanged(float progress)
    {
        if (progress > 0.1F && !spawning)
        {
            spawningCoroutine = StartCoroutine(ObjectSpawnRoutine());
            spawning = true;
        }

        if(progress > 0.9F && spawning) 
        {
            StopCoroutine(spawningCoroutine);
            spawning = false;
        }
    }

    private void Start()
    {
        target = FindObjectOfType<ShipUnit>();
        objectsController = GetComponent<ObjectPool>();
    }

    private  IEnumerator ObjectSpawnRoutine()
    {
        while (true)
        {
            LevelOptions options = LevelManager.Instance.currentLevel.options;

            float timeout = options.asteroidsSpawnTimeout + Random.value * options.asteroidsSpawnTimeoutSpread;

            yield return new WaitForSeconds(timeout);

            for (int i = 0; i < Random.Range(1, options.maximumAsteroids + 1); i++)
            {
                CreateAsteroid(target.gameObject);
            }
        }
    }

    private void CreateAsteroid(GameObject target)
    {
        float t = Mathf.Sqrt(2 * height / Physics.gravity.magnitude);
        float alpha = Random.value * Mathf.PI * 2F;

        float sin = Mathf.Sin(alpha);
        float cos = Mathf.Cos(alpha);

        Vector3 point = target.transform.position;
        Vector3 vel = target.transform.forward;

        if (target.GetComponent<Rigidbody>() != null)
        {
            vel = target.GetComponent<Rigidbody>().velocity;
        }

        Vector3 v = vel * t * (Mathf.Lerp(minSpread.z * cos, maxSpread.z * cos, Random.value) + (maxSpread.z - minSpread.x));

        point += target.transform.right * Mathf.Lerp(minSpread.x * sin, maxSpread.x * sin, Random.value);
        point += v;
        point.y += Mathf.Lerp(minSpread.y, maxSpread.y, Random.value) + height;

        GameObject asteroid = objectsController.GetObject(ObjectType.Asteroid);

        if (asteroid == null)
        {
            asteroid = Instantiate(asteroidPrototype).gameObject;
            objectsController.AddObject(ObjectType.Asteroid, asteroid);
        }

        asteroid.transform.position = point;
    }
}
