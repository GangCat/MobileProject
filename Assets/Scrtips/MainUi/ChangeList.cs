using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeList : MonoBehaviour
{
    public GameObject[] listGos;

    public void ChangeToStatList()
    {
        ChangeUiList(0);
    }

    public void ChangeToEquipList()
    {
        ChangeUiList(1);
    }

    public void ChangeToSkillList()
    {
        ChangeUiList(2);
    }

    public void ChangeToWorldList()
    {
        ChangeUiList(3);
    }

    public void ChangeToShopList()
    {
        ChangeUiList(4);
    }
    public void ChangeUiList(int _idx)
    {
        foreach (var go in listGos)
            go.SetActive(false);

        listGos[_idx].SetActive(true);
    }


}
