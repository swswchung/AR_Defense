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
    
    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 2.0f);
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
