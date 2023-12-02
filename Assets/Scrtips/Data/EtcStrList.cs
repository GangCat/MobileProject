using System.Collections.Generic;
using UnityEngine;

public class EtcStrList : ScriptableObject
{

    public List<EtcStr> etcStrList;
    public string GetStr(string code)
    {
        var str = etcStrList.Find(l => l.code == code);

        if (str == null)
        {
            return $"[{code}]";
        }

        return str.ToString();
    }
}