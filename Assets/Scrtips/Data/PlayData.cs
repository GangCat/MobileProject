using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayData 
{
    /// <summary>
    /// ���� ���������� ���� ������ ����
    /// </summary>
    public Stage currentStage;

    /// <summary>
    /// ��ų�� �ܿ� ��Ÿ���� ����
    /// </summary>
    public Dictionary<int, float> leftCooltimes =new Dictionary<int, float>();

    /// <summary>
    /// ���� ���������� ������ ������ ������������ �˷��ִ� �뵵
    /// </summary>
    public bool isBossStage;
}
/// <summary>
/// ���� ��ҿ��� �����ϱ� ���� Ŭ����
/// </summary>
public class MainObjs
{
    /// <summary>
    /// ���� �ʿ� �����ϴ� ������ ����
    /// </summary>
    public List<EnemyUnit> EnemyUnits { get; set; }

    /// <summary>
    /// ���� ������ ������ ����
    /// </summary>
    public HeroUnit HeroUnit { get; internal set; }
}




public class PlayerStat
{
    Dictionary<Status.Stat, float> Stats = new Dictionary<Status.Stat, float>();

    [Inject]
    public GameData gamedata;

    [Inject]
    public UserData userdata;
        
    public void Init()
    {
        DIContainer.Inject(this);
    }

    /// <summary>
    /// ���� �������� ���Ⱥ� ������ �������� �ش� ������ �������� ���� ĳ������ �������ͽ� ���� ����� �־���.
    /// </summary>
    public void UpdateAllStat()
    {
        var statusDict = userdata.statusCodePerLv;
        foreach(var kp in statusDict)
        {
            int statCode=kp.Key;
            var lv = kp.Value;

            var statInfo=gamedata.status.Find(l => l.code == statCode);
            var val= CalculateAccumulatedValue(lv, statInfo.lvTableCode);

            IncrStat(statInfo.statKind, val);

        }
    }

    /// <summary>
    /// ���̺��� Ȯ���Ͽ� ���� ������ ��� ���� ������.
    /// </summary>
    /// <param name="currentLv"></param>
    /// <param name="lvTableCode"></param>
    /// <returns></returns>
    public int CalculateAccumulatedValue(int currentLv, int lvTableCode)
    {
        var lvTables=gamedata.lvTable.Where(l => l.code == lvTableCode);
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


    public float GetStat(Status.Stat stat)
    {
        if (Stats.ContainsKey(stat) == false)
        {
            return 0;
        }
        return Stats[stat];
    }

    public void IncrStat(Status.Stat stat,float value)
    {
        if (Stats.ContainsKey(stat) == false)
        {
            Stats[stat] = 0;
        }
        Stats[stat] += value;

    }
}