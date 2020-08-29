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

    //public override bool Equals(object obj)
    //{
    //    return base.Equals(obj);
    //}

    //public override int GetHashCode()
    //{
    //    return base.GetHashCode();
    //}

    //public override string ToString()
    //{
    //    return base.ToString();
    //}

    //public static bool operator ==(Buff a, Buff b)
    //{
    //    return a.name == b.name;
    //}

    //public static bool operator !=(Buff a, Buff b)
    //{
    //    return a.name != b.name ;
    //}
}
