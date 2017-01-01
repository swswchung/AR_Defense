using UnityEngine;
using System.Collections;

public class CSoldierAnimation : MonoBehaviour {

    CSoldierStat _stat;
    Animator _animator;

	void Awake()
    {
        _animator = GetComponent<Animator>();
        _stat = GetComponent<CSoldierStat>();
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

    public void PlayAnimation(CSoldierStat.STATE state)
    {
        _stat._state = state;

        switch (state)
        {
            case CSoldierStat.STATE.SHOOT:
                if (IsPlayAnimation("Shoot", 0))
                {
                    break;
                }
                _animator.SetTrigger("Shoot");
                break;
            case CSoldierStat.STATE.DEATH:
                if(IsPlayAnimation("Death",0))
                {
                    break;
                }
                _animator.SetTrigger("Death");
                break;
        }
    }
}
