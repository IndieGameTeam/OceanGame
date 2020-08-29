using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class PickupObject : MonoBehaviour
{
    public AudioClip soundPickup;

    private AudioSource audioSource;

    public void Pickup(GameManager manager)
    {
        if (soundPickup != null)
        {
            audioSource.PlayOneShot(soundPickup);
        }

        OnPickup(manager);
    }

    protected virtual void OnPickup(GameManager manager) { }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
