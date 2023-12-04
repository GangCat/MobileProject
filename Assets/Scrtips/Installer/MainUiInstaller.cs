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

    // �������� �޾ƿ� �������� ������ ���� ����Ʈ
    public List<EquipmentSlot> initEquipSlots = new List<EquipmentSlot>();

    private void Awake()
    {
        container = new DIContainer();

        DIContainer.AddContainer(container);
        container.Regist(gameSetting);

        var gameData = DIContainer.GetObjT<GameData>();
        var userData = DIContainer.GetObjT<UserData>();

#if UNITY_EDITOR


        foreach (var pair in TestSetting.statLevels)
            userData.statusCodePerLv[pair.code] = pair.level;

        userData.gold = TestSetting.gold;
#endif

        var playerStatGroup = new PlayerStatGroup();

        var playerStatByStat = new PlayerStatByStatus();
        playerStatGroup.SetPlayerStat(PlayerStatGroup.Layer.FirstValue, new FirstStat());
        playerStatGroup.SetPlayerStat(PlayerStatGroup.Layer.Stat, playerStatByStat);
        
        // ���⼭ ��� SetPlayerStat
        var playerEquipStat = new EquipmentStat();
        foreach (var eq in initEquipSlots)
        {
            var equipment = gameData.equipments.Find(l => l.code == eq.equipmentCode);
            playerEquipStat.Equip(equipment, eq.level);
            userData.equipSlots[(int)equipment.equipSlot] = equipment.code;
        }
        playerStatGroup.SetPlayerStat(PlayerStatGroup.Layer.Equipment, playerEquipStat);

        // �� ���̾ ���� ��ġ ����.
        playerStatGroup.Init();

        container.Regist(etcStrList);
        container.Regist(playerStatByStat);
        container.Regist(playerEquipStat);
        container.Regist(playerStatGroup);
    }

    private void OnDestroy()
    {
        DIContainer.RemoveContainer(container);
    }
}
