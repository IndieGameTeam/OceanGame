using System;

public class Buff
{
    public bool isSingle;
    public float timeBonus;
    public float timeLeft;

    public Action onStart;
    public Action onEnd;

    public Buff(bool isSingle, float timeBonus, Action onStart, Action onEnd)
    {
        this.isSingle = isSingle;
        this.timeBonus = timeBonus;
        timeLeft = timeBonus;

        this.onStart = onStart;
        this.onEnd = onEnd;
    }
}
