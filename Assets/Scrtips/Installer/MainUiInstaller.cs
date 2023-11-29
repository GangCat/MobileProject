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

public class MainUiInstaller : MonoBehaviour
{
    DIContainer container;

    public TestSetting TestSetting;

    private void Awake()
    {
        container = new DIContainer();

        DIContainer.AddContainer(container);


#if UNITY_EDITOR
        var userData = DIContainer.GetObjT<UserData>();

        foreach (var pair in TestSetting.statLevels)
            userData.statusCodePerLv[pair.code] = pair.level;

        userData.gold = TestSetting.gold;
#endif

        var playerStat = new PlayerStat();
        playerStat.Init();
        playerStat.UpdateAllStat();
        container.Regist(playerStat);
    }

    private void OnDestroy()
    {
        DIContainer.RemoveContainer(container);
    }
}
