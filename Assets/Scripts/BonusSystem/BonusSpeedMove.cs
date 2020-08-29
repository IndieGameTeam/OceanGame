using System;

public class BonusSpeedMove : Bonus, IPickup
{
    public float addSpeed;
    public float timeBuff;
    public bool isSingle;

    public void Pickup()
    {
        OnPickUp();
        Destroy(gameObject);
    }

    public override void OnPickUp()
    {
        base.OnPickUp();

        ShipMovementController shipMovement = FindObjectOfType<ShipMovementController>();
        PlayerBuffs buffs = FindObjectOfType<PlayerBuffs>();

        Action onStart = () =>
        {
            shipMovement.maxSpeed += addSpeed;
            shipMovement.minSpeed += addSpeed;
        };

        Action onEnd = () =>
        {
            shipMovement.maxSpeed -= addSpeed;
            shipMovement.minSpeed -= addSpeed;
        };

        buffs.AddBuff(new Buff(name, isSingle, timeBuff, onStart, onEnd));
    }
}
