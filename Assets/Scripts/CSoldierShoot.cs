using UnityEngine;
using System.Collections;

public class CSoldierShoot : MonoBehaviour {

    CSoldierStat _stat;
    
    public Object _bulletPrefab;
    public Transform _attackPoint;

    void Awake()
    {
        _stat = GetComponent<CSoldierStat>();
    }

    public void Attack()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _attackPoint.position, transform.rotation) as GameObject;
        bullet.GetComponent<CBullet>().SetDamage(_stat._damage);
        bullet.GetComponent<Rigidbody>().velocity = _attackPoint.forward * 5.0f;
    }
}
