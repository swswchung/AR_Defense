using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class CEnemyStat : MonoBehaviour {

    public int _hp;
    public int _damage;

    public Image _hpBar;

    public enum STATE
    {
        CREATE,
        IDLE,
        MOVE,
        ATTACK,
        DEATH
    };

    public STATE _state;
	// Use this for initialization
	
    void Start()
    {
        _hp = 50;
    }

    public void SetDamage(int damage)
    {
        _damage += damage;
    }

    public void HpDown(int damage)
    {
        _hp -= damage;
        _hpBar.fillAmount = (_hp * 0.01f);
        if(_hp <= 0)
        {
            _state = STATE.DEATH;
        }
    }
}