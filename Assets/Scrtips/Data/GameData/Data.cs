using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SocialPlatforms;
using static Status;

public enum CurrencyType
{
    Gold,
    DungeonKey
}

public enum SpaceType
{
    Stage,
    Dungeon
}


[Serializable]
public class StatValue : IParsable
{
    public Stat stat;
    //public double val;
    public float val;

    public void FillFromStr(string str)
    {
        var strArr = str.Split(',');
        stat = Enum.Parse<Stat>(strArr[0]);
        //val = double.Parse(strArr[1]);
        val = float.Parse(strArr[1]);
    }
}


[Serializable]
public class StatValueList : IParsable ,IList<StatValue>
{
    public List<StatValue> statValues = new List<StatValue>();

    public void FillFromStr(string str)
    {
        var strArr = str.Split('/');
        foreach (var curStr in strArr)
        {
            var statVal = new StatValue();
            statVal.FillFromStr(curStr);
            statValues.Add(statVal);
        }
    }


    public StatValue this[int index] { get => statValues[index]; set => statValues[index] =value; }

    public int Count => statValues.Count;

    public bool IsReadOnly => true;

    public void Add(StatValue item)
    {
        statValues.Add(item);
    }

    public void Clear()
    {
        statValues.Clear();
    }

    public bool Contains(StatValue item)
    {
        return statValues.Contains(item);
    }

    public void CopyTo(StatValue[] array, int arrayIndex)
    {
        statValues.CopyTo(array,arrayIndex);
    }

    public IEnumerator<StatValue> GetEnumerator()
    {
        return statValues.GetEnumerator();
    }

    public int IndexOf(StatValue item)
    {
        return statValues.IndexOf(item);
    }

    public void Insert(int index, StatValue item)
    {
        statValues.Insert(index, item);
    }

    public bool Remove(StatValue item)
    {
        return statValues.Remove(item);
    }

    public void RemoveAt(int index)
    {
        statValues.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return statValues.GetEnumerator();
    }
}

[Serializable]
public class Equipment
{
    public int code;
    public string name;

    public enum Grade
    {
        Normal,
        Rare,
        Unique,
        Epic,
        Legendary
    }

    public Grade grade;

    // Enum의 경우 공백이면 None이 들어감.
    public enum EquipSlot
    {
        Weapon,
        Armor,     
        Boots,
        Ring,
        Shield
    }
    public EquipSlot equipSlot;
    public StatValueList stats,statsPerLv;
    public string iconPath;
    public Sprite _icon;

    public void Init()
    {
        try
        {
            _icon = Addressables.LoadAssetAsync<Sprite>(iconPath).WaitForCompletion();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        _icon.name = $"{code}_{name}";
    }

    public void ReleaseIcon()
    {
        Addressables.Release(_icon);
        _icon = null;
    }
}

[Serializable]
public class Item
{
    // 엑셀의 구분의 이름과 동일하게 설정
    public int code;
    public string name;

    // Enum의 경우 공백이면 None이 들어감.
    public enum ItemType
    {
        None,
        Weapon,
        Material
    }
    public ItemType itemType;
}

// 값을 넣을 때 확인할 인터페이스


[Serializable]
public class Range: IParsable
{
    public int min, max;

    // 인터페이스의 메소드를 재정의
    public void FillFromStr(string str)
    {
        // 여러개일 경우 , 를 기준으로 string을 분할
        var strArr = str.Split(',');
        min = int.Parse(strArr[0]);
        max = int.Parse(strArr[1]);

    }
}

[Serializable]
public class Monster
{
    public int code;
    public Str name;
    public string path;

    public float health;
}

[Serializable]
public class Stage
{
    public int code;
    public Str name;
    public SpaceType type;

    public IntList monsters;
    // 스테이지 정예몹
    public int eliteMonsterCode;
    // 스테이지 보스몹
    public int bossCode;
    List<Monster> _monsterList;
    public string backgroundPath;
    public List<Monster> Monsters
    {
        get => _monsterList;

    }

    public void Init(GameData gamedata)
    {
        _monsterList = monsters.Select(code => {
            var mob = gamedata.monsters.Find(m => m.code == code);
            if (mob == null)
                throw new Exception($"Stage ({this.code}){name}의 monster code({code})가 존재하지 않습니다.");

            return mob;
        }).ToList();
           
    }
}

[Serializable]
public class Skill
{
    public int code;
    public Str name;

    public float damage;
    public float coolTime;

    public string imagePath;
    public string fxPrerfabPath;

    Sprite _icon;
    public Sprite IconImage
    {
        get => _icon;
    }

    /// <summary>
    /// 여기서 할당받은 icon이 해제가 안되는 것 같음.
    /// 종료하고 재실행할 때 icon에 할당된 값이 이미 존재함.
    /// </summary>
    public void Init()
    {
        Debug.Log("prevIcon:" + _icon);
        try
        {
            _icon = Addressables.LoadAssetAsync<Sprite>(imagePath).WaitForCompletion();
        }catch(Exception e)
        {
            Debug.LogException(e);
        }
        Debug.Log("curIcon:" + _icon);
        _icon.name = $"{code}_{name}";
    }

    public void ReleaseIcon()
    {
        Addressables.Release<Sprite>(_icon);
        _icon = null;
    }

}

[Serializable]
public class BossMonster
{
    public int code;
    public Str name;
    public string path;
    public float damage;
    public float health;
    public float attackRate;
}

[Serializable]
public class Status
{
    public int code;
    public Str desc;

    public enum Stat
    {
        Attack,
        Health
    }

    public Stat statKind;
    public int lvTableCode;
}

[Serializable]
public class LvTable
{
    public int code;
    public int startLv;
    public int endLv;
    public int Incr;
    public int costIncr;
}



[Serializable]
public class Dungeon
{
    public int code;
    public Str name;
    public Reward reward;
    public CurrencyPair dungeonCost;
    public int stageCode;
}

[Serializable]
public class Reward : IParsable
{
    public List<CurrencyPair> listRewards =new List<CurrencyPair>();

    public void FillFromStr(string str)
    {
        listRewards.Clear();
        // 여러개일 경우 , 를 기준으로 string을 분할
        var strArr = str.Split('/');
        foreach(var rewardStr in strArr)
        {
            var r = new CurrencyPair();
            r.FillFromStr(rewardStr);
            listRewards.Add(r);
        }
    }
}

[Serializable]
public class CurrencyPair : IParsable
{
    public CurrencyType currencyType;
    public int amount;

    public void FillFromStr(string str)
    {
        var rewardStrArr = str.Split(',');
        currencyType = Enum.Parse<CurrencyType>(rewardStrArr[0]);
        amount = int.Parse(rewardStrArr[1]);
    }

}
