using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Equipment;

public class EquipmentList : DIMono
{
    [Inject]
    GameData gameData;

    public Transform cellParents;

    public List<EquipmentListItemCell> cells = new List<EquipmentListItemCell>();

    public EquipmentListItemCell GetCell(int idx)
    {
        if(cells.Count > idx)
        {
            return cells[idx];
        }

        while (cells.Count <= idx)
        {
            var c= cellParents.GetChild(0);
            var cell=Instantiate(c,cellParents).GetComponent< EquipmentListItemCell>();
            cells.Add(cell);
        }

        return cells[idx];
    }

    public override void Init()
    {
        foreach (Transform c in cellParents)
        {
            var cell = c.GetComponent<EquipmentListItemCell>();
            cells.Add(cell);
        }

        SetInitData();
    }

    void SetInitData()
    {
        SetDataWithType(EquipSlot.Weapon);
        // 무기 정보로 데이터 세팅
    }

    public void SetDataWithIndex(int _idx)
    {
        SetDataWithType((EquipSlot)_idx);
    }

    public void SetDataWithType(EquipSlot _slotType)
    {
        // 타입에 맞는 데이터 세팅
        int idx = 0;

        foreach(var eq  in  gameData.equipments.Where(l=>l.equipSlot== _slotType))
        {
            var cell = GetCell(idx);
            cell.SetData(eq);
            idx++;
        }

        for(;idx < cells.Count ;++idx)
        {
            cells[idx].gameObject.SetActive(false);
        }
    }
}
