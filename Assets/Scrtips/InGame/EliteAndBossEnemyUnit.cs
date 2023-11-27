using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitAnim;

public class EliteAndBossEnemyUnit : EnemyUnit
{
    // 보스나 엘리트의 경우 다른것보다 크기가 달라서 다르게 만듬.
    // 그리고 공격도 하기 때문에 확장하는게 더 나을 것이라 판단.
    // 나중에 보스나 엘리트만 템을 떨구게 할 생각. 재료같은거?
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
