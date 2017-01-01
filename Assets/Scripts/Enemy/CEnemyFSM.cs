using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CEnemyFSM : MonoBehaviour {

    CEnemyStat _stat;
    CEnemyAnimation _anim;
    NavMeshAgent _nav;
    
    public float _attackDist;

    [SerializeField]
    List<Transform> _playerUnit;

    [SerializeField]//test
    GameObject Target;

    void Awake()
    {
        _stat = GetComponent<CEnemyStat>();
        _anim = GetComponent<CEnemyAnimation>();
        _nav = GetComponent<NavMeshAgent>();
    }

	// Use this for initialization
	void Start () {
        _nav.Stop();

        StartCoroutine("MonsterFSMCoroutine");
	}

    //시작할때마다 매니저에서 유닛리스트를뽑아서 가져온 후 
    public void PlayerUnitPos(List<GameObject> units)
    {
        for(int i = 0 ; i < units.Count ; i++)
        {
            _playerUnit.Add(units[i].transform);
        }
    }
	
	IEnumerator MonsterFSMCoroutine()
    {
        while(_stat._state == CEnemyStat.STATE.CREATE)
        {
            yield return null;
        }

        while(_stat._state != CEnemyStat.STATE.DEATH)
        {
            if (_playerUnit.Count == 0)
            {
                _anim.PlayAnimation(CEnemyStat.STATE.IDLE);
                yield break;
            }
                int targetNum = 0;
                float dist = 100.0f;

                for (int i = 0; i < _playerUnit.Count; i++)
                {
                    float targetDist = Vector3.Distance(_playerUnit[i].position, transform.position);
                    if (targetDist < dist)
                    {
                        dist = targetDist;
                        targetNum = i;
                        Target = _playerUnit[targetNum].gameObject;
                    }
                }

                //공격범위 안에 있을때
                if (dist <= _attackDist)
                {
                    _nav.Stop();

                    Vector3 dir = _playerUnit[targetNum].position - transform.position;

                    Quaternion rot = Quaternion.LookRotation(dir.normalized);

                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.2f);

                    _anim.PlayAnimation(CEnemyStat.STATE.ATTACK);
                }

                //공격범위 밖일때 
                else if (_attackDist < dist)
                {
                    _nav.SetDestination(_playerUnit[targetNum].position);
                    _nav.Resume();

                    _anim.PlayAnimation(CEnemyStat.STATE.MOVE);
                }
            

            yield return null;
        }

        //HP가 0이하면 플레이어유닛들의 적리스트에서 이 오브젝트를 삭제함
        for(int i = 0 ; i < _playerUnit.Count ; i++)
        {
            _nav.Stop();
            _playerUnit[i].SendMessage("DeleteEnemyList", gameObject);
        }
    }

    void DeletePlayerList(GameObject unit)
    {
        _playerUnit.Remove(unit.transform);
    }
}
