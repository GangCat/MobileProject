using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomStat : PlayerStat
{

    System.Action _updateAllStat;
    public CustomStat(System.Action UpdateAllStat)
    {
        this._updateAllStat = UpdateAllStat;
    }

    public override void UpdateAllStat()
    {
        _updateAllStat();
    }
}
