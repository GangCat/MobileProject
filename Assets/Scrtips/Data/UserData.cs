using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class EquipmentSlot
{
    public int equipmentCode;
    public int count, level;
    public bool isEquipped;
}


[Serializable]
public class UserData
{
    public Dictionary<int, int> skillCodePerLv = new Dictionary<int, int>();
    public Dictionary<int, int> statusCodePerLv = new Dictionary<int, int>();
    // 모든 착용 가능한 장비들에 대한 정보.
    public Dictionary<int, EquipmentSlot> equipments = new Dictionary<int, EquipmentSlot>();

    public List<int> equippedSkillCodes = new List<int>();
    // 착용한 아이템의 코드를 각 요소로 지정.
    public List<int> equipSlots = new List<int>() { 0, 0, 0, 0, 0 };

    public int gold;

    public void IncrCurrency(CurrencyPair currency)
    {
        switch (currency.currencyType)
        {
            case CurrencyType.Gold:
                gold += currency.amount;
                break;
            case CurrencyType.DungeonKey:
                break;
        }
    }
}
