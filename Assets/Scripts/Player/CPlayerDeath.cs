using UnityEngine;
using System.Collections;

public class CPlayerDeath : MonoBehaviour {

    public CGameManager _manager;
    
    void GameOver()
    {
        _manager.GameOver();
    }
}
