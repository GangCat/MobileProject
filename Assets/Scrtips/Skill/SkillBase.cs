using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DI;

public class SkillBase : DIMono, IXCollision
{
    [Inject]
    protected MainObjs mainObjs;

    [Range(0.1f,6)]
    public float width;
    public float dmg;
    public float coolTime;
    public float attackDelay = 0f; // �������� ���� �ֱ�
    public float attackCount; // �� �� ��ų �ߵ��� �������� �ִ� Ƚ��

    public ParticleSystem particle;

    protected WaitForSeconds waitAttackDelay;

    public float CenterX => transform.position.x;
    public float Width => width;

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(Width, 1));
    }

    public override void Init()
    {
        waitAttackDelay = new WaitForSeconds(attackDelay);
    }

    public void SetData(float _dmg, float _cooltime)
    {
        dmg = _dmg;
        coolTime = _cooltime;
    }


    protected virtual void OnEnable()
    {
        CheckAndInject();
        StartCoroutine(ActivateSkillCoroutine());
        //ActivateSkill();
    }

    protected virtual void Update()
    {
        if (!particle.isPlaying)
            gameObject.SetActive(false);
    }

    protected virtual IEnumerator ActivateSkillCoroutine()
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
            yield return waitAttackDelay;
        }
        

    }
}
