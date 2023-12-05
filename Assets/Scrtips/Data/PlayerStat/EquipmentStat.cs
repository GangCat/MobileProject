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

        // 레벨별 증가량 적용시키기.

        foreach(var st in equipment.statsPerLv)
        {
            ps.IncrStat(st.stat, st.val*level);
        }

        foreach(var st in equipment.stats)
        {
            ps.IncrStat(st.stat, st.val);
        }

        UpdateAllStat();
    }

    public override void UpdateAllStat()
    {
        SetStat(Status.Stat.Health, statPerEquipment.Sum(l => l.Value.GetStat(Status.Stat.Health)));
        SetStat(Status.Stat.Attack, statPerEquipment.Sum(l => l.Value.GetStat(Status.Stat.Attack)));

        //foreach (var ps in statPerEquipment.Values)
        //{
        //    IncrStat(Status.Stat.Health, ps.GetStat(Status.Stat.Health));
        //    IncrStat(Status.Stat.Attack, ps.GetStat(Status.Stat.Attack));
        //}
    }

}
