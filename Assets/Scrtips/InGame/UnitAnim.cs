using System.Collections.Generic;
using Unity;
using UnityEngine;

public class UnitAnim
{
    public Animator animator;
    public AniKind currentAniKind;

    AniKind previousAniKind;
    bool isAttack = false;
    public bool IsAttack => isAttack;

    public enum AniKind
    {
        None,
        Idle,
        Walk,
        Attack,
        Dead
    }

    // 딕셔너리 인라인 초기화 방법
    Dictionary<AniKind, int> AnikindHash = new() {
        { AniKind.Idle, Animator.StringToHash("Idle")},
        { AniKind.Walk, Animator.StringToHash("Walk")},
        { AniKind.Attack, Animator.StringToHash("Attack")},
        { AniKind.Dead, Animator.StringToHash("Dead")},
    };

    // 메모리 풀의 경우 계속 생성하지 않고 한 번만 생성하되 animator를 갱신해주기 위해 별도로 추가함.
    public UnitAnim()
    {
    }

    public void SetAnimator(Animator anim)
    {
        this.animator = anim;
    }

    // 메모리 풀을 사용하지 않는 오브젝트의 경우 사용
    public UnitAnim(Animator anim )
    {
        this.animator = anim;
    }


    public void PlayAni(AniKind kind)
    {
        if (previousAniKind == kind)
            return;

        var curState = animator.GetCurrentAnimatorStateInfo(0);      
        // 공격 중일 경우 검사
        if (curState.shortNameHash == AnikindHash[AniKind.Attack])
        {
            // 현재 재생중인 공격의 0.9퍼센트 이상 진행이 되지 않은 경우 리턴
            if ((curState.normalizedTime%1) < 0.9f)
            {
                return;
            }
        }

        // 플레이어 공격 중 이동하지 못하게 하기 위한 조건
        isAttack = kind.Equals(AniKind.Attack) ? true : false;

        previousAniKind = kind;
        animator.Play(AnikindHash[kind]);
    }
}
