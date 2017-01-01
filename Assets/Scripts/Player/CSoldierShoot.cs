using UnityEngine;
using System.Collections;

public class CSoldierShoot : MonoBehaviour {

    CSoldierStat _stat;
    
    public GameObject _bulletPrefab;
    public Transform _attackPoint;

    void Awake()
    {
        _stat = GetComponent<CSoldierStat>();
    }

    public void Attack()
    {
        GameObject obj = CObjectPool.current.GetObject(_bulletPrefab);
        obj.transform.position = _attackPoint.transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
        obj.GetComponent<CBullet>().SetDamage(_stat._damage);
        obj.GetComponent<Rigidbody>().velocity = _attackPoint.forward * 8.0f;
        //GameObject bullet = Instantiate(_bulletPrefab, _attackPoint.position, transform.rotation) as GameObject;
        //bullet.GetComponent<CBullet>().SetDamage(_stat._damage);
        //bullet.GetComponent<Rigidbody>().velocity = _attackPoint.forward * 5.0f;
    }
}
