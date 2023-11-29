using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillListItemCell : ListItemCell<Skill>
{
    public override void SetData(Skill _data, int _idx)
    {
        text.text = _data.name;
        idx = _idx;
    }
}
