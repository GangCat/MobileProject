using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBullet : SkillBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        isArriveDest = false;
        Invoke("Arrived", 0.5f);
    }

    public void Arrived()
    {
        isArriveDest = true;
    }

    protected override IEnumerator ActivateSkillCoroutine()
    {
        particle.Play();
        var x = this as IXCollision;

        // 다단히트의 경우 처리
        while (!isArriveDest)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            

            yield return null;
        }

        for (int i = 0; i < mainObjs.EnemyUnits.Count; ++i)
        {
            var enemy = mainObjs.EnemyUnits[i];
            if (x.IsCollide(enemy))
                enemy.TakeDamage(dmg);
        }
    }

    bool isArriveDest;
    public float moveSpeed;
}
