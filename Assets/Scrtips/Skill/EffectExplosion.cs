using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DI;

public class EffectExplosion : DIMono, IXCollision
{
    [Inject]
    MainObjs mainObjs;

    [Range(0.1f,6)]
    public float width;
    public float dmg;
    public float coolTime;
    public float attackDelay = 0f; // 데미지가 들어가는 주기
    public float attackCount; // 한 번 스킬 발동에 데미지를 주는 횟수

    public ParticleSystem particle;

    WaitForSeconds waitSkillDelay;

    public float CenterX => transform.position.x;
    public float Width => width;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(Width, 1));
    }

    public override void Init()
    {
        waitSkillDelay = new WaitForSeconds(attackDelay);
    }


    private void OnEnable()
    {
        CheckAndInject();
        StartCoroutine(ActivateSkillCoroutine());
        //ActivateSkill();
    }

    private void Update()
    {
        if (!particle.isPlaying)
            gameObject.SetActive(false);
    }

    private IEnumerator ActivateSkillCoroutine()
    {
        particle.Play();
        var x = this as IXCollision;
        int curAttackCnt = 0;

        while (curAttackCnt < attackCount)
        {
            foreach (var enemy in mainObjs.EnemyUnits)
            {
                if (x.IsCollide(enemy))
                    enemy.TakeDamage(dmg);
            }
            ++curAttackCnt;
            yield return waitSkillDelay;
        }
        

    }
}
