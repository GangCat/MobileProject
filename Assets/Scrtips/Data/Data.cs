using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SocialPlatforms;

[Serializable]
public class Item
{
    // 엑셀의 구분의 이름과 동일하게 설정
    public int code;
    public string name;
    public int price;
    public int bbb;

    // Enum의 경우 공백이면 None이 들어감.
    public enum ItemType
    {
        None,
        Weapon,
        Material
    }
    public ItemType itemType;

    // 사용자 정의 자료형
    public Range lootRange;
}

// 값을 넣을 때 확인할 인터페이스
public interface IParsable
{
    public void FillFromStr(string str);
}

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
    public string name;
    public string path;

    public float health;
}

[Serializable]
public class Stage
{
    public int code;
    public string name;

    public IntList monsters;
    // 스테이지 정예몹
    public int eliteMonsterCode;
    // 스테이지 보스몹
    public int bossCode;
    List<Monster> _monsterList;
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
    public string name;

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
        _icon = Addressables.LoadAssetAsync<Sprite>(imagePath).WaitForCompletion();
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
    public string name;
    public string path;
    public float damage;
    public float health;
    public float attackRate;
}