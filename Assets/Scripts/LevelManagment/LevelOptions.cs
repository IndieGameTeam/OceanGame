using UnityEngine;

[CreateAssetMenu]
public class LevelOptions : ScriptableObject
{
    public float asteroidsSpawnTimeoutSpread = 0.5F;

    [Tooltip("Timeout between asteroid spawn iterations.")]
    public float asteroidsSpawnTimeout = 2;

    [Tooltip("Count of asteroids can will be spawned in one time.")]
    public int maximumAsteroids = 1;

    [Tooltip("Distance from start to level end.")]
    public float levelLength = 100F;
}
