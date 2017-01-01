using UnityEngine;
using System.Collections;

public class CEnemyDamage : MonoBehaviour {

    CapsuleCollider _col;
    CEnemyStat _stat;
    CEnemyAnimation _anim;
    CEnemyManager _manager;

    void Awake()
    {
        _col = GetComponent<CapsuleCollider>();
        _manager = GameObject.Find("Manager").GetComponent<CEnemyManager>();
        _stat = GetComponent<CEnemyStat>();
        _anim = GetComponent<CEnemyAnimation>();
    }

    public void Hit(int damage)
    {
        _stat.HpDown(damage);
        if (0 < _stat._hp)
        {
            _anim.PlayMultiLayerAnimation("Damage", 1);
        }
        else
        {
            _col.enabled = false;
            _stat._state = CEnemyStat.STATE.DEATH;
            _anim.PlayAnimation(CEnemyStat.STATE.DEATH);
        }
    }

    public void DeathAnimationComplete()
    {
        _manager.SendMessage("DeleteList", gameObject);
        Destroy(gameObject,1f);
    }
}
