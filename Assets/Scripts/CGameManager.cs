using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CGameManager : MonoBehaviour {


    bool isGameOver;
    public int stage;

    //프리팹////////////////////////////////////
    public GameObject _player;
    public GameObject _soldier;
    //1///////////////////////////////////////

    /// 패널, 버튼/////////////////////////////
    public GameObject _buildMenu;
    public GameObject _qPanel; //업그레이드,판매용 패널
    public GameObject _workPanel;
    public GameObject _destroyPanel;
    public GameObject _upgradePanel;
    public GameObject _gameoverPanel;

    public GameObject _buildButton;
    public GameObject _startButton;
    ///////////////////////////////////
    
    //텍스트////////////////////////////
    public Text _buildText;
    public Text _logText;
    public Text _stageText;
    public Text _gameoverText;
    //1/////////////////////////////////

    //스크립트//////////////////////////
    CTileManager _tileManager;
    CPlayerManager _playerManager;
    [SerializeField]
    CEnemyManager _enemyManager;
    //1////////////////////////////////

    void Awake()
    {
        Screen.SetResolution(1280, 720, true);
        _tileManager = GetComponent<CTileManager>();
        _playerManager = GetComponent<CPlayerManager>();
        _enemyManager = GetComponent<CEnemyManager>();

        stage = 1;
    }

    void Start()
    {
        _stageText.text = stage.ToString();
        _tileManager._tileList[40].SendMessage("AttachUnit", _player);
    }
    
    public void OnBuildMenuClick()
    {
        //버튼을 누르면 생성메뉴로 바뀜, 이 상태에서는 게임시작불가
        if (!_buildMenu.activeInHierarchy)
        {
            _buildMenu.SetActive(true);
            _buildText.text = "Back";

            //배치중에는 시작불가로 변경
            _startButton.SetActive(false);
            SendMessage("ViewTile");
        }
        //한번 더 누르면 원상태로 돌아감
        else
        {
            _buildMenu.SetActive(false);
            _buildText.text = "Build";
            _startButton.SetActive(true);

            StopCoroutine("BuildSoliderCoroutine");
            SendMessage("DisableTile");
        }
    }

    public void OnBuildSoliderButtonClick()
    {
        _logText.text = "생성할 위치를 터치해주세요";
        
        StopCoroutine("BuildSoliderCoroutine");
        StartCoroutine("BuildSoliderCoroutine");
    }

    IEnumerator BuildSoliderCoroutine()
    {
        while (true)
        {
            if(Input.GetMouseButton(0) && !_qPanel.activeInHierarchy)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    int layer = hit.transform.gameObject.layer;
                    if (layer ==LayerMask.NameToLayer("Tile"))
                    {
                        bool isBuild = hit.transform.gameObject.GetComponent<CTile>().GetIsBuild();
                        //true면 이미 생성된곳, false면 생성가능
                        if(isBuild)
                        {
                            _qPanel.SetActive(true);
                            _workPanel.SetActive(true);
                            //타일에 생성된 유닛을 가져와서 패널에 넣음
                            _qPanel.SendMessage("GetUnit",hit.transform.gameObject.GetComponent<CTile>()._unit);
                        }
                        else
                        {
                            if (_playerManager.GetGold() < 100)
                            {
                                MessageBox("돈이 부족합니다.");
                                //StartCoroutine("ViewLogMessageCoroutine","돈이 부족합니다");
                            }
                            else
                            {
                                //터치한 자리에 오브젝트 생성
                                GameObject soldier = Instantiate(_soldier, transform.position, Quaternion.identity) as GameObject;
                                soldier.transform.SetParent(hit.transform);
                                soldier.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                                soldier.transform.SetParent(null);

                                //해당 타일에 오브젝트를 보냄(업그레이드 관리용)
                                hit.transform.gameObject.SendMessage("AttachUnit", soldier);

                                //터치한 자리의 타일색을 빨간색으로 변경 후 설치불가지역으로 만듬
                                hit.transform.gameObject.SendMessage("ChangeMaterial", 2);

                                _playerManager.GoldDown(100);

                                //오브젝트관리를 위해 매니저에 오브젝트를 리스트로 집어넣음
                                _playerManager.AddSoldier(soldier);

                                yield return new WaitForSeconds(0.5f);
                            }
                        }
                    }
                    else
                    {
                        MessageBox("유효한 장소를 터치해주세요");
                    }
                }
            }
            yield return null;
        }
    }

    public void OnEscapeButtonClick()
    {
        _workPanel.SetActive(false);
        _qPanel.SetActive(false);
    }

    public void OnDestroyButtonClick()
    {
        _workPanel.SetActive(false);
        _destroyPanel.SetActive(true);
    }

    //회수버튼은 Soldier 오브젝트가 필요하므로 CPanel스크립트에서 추가로 작성
    public void OnDestroyOKButtonClick()
    {
        // 골드 반환, 해당 타일의 유닛 제거
        _playerManager.GoldUp(100);
        _destroyPanel.SetActive(false);
        _qPanel.SetActive(false);
    }

    //회수 취소버튼
    public void OnDestroyCancelButtonClick()
    {
        //다시 워크패널로 돌아감
        _workPanel.SetActive(true);
        _destroyPanel.SetActive(false);
    }

    public void OnUpgradeButtonClick()
    {
        _workPanel.SetActive(false);
        _upgradePanel.SetActive(true);
    }

    public void OnUpgradeCancelButtonClick()
    {
        _workPanel.SetActive(true);
        _upgradePanel.SetActive(false);
    }

    void MessageBox(string msg)
    {
        StopCoroutine("ViewLogMessageCoroutine");
        StartCoroutine("ViewLogMessageCoroutine",msg);
    }

    public void OnGameStartButtomClick()
    {
        if (stage <= 27)
        {
            _enemyManager.EnemyGen(stage * 2);

            _startButton.SetActive(false);
            _buildButton.SetActive(false);

            SendMessage("UpdateEnemy");
        }
    }

    public void StageClear()
    {
        if (stage <= 26 && !isGameOver)
        {
            _startButton.SetActive(true);
            _buildButton.SetActive(true);
            stage++;
            _stageText.text = stage.ToString();
        }
        else if (stage == 27 && !isGameOver) 
        {
            _gameoverPanel.SetActive(true);
            _gameoverText.text = "GameClear";
        }
    }

    public void GameOver()
    {
        _gameoverPanel.SetActive(true);
        _gameoverText.text = "GameOver";
        isGameOver = true;
    }

    //메세지창 3초뒤 사라지게 설정
    IEnumerator ViewLogMessageCoroutine(string msg)
    {
        _logText.text = msg;

        yield return new WaitForSeconds(3.0f);

        _logText.text = "";
    }


    //게임종료시
    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
        OnApplicationQuit();
    }

    void OnApplicationQuit()
    {
        Application.CancelQuit();

#if !UNITY_EDITOR

        System.Diagnostics.Process.GetCurrentProcess().Kill();

#endif

        Application.Quit();
    }

}