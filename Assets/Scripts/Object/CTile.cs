using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CTile : MonoBehaviour {

    public Renderer _renderer;

    [SerializeField]
    private bool _isBuild;

    public GameObject _unit;
    
    void Awake()
    {
        _unit = null;
        _renderer = GetComponent<Renderer>();
        Ini();
    }

    void Ini()
    {
        _renderer.material.color = new Color(0f, 1f, 0f, 0.5f);
    }

    public void ChangeMaterial(int num)
    {
        //1번 가능 2번 불가능
        if (num == 1)
        {
            _renderer.material.color = new Color(0.0f, 1.0f, 0.0f,0.5f);
            _isBuild = false;
        }
        else
        {
            _renderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            _isBuild = true;
        }
    }

    void ChangeMaterial()
    {
        _renderer.material.color = new Color(0.0f, 1.0f, 0.0f, 0.0f);
        _isBuild = false;
    }

    public bool GetIsBuild()
    {
        return _isBuild;
    }

    void AttachUnit(GameObject unit)
    {
        _unit = unit;
    }

    void DeleteUnit()
    {
        _unit = null;
        ChangeMaterial();
    }
}