using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using static UnitAnim;

public class HeroUnit : DIMono
{

    public float curHP;
    public bool isAttack;
    public UnitVisual unitVisual;

    [Inject]
    MainObjs MainObjs;

    [Inject]
    PlayerStatGroup playerStatGroup;

    [Inject]
    protected ObjectPoolManager poolManager;

    [Inject("DamageFontPath")]
    public string DamageFontPath;

    UnitAnim unitAnim;

    public Animator animator;

    public float HpRatio => curHP / MaxHp;

    public float MaxHp => playerStatGroup.GetStat(Status.Stat.Health);
    public float Damage => playerStatGroup.GetStat(Status.Stat.Attack);

    public override void Init()
    {
        unitAnim = new UnitAnim(animator);
        unitAnim.PlayAni(AniKind.Walk);
        curHP = MaxHp;
    }

    public void Update()
    {
        curHP -= Time.deltaTime * 100;

        // 적이 더이상 없는 경우
        if (MainObjs.EnemyUnits.Count < 1)
        {
            HeroWalk();
            return;
        }
        // 적이 충분히 멀리있는 경우
        if (Vector3.Distance(transform.position, MainObjs.EnemyUnits[0].transform.position) > 0.5f)
        {
            HeroWalk();
            return;
        }

        isAttack = true;
        unitAnim.PlayAni(AniKind.Attack);
    }


    private void HeroWalk()
    {
        isAttack = false;
        unitAnim.PlayAni(AniKind.Walk);
        if(!unitAnim.IsAttack)
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    public void AttackEnemy()
    {
        if (MainObjs.EnemyUnits.Count == 0)
            return;

        MainObjs.EnemyUnits[0].TakeDamage(Damage);
    }

    public void TakeDamage(float _dmg)
    {
        curHP -= _dmg;
        if (curHP <= 0)
        {
            unitAnim.PlayAni(AniKind.Dead);
        }

        //데미지 폰트를 오브젝트 풀링으로 관리하며 데미지 출력
        GameObject damageFontGo = poolManager.GetObject(DamageFontPath);
        damageFontGo.transform.position = unitVisual.damageFontTf.position;
        damageFontGo.GetComponent<DamageFont>().Show(_dmg, Color.white);

        StartCoroutine(unitVisual.DamageFx());
    }


    public float moveSpeed;
}
