using System;
using UnityEngine;

public class ShipUnit : MonoBehaviour
{
    public event Action OnDamaged;

    private void OnCollisionEnter(Collision collision)
    {
        OnDamaged?.Invoke();
    }
}
