using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitAnim;

public class StageBossEnemyUnit : EnemyUnit
{
    BossMonster bossMobData;
    WaitForSeconds wait;

    float attackRate;
    float damage;
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
        float lastAttackTime = 0f;
        while (true)
        {
            
            if(Vector3.SqrMagnitude(MainObjs.HeroUnit.transform.position - transform.position) > Mathf.Pow(0.1f, 2f))
            {
                yield return wait;
                continue;
            }

            if(Time.time - lastAttackTime > attackRate)
            {
                //공격 애니메이션 재생
                UnitAnim.PlayAni(AniKind.Attack);
                isAttack = true;
                lastAttackTime = Time.time;
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
        if (MainObjs.HeroUnit == null)
        {
            isAttack = false;
            return;
        }

        MainObjs.HeroUnit.TakeDamage(damage);
        isAttack = false;
    }
}
