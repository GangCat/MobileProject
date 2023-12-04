using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Equipment;

public class EquipmentStat : PlayerStat
{

    public Dictionary<EquipSlot, PlayerStat> statPerEquipment = new Dictionary<EquipSlot, PlayerStat>();

    public EquipmentStat()
    {
    }

    public void Equip(Equipment equipment, int level)
    {
        PlayerStat ps = new PlayerStat();

        statPerEquipment[equipment.equipSlot] = ps;
        foreach(var st in equipment.stats)
        {

        }

        UpdateAllStat();
    }



    public override void UpdateAllStat()
    {

    }
}
