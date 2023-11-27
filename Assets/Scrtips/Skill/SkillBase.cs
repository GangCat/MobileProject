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

    /// <summary>
    /// �浹�˻縦 ���� �߽����� x�� ���̸� ����.
    /// </summary>
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
    }

    protected virtual void Update()
    {
        if (!particle.isPlaying)
            gameObject.SetActive(false);

    }

    // ��ų ���� �Ʒ��� ���� �˻�.
    protected virtual IEnumerator ActivateSkillCoroutine()
    {
        particle.Play();
        var x = this as IXCollision;
        int curAttackCnt = 0;

        // �ٴ���Ʈ�� ��� ó��
        while (curAttackCnt < attackCount)
        {
            for(int i = 0; i < mainObjs.EnemyUnits.Count; ++i)
            {
                var enemy = mainObjs.EnemyUnits[i];
                if (x.IsCollide(enemy))
                    enemy.TakeDamage(dmg);
            }
          
            ++curAttackCnt;
            yield return waitAttackDelay;
        }
    }
}
