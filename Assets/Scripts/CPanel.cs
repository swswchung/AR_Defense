using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class CPanel : MonoBehaviour {

    [SerializeField]
    public GameObject _unit;   //업그레이드 대상유닛 개조버튼을 누르면 해당 오브젝트의 정보가 자동으로 들어옴
    
    //Image////////////////////////
    public Image _hpBar;
    public Image _apBar;
    public Image _dpBar;
    //1////////////////////////////

    //Text/////////////////////////
    [SerializeField]
    Text _hpText;
    [SerializeField]
    Text _apText;
    [SerializeField]
    Text _dpText;
    [SerializeField]
    Text _requireGoldText;
    [SerializeField]
    Text _logText;
    //1/////////////////////////////////

    //scripts////////////////////////////
    public CPlayerManager _playerManager;
    public CTileManager _tileManager;
    CSoldierStat _stat;

    //1//////////////////////////////////

	void GetUnit(GameObject unit)
    {
        _stat = unit.GetComponent<CSoldierStat>();
        UpdateData();
        if (_stat._maxHp == _stat._hp)
        {
            _requireGoldText.text = "0";
        }
        else
        {
            _requireGoldText.text = ((_stat._maxHp - _stat._hp) * 2).ToString();
        }
        _unit = unit;
    }

    void UpdateData()
    {
        _hpText.text = _stat._maxHp.ToString();
        _dpText.text = _stat._defense.ToString();
        _apText.text = _stat._damage.ToString();


        float damagePer = 100.0f / _stat._damagelimit;
        float defensePer = 100.0f / _stat._defenselimit;
        float hpPer = 100.0f / _stat._hplimit;

        _hpBar.fillAmount = _stat._maxHp * (0.01f * hpPer);
        _apBar.fillAmount = _stat._damage * (0.01f * damagePer);
        _dpBar.fillAmount = _stat._defense * (0.01f * defensePer);
    }

    public void UpgradeHPButton()
    {
        if (_stat._maxHp < _stat._hplimit)
        {
            if (50 <= _playerManager.GetGold())
            {
                _stat.MaxHpUp();
                _playerManager.GoldDown(50);
            }
            else
            {
                _logText.text = "돈이 부족합니다";
            }
        }
        else
        {
            _logText.text = "HP가 최대치에 도달했습니다.";
        }
        UpdateData();
    }

    public void UpgradeAPButton()
    {
        if (_stat._damage < _stat._damagelimit)
        {
            if (50 <= _playerManager.GetGold())
            {
                _stat.APUp();
                _playerManager.GoldDown(50);
            }
            else
            {
                _logText.text = "돈이 부족합니다.";
            }
        }
        else
        {
            _logText.text = "공격력이 최대치에 도달했습니다.";
        }
        UpdateData();
    }

    public void UpgradeDPButton()
    {
        if (_stat._defense < _stat._defenselimit)
        {
            if ( 50 <= _playerManager.GetGold())
            {
                _stat.DPUp();
                _playerManager.GoldDown(50);
            }
            else
            {
                _logText.text = "돈이 부족합니다.";
            }
        }
        else
        {
            _logText.text = "방어력이 최대치에 도달했습니다.";
        }
        UpdateData();
    }

    public void RecoveryHPButton()
    {
        if (_stat._hp != _stat._maxHp)
        {
            if (int.Parse(_requireGoldText.text) <= _playerManager.GetGold())
            {
                _stat.HPRecovery();
                _playerManager.GoldDown(int.Parse(_requireGoldText.text));
                _requireGoldText.text = "0";
            }
            else
            {
                _logText.text = "돈이 부족합니다";
            }
        }
        else
        {
            _logText.text = "이미 최대체력입니다.";
        }
        UpdateData();
    }

    public void OnDestroyOKButtonClick()
    {
        // 해당 타일의 유닛 제거
        //GameManager오브젝트에는 CPlayerManager와 CEnemyManager 스크립트 둘다
        //DeleteList라는 함수를 가지고있어서 SendMessage함수 사용불가.
        _playerManager.GetComponent<CPlayerManager>().DeleteList(_unit);
        _tileManager.DeleteTileUnit(_unit);
        Destroy(_unit);
    }
}