using System;

public class Buff
{
    public string name;
    public bool isSingle;
    public float timeBonus;
    public float timeLeft;

    public Action onStart;
    public Action onEnd;

    public Buff(string name, bool isSingle, float timeBonus, Action onStart, Action onEnd)
    {
        this.name = name;
        this.isSingle = isSingle;
        this.timeBonus = timeBonus;
        timeLeft = timeBonus;

        this.onStart = onStart;
        this.onEnd = onEnd;
    }
}
