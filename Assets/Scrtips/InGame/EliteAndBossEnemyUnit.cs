using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitAnim;

public class EliteAndBossEnemyUnit : EnemyUnit
{
    BossMonster bossMobData;
    WaitForSeconds wait;

    float attackRate;
    float damage;
    float lastAttackTime;
    bool isAttack;


    public override void Init()
    {
        base.Init();
        wait = new WaitForSeconds(0.1f);
        isAttack = false;
    }

    protected internal void SetData(BossMonster mobData)
    {
        CheckAndInject();
        this.bossMobData = mobData;

        unitVisual = poolManager.GetObject(mobData.path).GetComponent<UnitVisual>();
        unitVisual.SetDeathAction(ReturnToPool);
        unitVisual.SetAttackAction(AttackHero);
        healthBar.transform.localPosition = unitVisual.hpBarTf.localPosition;
        UnitAnim.SetAnimator(unitVisual.GetComponent<Animator>());

        unitVisual.transform.SetParent(this.transform);
        unitVisual.transform.localScale = Vector3.one * 1.5f;
        unitVisual.SetPosition();

        attackRate = mobData.attackRate;
        damage = mobData.damage;
        maxHP = mobData.health;
        curHP = maxHP;

        StartCoroutine(AutoAttackCoroutine());
    }


    private IEnumerator AutoAttackCoroutine()
    {
        lastAttackTime = 0f;
        while (true)
        {
            if(curHP <= 0)
            {
                UnitAnim.PlayAni(AniKind.Dead);
                yield break;
            }

            // 공격범위 기즈모 만들기
            if(Vector3.SqrMagnitude(MainObjs.HeroUnit.transform.position - transform.position) > 1)
            {
                yield return wait;
                continue;
            }

            if(isAttack==false && Time.time - lastAttackTime > attackRate)
            {
                //공격 애니메이션 재생
                UnitAnim.PlayAni(AniKind.Attack);
                Debug.Log("Attack");
                isAttack = true;
            }
            else if(!isAttack)
            {
                UnitAnim.PlayAni(AniKind.Idle);
            }

            yield return wait;
        }
    }

    public void AttackHero()
    {
        Debug.Log("AttackHero");

        lastAttackTime = Time.time;
        if (MainObjs.HeroUnit == null)
        {
            isAttack = false;
            return;
        }

        MainObjs.HeroUnit.TakeDamage(damage);
        isAttack = false;
    }
}
