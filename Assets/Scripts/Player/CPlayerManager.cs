using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class CPlayerManager : MonoBehaviour {

    public Text _moneyText;
    private int money;

    [SerializeField]
    List<GameObject> _soldierList;

    CEnemyManager _enemyManager;
    CTileManager _tileManager;

    void Awake()
    {
        _enemyManager = GetComponent<CEnemyManager>();
        _tileManager = GetComponent<CTileManager>();
    }

	// Use this for initialization
	void Start () {
        _soldierList.Clear();
        money = 1000;
        UpdateMoney();
        _soldierList.Add(GameObject.Find("Player"));
    }
	
    public void GoldUp(int gold)
    {
        money += gold;
        UpdateMoney();
    }
    
    public void GoldDown(int gold)
    {
        money -= gold;
        UpdateMoney();
    }

    public int GetGold()
    {
        return money;
    }

    void UpdateMoney()
    {
        _moneyText.text = money.ToString();
    }

    public void AddSoldier(GameObject soldier)
    {
        _soldierList.Add(soldier);
    }

    public void DeleteList(GameObject unit)
    {
        _tileManager.DeleteTileUnit(unit);
        _soldierList.Remove(unit);
    }

    //게임 시작시 EnemyManager로 리스트보냄
    public List<GameObject> GetSoldier()
    {
        return _soldierList;
    }

    //게임 시작시 실행
    public void UpdateEnemy()
    {
        List<GameObject> _enemy = _enemyManager.GetEnemy();

        for (int i = 0 ; i < _soldierList.Count ; i++)
        {
            _soldierList[i].SendMessage("UpdateEnemyObject", _enemy);
        }
    }
}