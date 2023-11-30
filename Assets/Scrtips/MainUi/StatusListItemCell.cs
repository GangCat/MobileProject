using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusListItemCell : ListItemCell<Status>
{
    public TMP_Text curLvText;
    public TMP_Text curValText;
    public Button incrLvBtn;

    [Inject]
    UserData userData;

    [Inject]
    PlayerStatGroup playerStatGroup;

    PlayerStat playerStat;

    [Inject]
    GameData gameData;

    InjectObj injectObj = new InjectObj();
    Status stat;

    private void Start()
    {
        incrLvBtn.onClick.AddListener(OnLvUpClick);
    }

    int StatLevel
    {
        get
        {
            if (userData.statusCodePerLv.TryGetValue(stat.code, out int level) == false)
                return 1;
            else
                return level;
        }
        set
        {
            userData.statusCodePerLv[stat.code] = value;
        }
    } 
    
    public override void SetData(Status _data, int _idx)
    {
        stat = _data;
        injectObj.CheckAndInject(this);

        playerStat = playerStatGroup.GetPlayerStat(PlayerStatGroup.Layer.Stat);

        UpdateStatInfo();

        idx = _idx;
    }

    /// <summary>
    /// 버튼 클릭시 발생하는 내용
    /// </summary>
    public void OnLvUpClick()
    {
        var curLvTable = GetCurLvTable();

        if (curLvTable == null)
            return;

        playerStat.IncrStat(stat.statKind, curLvTable.Incr);
        userData.gold -= curLvTable.costIncr;
        StatLevel = StatLevel + 1;
        UpdateStatInfo();
    }

    /// <summary>
    /// 표시될 내용 갱신
    /// </summary>
    void UpdateStatInfo()
    {
        text.text = stat.statKind.ToString();
        curLvText.text = StatLevel.ToString();
        curValText.text = playerStat.GetStat(stat.statKind).ToString();
    }

    LvTable GetCurLvTable()
    {

        var lvTables = gameData.lvTable.Where(l => l.code == stat.lvTableCode);
        foreach (LvTable lvTable in lvTables)
        {
            if (StatLevel > lvTable.endLv)
                continue;

            return lvTable;
        }

        Debug.LogError("레벨 테이블에 값이 존재하지 않습니다.");
        return null;
    }

    public void Pressed()
    {
        Debug.Log("Pressed!");
    }
}
