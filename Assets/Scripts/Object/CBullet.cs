using UnityEngine;
using System.Collections;

public class CBullet : MonoBehaviour {

    Rigidbody _rigidBody;

    [SerializeField]
    int _damage;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        Invoke("Die",2f);
    }

    void OnDisable()
    {
        CancelInvoke("Die");
        _rigidBody.velocity = Vector2.zero;
    }
    
    void Die()
    {
        CObjectPool.current.PoolObject(gameObject);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag.Equals("Enemy"))
        {
            col.SendMessage("Hit",_damage);
            
            Destroy(gameObject);
        }
    }
}
