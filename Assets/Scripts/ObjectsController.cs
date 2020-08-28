using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsController : MonoBehaviour
{
    public bool IsSpawninig { get; private set; }
    
    public float height = 5F;

    public Vector3 minSpread = Vector3.zero;
    public Vector3 maxSpread = Vector3.one;

    public Asteroid asteroidPrototype;

    private List<GameObject> cachedAsteroids = new List<GameObject>();
    private Coroutine spawnCoroutine;

    public void BeginAsteroidSpawning(GameObject target, LevelOptions options)
    {
        IsSpawninig = true;
        spawnCoroutine = StartCoroutine(ObjectSpawnRoutine(target, options));
    }

    public void EndAsteroidSpawning()
    {
        IsSpawninig = false;
        StopCoroutine(spawnCoroutine);
    }

    private IEnumerator ObjectSpawnRoutine(GameObject target, LevelOptions options)
    {
        while (true)
        {
            float timeout = options.asteroidsSpawnTimeout + Random.value * options.asteroidsSpawnTimeoutSpread;

            yield return new WaitForSeconds(timeout);

            for (int i = 0; i < Random.Range(1, options.maximumAsteroids + 1); i++)
            {
                CreateAsteroid(target);
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

        GameObject asteroid = cachedAsteroids.Find(x => x.activeSelf == false);

        if (asteroid == null)
        {
            asteroid = Instantiate(asteroidPrototype).gameObject;
            cachedAsteroids.Add(asteroid);
        }
        else
        {
            asteroid.gameObject.SetActive(true);
        }

        asteroid.transform.position = point;
    }
}
