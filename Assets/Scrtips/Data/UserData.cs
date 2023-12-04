using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class EquipmentSlot
{
    public int count, level;

}


[Serializable]
public class UserData
{
    public Dictionary<int, int> skillCodePerLv = new Dictionary<int, int>();
    public Dictionary<int, int> statusCodePerLv = new Dictionary<int, int>();

    public Dictionary<int, EquipmentSlot> equipments = new Dictionary<int, EquipmentSlot>();
    public List<int> equippedSkillCodes = new List<int>();

    public List<int> equipSlots = new List<int>() { 0, 0, 0, 0 };

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
