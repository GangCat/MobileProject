using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class DungeonListItemCell : ListItemCell<Dungeon>
{
    public Button enterDungeonButton;

    Dungeon currentDungeon;

    private void Start()
    {
        enterDungeonButton.onClick.AddListener(EnterDungeon);
    }

    public override void SetData(Dungeon _data, int _idx)
    {
        currentDungeon = _data;
        text.Str = _data.name;
        idx = _idx;
    }

    void EnterDungeon()
    {
        EventBus.Publish(new EnterToDungeon()
        {
            dungeon = currentDungeon
        });
    }
}
