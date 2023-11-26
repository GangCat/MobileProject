using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData :ScriptableObject
{
    public List<Item> items;
    public List<Monster> monsters;
    public List<Stage> stages;
    public List<Skill> skills;


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
    }

}
