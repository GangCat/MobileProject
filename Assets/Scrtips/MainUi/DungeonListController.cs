using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonListController : ListController<Dungeon>
{
    [Inject]
    GameData gameData;

    protected override void Start()
    {
        base.Start();

        DIContainer.Inject(this);
        SetData(gameData.dungeons);
    }
}
