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
    /// 유저 데이터의 스탯별 레벨을 가져오고 해당 레벨을 바탕으로 현재 캐릭터의 스테이터스 값을 계산해 넣어줌.
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
