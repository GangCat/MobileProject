using DarkPixelRPGUI.Scripts.UI.Equipment;
using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipDetail : DIMono
{
    [Inject]
    UserData userData;

    [Inject]
    EquipmentStat equipStat;

    Equipment equipment;
    EquipmentSlot equipmentSlot;

    public Button equipButton;
    public int eqCode;
    public GameObject targetGo;

    public override void Init()
    {
        base.Init();
        gameObject.SetActive(false);
    }

    public void SetData(EquipmentSlot _slot, Equipment _equipment)
    {
        gameObject.SetActive(true);
        equipment = _equipment;
        equipmentSlot = _slot;
        SetButtonInteractable(_slot != null);
    }

    void SetButtonInteractable(bool _isSlot)
    {

        bool isEquipped = userData.equipSlots.Contains(equipment.code);
        if(isEquipped)
            equipButton.interactable = isEquipped;
        else
            equipButton.interactable = _isSlot;
    }

    public void OnEquipClick()
    {
        userData.equipSlots[(int)equipment.equipSlot] = equipment.code;
        equipStat.Equip(equipment, equipmentSlot.level);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            if (!IsPointOverThis()) // UGUI 객체 위를 클릭하지 않았을 때
            {
                // 특정 UGUI 객체 외부를 클릭했을 때 실행할 동작
                Debug.Log("Clicked outside the target object.");
                gameObject.SetActive(false);
            }
        }
    }

 

    List<RaycastResult> uiCheckResult = new List<RaycastResult>();
    private bool IsPointOverThis()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition;

        EventSystem.current.RaycastAll(eventDataCurrentPosition, uiCheckResult);


        return uiCheckResult.Any(l=>l.gameObject== targetGo);


    }
    /*
    private bool IsInsideTargetRect()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRect, Input.mousePosition, null, out localPoint);

        return targetRect.rect.Contains(localPoint);
    }
    */
}
