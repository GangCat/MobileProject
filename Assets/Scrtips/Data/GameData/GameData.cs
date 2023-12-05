using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 어플리케이션 종료시 호출
/// </summary>
public class ApplicationQuitEvent { }

public class GameData :ScriptableObject
{
    // 각종 정보들 저장.
    public List<Item> items;
    public List<Equipment> equipments;
    public List<Monster> monsters;
    public List<Stage> stages;
    public List<Skill> skills;
    public List<BossMonster> bossMonsters;
    public List<Status> status;
    public List<LvTable> lvTable;
    public List<Dungeon> dungeons;

    Dictionary<int, Skill> skillDic= new Dictionary<int, Skill>();




    public Skill GetSkill(int code)
    {
        return skillDic[code];
    }

    public void Init()
    {
        foreach(var s in stages)
        {
            s.Init(this);
        }

        foreach (var s in skills)
        {
            s.Init();
            skillDic[s.code] = s;
        }

        foreach (var e in equipments)
        {
            e.Init();
        }
        EventBus.Unsubscribe<ApplicationQuitEvent>(OnQuit);
        EventBus.Subscribe<ApplicationQuitEvent>(OnQuit);
    }

    public void Release()
    {
        foreach (var s in skills)
        {
            s.ReleaseIcon();
        }

        foreach(var e in equipments)
        {
            e.ReleaseIcon();
        }
    }

    private void OnQuit(ApplicationQuitEvent obj)
    {
        Release();

        EventBus.Unsubscribe<ApplicationQuitEvent>(OnQuit);
    }
}
