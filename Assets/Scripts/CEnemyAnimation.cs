using UnityEngine;
using System.Collections;

public class CEnemyAnimation : MonoBehaviour {

    CEnemyStat _stat;
    Animator _animator;
    void Awake()
    {
        _stat = GetComponent<CEnemyStat>();
        _animator = GetComponent<Animator>();
    }

    public bool IsPlayAnimation(string animName, int layer)
    {
        if (_animator.GetCurrentAnimatorStateInfo(layer).IsName(animName))
        {
            return true;
        }

        return false;
    }

    //데미지 애니메이션처리
    public void PlayMultiLayerAnimation(string animName, int layer)
    {
        //현재 애니메이션이 이미 재생중이면 무시
        if (_animator.GetCurrentAnimatorStateInfo(layer).IsName(animName)) return;

        _animator.Play(animName, layer);
    }

    public void PlayAnimation(CEnemyStat.STATE state)
    {
        _stat._state = state;

        switch (state)
        {
            case CEnemyStat.STATE.IDLE:
                if(IsPlayAnimation("Idle",0))
                {
                    break;
                }
                _animator.SetBool("Attack", false);
                _animator.SetBool("Move", false);
                break;
            case CEnemyStat.STATE.MOVE:
                if (IsPlayAnimation("Move", 0))
                {
                    break;
                }
                _animator.SetBool("Move",true);
                _animator.SetBool("Attack", false);
                break;
            case CEnemyStat.STATE.ATTACK:
                if (IsPlayAnimation("Attack", 0))
                {
                    break;
                }
                _animator.SetBool("Move", false);
                _animator.SetBool("Attack", true);
                break;
            case CEnemyStat.STATE.DEATH:
                if (IsPlayAnimation("Death", 0))
                {
                    break;
                }
                _animator.SetTrigger("Death");
                break;
        }
    }

    void CreateAnimationComplete()
    {
        PlayAnimation(CEnemyStat.STATE.MOVE);
    }
}
