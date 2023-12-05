using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentListItemCell : DIMono
{
    public Image iconImage;

    public Equipment data;

    public EquipDetail equipDetail;

    public EquipmentSlot equipmentSlot;

    [Inject]
    UserData userData;
    public override void Init()
    {
        base.Init();
        GetComponent<Button>().onClick.AddListener(OnClick);

    }

    public void OnClick()
    {
        equipDetail.SetData(equipmentSlot, data);
    }


    public void SetData(Equipment _data)
    {
        CheckAndInject();
        this.data = _data;
        iconImage.sprite = _data._icon;
        gameObject.SetActive(true);
        equipmentSlot=null;
        userData.equipments.TryGetValue(_data.code,out equipmentSlot);

    }

    
}
