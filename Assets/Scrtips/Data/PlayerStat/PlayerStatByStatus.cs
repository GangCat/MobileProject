using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatByStatus : PlayerStat
{
    [Inject]
    public GameData gamedata;

    [Inject]
    public UserData userdata;

    public override void UpdateAllStat()
    {
        var statusDict = userdata.statusCodePerLv;
        foreach (var kp in statusDict)
        {
            int statCode = kp.Key;
            var lv = kp.Value;

            var statInfo = gamedata.status.Find(l => l.code == statCode);
            var val = CalculateAccumulatedValue(lv, statInfo.lvTableCode);

            IncrStat(statInfo.statKind, val);
        }
    }

    /// <summary>
    /// 테이블을 확인하여 현재 레벨일 경우 값을 가져옴.
    /// </summary>
    /// <param name="currentLv"></param>
    /// <param name="lvTableCode"></param>
    /// <returns></returns>
    public int CalculateAccumulatedValue(int currentLv, int lvTableCode)
    {
        var lvTables = gamedata.lvTable.Where(l => l.code == lvTableCode);
        int accumulatedValue = 0;

        foreach (LvTable lvTable in lvTables)
        {
            if (currentLv > lvTable.startLv && currentLv <= lvTable.endLv)
            {
                int lvDifference = currentLv - lvTable.startLv;
                accumulatedValue += lvDifference * lvTable.Incr;
                break; // assuming only one matching range exists
            }
            else if (currentLv > lvTable.endLv)
            {
                int lvRange = lvTable.endLv - lvTable.startLv + 1;
                accumulatedValue += lvRange * lvTable.Incr;
            }
        }

        return accumulatedValue;
    }

}
