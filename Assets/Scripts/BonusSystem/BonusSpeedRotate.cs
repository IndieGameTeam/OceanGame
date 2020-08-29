using System;

public class BonusSpeedRotate : Bonus, IPickup
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
            shipMovement.sideRollAngle += addSpeed;
        };

        Action onEnd = () =>
        {
            shipMovement.sideRollAngle -= addSpeed;
        };

        buffs.AddBuff(new Buff(name, isSingle, timeBuff, onStart, onEnd));
    }
}
