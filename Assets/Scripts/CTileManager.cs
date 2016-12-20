using UnityEngine;

using System.Collections.Generic;
using System.Collections;

public class CTileManager : MonoBehaviour {
    
    public GameObject _tile;//타일 프리팹

    public GameObject _tiles;//타일들 저장용 오브젝트

    public List<GameObject> _tileList;

    public CGameManager _gameManager;

    void Awake()
    {
        CreateTile();
    }

    void Start()
    {
        DisableTile();
    }

    void CreateTile()
    {
        for (int y = 9; 0 < y; y--)
        {
            for (int x = 0; x < 9; x++)
            {
                GameObject tile = Instantiate(_tile, new Vector3(x - 4, 0f, y - 5), Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
                _tileList.Add(tile);

                if(x == 4 && y == 5)//가운데 플레이어본체가 있는 자리에는 건설금지
                {
                    _tileList[_tileList.Count - 1].SendMessage("ChangeMaterial", 2);
                }
            }
        }

        for (int i = 0 ; i < _tileList.Count ;i++)
        {
            _tileList[i].transform.SetParent(_tiles.transform);
        }
    }

    void ViewTile()
    {
        Color color;
        for (int i = 0; i < _tileList.Count; i++)
        {
            //_tileList[i].SetActive(true);
            color = _tileList[i].GetComponent<CTile>()._renderer.material.color;
            _tileList[i].GetComponent<CTile>()._renderer.material.color = new Color(color.r, color.g, color.b, 0.5f);
            
        }
    }

    void DisableTile()
    {
        Color color;
        for (int i = 0 ; i < _tileList.Count ; i++)
        {
            //_tileList[i].SetActive(false);
            color = _tileList[i].GetComponent<CTile>()._renderer.material.color;
            _tileList[i].GetComponent<CTile>()._renderer.material.color = new Color(color.r, color.g, color.b, 0.0f);
        }
    }

    void SetMaterial(int x, int y, int color)
    {
        //9 = 한줄에 존재하는 타일들의 수 ex) x = 5, y = 5면 5번쨰줄에서 5번째
        //맨 마지막 +1은 배열이 0부터시작해서
        //int num = (9 * y) + x + 1;

        //_tileList[num].SendMessage("ChangeMaterial", color);

        //SendMessage("ChangeMaterial");
    }

    public void DeleteTileUnit(GameObject unit)
    {
        for (int i = 0; i < _tileList.Count; i++)
        {
            if (_tileList[i].GetComponent<CTile>()._unit == unit)
            {
                _tileList[i].SendMessage("DeleteUnit");
            }
        }
    }
}
