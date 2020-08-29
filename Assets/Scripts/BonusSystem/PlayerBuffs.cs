using System.Collections.Generic;

using UnityEngine;

public class PlayerBuffs : MonoBehaviour
{
    private List<Buff> buffs;

    public void AddBuff(Buff buff)
    {
        if (buff.isSingle)
        {
            Buff findBuff = buffs.Find(_buff => _buff == buff);

            if (findBuff != null)
            {
                findBuff.timeLeft = findBuff.timeBonus;
            }
            else
            {
                buff.onStart();

                buffs.Add(buff);
            }
        }
        else
        {
            buffs.Add(buff);
        }
    }

    public void RemoveBuff(Buff buff)
    {
        buff.onEnd();

        buffs.Remove(buff);
    }

    private void Start()
    {
        buffs = new List<Buff>();
    }

    private void Update()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].timeLeft -= Time.deltaTime;

            if (buffs[i].timeLeft <= 0)
                RemoveBuff(buffs[i]);
        }
    }
}
