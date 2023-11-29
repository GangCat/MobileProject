using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillListController : ListController<Skill>
{

    [Inject]
    GameData gameData;

    protected override void Start()
    {
        base.Start();
        DIContainer.Inject(this);

        SetData(gameData.skills);
    }

}
