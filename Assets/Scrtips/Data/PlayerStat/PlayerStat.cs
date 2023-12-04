using DI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat
{
    protected Dictionary<Status.Stat, float> Stats = new Dictionary<Status.Stat, float>();


    public virtual void UpdateAllStat()
    {

    }

    public void Init()
    {
        DIContainer.Inject(this);
    }

    /// <summary>
    /// ���� �������� ���Ⱥ� ������ �������� �ش� ������ �������� ���� ĳ������ �������ͽ� ���� ����� �־���.
    /// </summary>


    public float GetStat(Status.Stat stat)
    {
        if (Stats.ContainsKey(stat) == false)
        {
            return 0;
        }
        return Stats[stat];
    }

    public void IncrStat(Status.Stat stat, float value)
    {
        if (Stats.ContainsKey(stat) == false)
        {
            Stats[stat] = 0;
        }
        Stats[stat] += value;

    }


}
