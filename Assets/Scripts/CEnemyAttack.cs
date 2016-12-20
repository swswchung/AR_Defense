using UnityEngine;
using System.Collections;

public class CEnemyAttack : MonoBehaviour {

    CEnemyStat _stat;
    public Transform _attackPoint;

    void Awake()
    {
        _stat = GetComponent<CEnemyStat>();
    }

    void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_attackPoint.position,
        0.4f, 1 << LayerMask.NameToLayer("Player"));

        //피격 대상이 존재하면
        if (hitColliders.Length <= 0) return;

        //대상 피격 처리
        hitColliders[0].SendMessage("Hit", _stat._damage);
    }
}
