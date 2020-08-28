using UnityEngine;

public class WaweAudioController : MonoBehaviour
{
    public AudioSource[] sources;

    private void FixedUpdate()
    {
        foreach (var source in sources)
        {
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
    }
}
