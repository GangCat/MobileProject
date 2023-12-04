﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class UserData
{
    public Dictionary<int, int> skillCodePerLv = new Dictionary<int, int>();
    public Dictionary<int, int> statusCodePerLv = new Dictionary<int, int>();
    public List<int> equippedSkillCodes = new List<int>();

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
