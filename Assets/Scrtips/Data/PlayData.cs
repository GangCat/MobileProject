using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayData 
{
    /// <summary>
    /// 현재 스테이지의 각종 정보를 저장
    /// </summary>
    public Stage currentStage;

    /// <summary>
    /// 스킬의 잔여 쿨타임을 관리
    /// </summary>
    public Dictionary<int, float> leftCooltimes =new Dictionary<int, float>();

    /// <summary>
    /// 현재 스테이지가 보스가 출현할 스테이지인지 알려주는 용도
    /// </summary>
    public bool isBossStage;
}
/// <summary>
/// 여러 장소에서 접근하기 위한 클래스
/// </summary>
public class MainObjs
{
    /// <summary>
    /// 현재 맵에 존재하는 적들을 저장
    /// </summary>
    public List<EnemyUnit> EnemyUnits { get; set; }

    /// <summary>
    /// 영웅 유닛의 정보를 저장
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
    /// 유저 데이터의 스탯별 레벨을 가져오고 해당 레벨을 바탕으로 현재 캐릭터의 스테이터스 값을 계산해 넣어줌.
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
    /// 테이블을 확인하여 현재 레벨일 경우 값을 가져옴.
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