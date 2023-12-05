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
public class CodeAndEquipmentSlot
{
    public int code;
    public EquipmentSlot slot;
}


[Serializable]
public class TestSetting
{
    public List<TestLvPair> statLevels;
    public int gold;
     
    public List<CodeAndEquipmentSlot> initEquipments = new List<CodeAndEquipmentSlot>();
    public List<int> initEquipSlots = new List<int>();

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

        var gameData = DIContainer.GetObjT<GameData>();
        var userData = DIContainer.GetObjT<UserData>();

#if UNITY_EDITOR
        userData.equipSlots = TestSetting.initEquipSlots;
        var eq = new Dictionary<int, EquipmentSlot>();

        foreach(var codeAndSlot in TestSetting.initEquipments)
        {
            eq[codeAndSlot.code] = codeAndSlot.slot;
        }

        userData.equipments = eq;

        foreach (var pair in TestSetting.statLevels)
            userData.statusCodePerLv[pair.code] = pair.level;

        userData.gold = TestSetting.gold;
#endif


        container.Regist(etcStrList);

        InitAndRegistPlayerStat(gameData, userData);
    }

    private void InitAndRegistPlayerStat(GameData gameData, UserData userData)
    {
        var playerStatGroup = new PlayerStatGroup();

        var playerStatByStat = new PlayerStatByStatus();
        playerStatGroup.SetPlayerStat(PlayerStatGroup.Layer.FirstValue, new FirstStat());
        playerStatGroup.SetPlayerStat(PlayerStatGroup.Layer.Stat, playerStatByStat);
        var playerEquipStat = new EquipmentStat();
        foreach (var eCode in userData.equipSlots)
        {
            var equipmentSlot = userData.equipments[eCode];
            var equipment = gameData.equipments.Find(l => l.code == eCode);

            playerEquipStat.Equip(equipment, equipmentSlot.level);
        }
        playerStatGroup.SetPlayerStat(PlayerStatGroup.Layer.Equipment, playerEquipStat);

        // 각 레이어별 스탯 수치 적용.
        playerStatGroup.Init();

        container.Regist(playerStatByStat);
        container.Regist(playerEquipStat);
        container.Regist(playerStatGroup);
    }

    private void OnDestroy()
    {
        DIContainer.RemoveContainer(container);
    }
}
