using UnityEngine;
using System.Collections;

public class CSoldierDamage : MonoBehaviour {

    CapsuleCollider _col;
    CSoldierStat _stat;
    CSoldierAnimation _anim;
    CPlayerManager _playerManager;
    
    void Awake()
    {
        _col = GetComponent<CapsuleCollider>();
        _stat = GetComponent<CSoldierStat>();
        _anim = GetComponent<CSoldierAnimation>();
        _playerManager = GameObject.Find("Manager").GetComponent<CPlayerManager>();
    }

    void Hit(int damage)
    {
        _stat.HpDown(damage);
        _anim.PlayMultiLayerAnimation("Damage", 1);

        if (_stat._hp <= 0)
        {
            _stat._state = CSoldierStat.STATE.DEATH;
            _anim.PlayAnimation(CSoldierStat.STATE.DEATH);
        }
    }

    public void DeathAnimationComplete()
    {
        if (!gameObject.name.Equals("Player"))
        {
            _playerManager.SendMessage("DeleteList", gameObject);
            _col.enabled = false;
            Destroy(gameObject, 1f);
        }
        else
        {
            SendMessage("GameOver");
        }
    }
}
