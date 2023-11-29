using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 어플리케이션 종료시 호출
/// </summary>
public class ApplicationQuitEvent { }
/// <summary>
/// 보스 스테이지로 이동
/// </summary>
public class ChallangeToBossStage { }
/// <summary>
/// 현재 스테이지 재시작
/// </summary>
public class RestartCurrentStage { }
/// <summary>
/// 다음 스테이지로 이동
/// </summary>
public class ChangeToNextStage { }

public class GameData :ScriptableObject
{
    // 각종 정보들 저장.
    public List<Item> items;
    public List<Monster> monsters;
    public List<Stage> stages;
    public List<Skill> skills;
    public List<BossMonster> bossMonsters;
    public List<Status> status;
    public List<LvTable> lvTable;


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
