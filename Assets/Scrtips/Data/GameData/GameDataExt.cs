using Codice.CM.Common.Merge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Status;

public static class GameDataExt
{

    public static (int,int) CalcIncrStat(this List<LvTable> tables,int lvTableCode,int currentLevel, int prevLevel)
    {
        var lvTables = tables.Where(l => l.code == lvTableCode);
        int accumulatedValue = 0;
        int accumulatedCost = 0;

        foreach (LvTable lvTable in lvTables)
        {
            if (lvTable.endLv < prevLevel)
                continue;

            if (currentLevel > lvTable.startLv && currentLevel <= lvTable.endLv)
            {
                int lvDifference;
                var diffStartLv = Mathf.Max(lvTable.startLv, prevLevel);

                lvDifference = currentLevel - diffStartLv;

                accumulatedValue += lvDifference * lvTable.Incr;
                accumulatedCost += lvDifference * lvTable.costIncr;
                break; // assuming only one matching range exists
            }
            else if (currentLevel > lvTable.endLv)
            {
                int lvRange;
                if (lvTable.startLv < prevLevel)
                    lvRange = lvTable.endLv - prevLevel + 1;
                else
                    lvRange = lvTable.endLv - lvTable.startLv + 1;

                accumulatedValue += lvRange * lvTable.Incr;
                accumulatedCost += lvRange * lvTable.costIncr;
            }
        }

        return (accumulatedValue, accumulatedCost);
    }

    public static int GetMaxLv(this List<LvTable> _tables, int lvTableCode)
    {
        return _tables.Where(l => l.code == lvTableCode).Max(l => l.endLv);
    }

    public static LvTable GetCurLvTable(this List<LvTable> _tables, int lvTableCode, int _curLv)
    {

        var lvTables = _tables.Where(l => l.code == lvTableCode);
        foreach (LvTable lvTable in lvTables)
        {
            if (_curLv > lvTable.endLv)
                continue;

            return lvTable;
        }

        Debug.LogError("레벨 테이블에 값이 존재하지 않습니다.");
        return null;
    }
}
