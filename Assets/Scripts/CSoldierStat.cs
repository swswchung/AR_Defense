using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CSoldierStat : MonoBehaviour {

    public int _hplimit;
    public int _maxHp;
    public int _hp;

    public int _defenselimit;
    public int _defense;

    public int _damagelimit;
    public int _damage;

    public Image _hpBar;
    
    public enum STATE
    {
        IDLE,
        SHOOT,
        DAMAGE,
        DEATH
    }

    public STATE _state;

	// Use this for initialization
	void Start () {
        _hp = _maxHp;
	}

    public void HPRecovery()
    {
        _hp = _maxHp;
        UpdateHPBar();
    }
	
    public void MaxHpUp()
    {
        _maxHp += 5;
        _hp += 5;
    }

    public void HpDown(int damage)
    {
        _hp -= (damage - _defense);
        UpdateHPBar();
    }

    public void APUp()
    {
        _damage += 1;
    }

    public void DPUp()
    {
        _defense += 1;
    }

    void UpdateHPBar()
    {
        float per = 100.0f / _maxHp;
        _hpBar.fillAmount = (_hp * (0.01f * per));
    }
}
