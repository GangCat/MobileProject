using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStat : PlayerStat
{
    [Inject]
    public GameSetting gameSetting;
    public override void UpdateAllStat()
    {
        foreach (var sv in gameSetting.firstValues)
        {
            IncrStat(sv.stat, sv.value);
        }
    }
}
