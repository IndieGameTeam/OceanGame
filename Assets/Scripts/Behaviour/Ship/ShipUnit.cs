using System;
using UnityEngine;

public class ShipUnit : MonoBehaviour
{
    public event Action OnDamaged;
    public event Action<PickupObject> OnPickup;

    private void OnCollisionEnter(Collision collision)
    {
        OnDamaged?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupObject pickup = other.GetComponent<PickupObject>();

        if (pickup != null)
        {
            OnPickup?.Invoke(pickup);
        }
    }
}
