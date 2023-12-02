using DI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TestLvPair
{
    public int code, level;
}

[Serializable]
public class TestSetting
{
    public List<TestLvPair> statLevels;
    public int gold;
}

[Serializable]
public class StatValue
{
    public Status.Stat stat;
    public float value;
}

[Serializable]
public class GameSetting
{
    public List<StatValue> firstValues;

    public float GetFirstValue(Status.Stat stat)
    {
        var sv = firstValues.Find(l => l.stat == stat);
        if (sv == null)
        {
            return 0;
        }

        return sv.value;
    }
}

public class MainUiInstaller : MonoBehaviour
{
    DIContainer container;

    public EtcStrList etcStrList;
    public TestSetting TestSetting;

    public GameSetting gameSetting;

    private void Awake()
    {
        container = new DIContainer();

        DIContainer.AddContainer(container);
        container.Regist(gameSetting);

#if UNITY_EDITOR
        var userData = DIContainer.GetObjT<UserData>();

        foreach (var pair in TestSetting.statLevels)
            userData.statusCodePerLv[pair.code] = pair.level;

        userData.gold = TestSetting.gold;
#endif

        var playerStatGroup = new PlayerStatGroup();

        var pss = new PlayerStatByStatus();
        playerStatGroup.SetPlayerStat(PlayerStatGroup.Layer.FirstValue, new FirstStat());
        playerStatGroup.SetPlayerStat(PlayerStatGroup.Layer.Stat, pss);


        playerStatGroup.Init();


        container.Regist(etcStrList);
        container.Regist(pss);
        container.Regist(playerStatGroup);
    }

    private void OnDestroy()
    {
        DIContainer.RemoveContainer(container);
    }
}
