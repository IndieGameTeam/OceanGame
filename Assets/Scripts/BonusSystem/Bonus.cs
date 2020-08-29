using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Bonus : MonoBehaviour
{
    public new string name;

    public Sprite bonusIcon;
    public AudioClip soundPickup;

    private AudioSource audioSource;

    public virtual void OnPickUp()
    {
        if (soundPickup != null)
            audioSource.PlayOneShot(soundPickup);

        Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
