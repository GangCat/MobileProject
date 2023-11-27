using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuitEvent
{

}

public class ChallangeToBossStage
{

}

public class RestartCurrentStage
{

}

public class ChangeToNextStage
{

}

public class GameData :ScriptableObject
{
    public List<Item> items;
    public List<Monster> monsters;
    public List<Stage> stages;
    public List<Skill> skills;
    public List<BossMonster> bossMonsters;


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
        EventBus.Unsubscribe<ApplicationQuitEvent>(OnQuit);
        EventBus.Subscribe<ApplicationQuitEvent>(OnQuit);
    }

    private void OnQuit(ApplicationQuitEvent obj)
    {
        foreach (var s in skills)
        {
            s.ReleaseIcon();
        }

        EventBus.Unsubscribe<ApplicationQuitEvent>(OnQuit);
    }
}
