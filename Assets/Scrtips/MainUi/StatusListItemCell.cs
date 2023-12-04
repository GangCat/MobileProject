using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatusListItemCell : ListItemCell<Status>
{
    public TMP_Text curLvText;
    public TMP_Text curValText;
    public PressedButton[] incrLvBtns;

    [Inject]
    UserData userData;

    [Inject]
    PlayerStatGroup playerStatGroup;

    PlayerStat playerStat;

    [Inject]
    GameData gameData;

    InjectObj injectObj = new InjectObj();
    Status stat;
    int maxLv;

    private void Start()
    {
        //incrLvBtn.onClick.AddListener(OnLvUpClick);
        int[] lvUpIncrs = { 1, 10, 100 };

        for(int idx = 0; idx < lvUpIncrs.Length; idx++)
            incrLvBtns[idx].whilePressed.AddListener(MakeOnLvUpClick(lvUpIncrs[idx]));
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
        maxLv = gameData.lvTable.GetMaxLv(this.stat.lvTableCode);

        playerStat = playerStatGroup.GetPlayerStat(PlayerStatGroup.Layer.Stat);

        UpdateButtonState();
        UpdateStatInfo();

        idx = _idx;
    }

    ///// <summary>
    ///// 버튼 클릭시 발생하는 내용
    ///// </summary>
    //public void OnLvUpClick()
    //{
    //    var curLvTable = gameData.lvTable.GetCurLvTable(this.stat.lvTableCode, StatLevel);

    //    if (curLvTable == null)
    //        return;

    //    playerStat.IncrStat(stat.statKind, curLvTable.Incr);
    //    userData.gold -= curLvTable.costIncr;
    //    StatLevel = StatLevel + 1;
    //    UpdateStatInfo();
    //}

    public UnityAction MakeOnLvUpClick(int _incrAmount)
    {
        return () =>
        {
            OnLvUpClick(_incrAmount);
        };
    }

    public void OnLvUpClick(int _incrAmount)
    {
        var curLvTable = gameData.lvTable.GetCurLvTable(this.stat.lvTableCode, StatLevel);

        if (curLvTable == null)
            return;
        var afterLv = StatLevel + _incrAmount;

        afterLv = Mathf.Min(afterLv, maxLv);

        var incrStat = gameData.lvTable.CalcIncrStat(this.stat.lvTableCode, afterLv, StatLevel);

        if (userData.gold < incrStat.Item2)
        {
            EventBus.Publish(new ErrorMessageEvent("NotEnoughGold"));
            return;
        }

        playerStat.IncrStat(stat.statKind, incrStat.Item1);

        userData.gold -= incrStat.Item2;
        StatLevel = afterLv;


        UpdateButtonState();
        UpdateStatInfo();
    }

    void UpdateButtonState()
    {
        var isMaxLv = StatLevel == maxLv;

        foreach (var btn in incrLvBtns)
            btn.interactable = !isMaxLv;
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

}
