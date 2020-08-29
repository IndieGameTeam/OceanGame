using UnityEngine;

public class BonusSlowTime : PickupObject
{
    public float multiplyTime = 0.6f;
    public float timeSlow = 5f;

    public bool isSingleBuff = true;

    protected override void OnPickup(GameManager manager)
    {
        Buff buff = new Buff(isSingleBuff, timeSlow, 
            () => Time.timeScale = multiplyTime, 
            () => Time.timeScale = 1);

        FindObjectOfType<PlayerBuffs>().AddBuff(buff);

        //Ждём реворка пула, пока задестрою
        Destroy(gameObject);
    }
}
