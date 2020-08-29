using System;

using UnityEngine;

public class BonusData
{
    public string name;
    public bool isSingle;
    public float timeBonus;
    public float timeLeft;

    public Sprite bonusIcon;

    public Action onEnd;

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public static bool operator ==(BonusData a, BonusData b)
    {
        return a.name == b.name;
    }

    public static bool operator !=(BonusData a, BonusData b)
    {
        return a.name != b.name;
    }
}
