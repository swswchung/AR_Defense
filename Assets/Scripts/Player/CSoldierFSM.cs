using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSoldierFSM : MonoBehaviour {

    CSoldierStat _stat;
    CSoldierAnimation _anim;
    CSoldierShoot _shoot;

    public float _attackDist;   //사거리

    [SerializeField]
    List<Transform> _enemy; //공격할 적(웨이브가 시작될떄마다 Manager오브젝트에서 List로 적의정보를 전달해주고 그중 거리가 제일 적은적의 정보를 가져옴)

    [SerializeField]
    int targetNum;  //공격할 적 번호

    void Awake()
    {
        _stat = GetComponent<CSoldierStat>();
        _anim = GetComponent<CSoldierAnimation>();
        _shoot = GetComponent<CSoldierShoot>();
    }

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("SoldierFSMCoroutine");
	}

    //시작할때마다 매니저에서 적들의 위치를 받아와서 저장함.
    public void UpdateEnemyObject(List<GameObject> units)
    {
        //리스트를 초기화 한 뒤 위치를 받아옴
        _enemy.Clear();

        for(int i = 0 ; i < units.Count ; i++)
        {
            _enemy.Add(units[i].transform);
        }
    }

    IEnumerator SoldierFSMCoroutine()
    {
        while(_stat._state != CSoldierStat.STATE.DEATH)
        {
            if(_enemy == null)
            {
                yield break;
            }

            float dist = 100f;

            for(int i = 0 ; i < _enemy.Count ; i++)
            {
                float targetDist = Vector3.Distance(_enemy[i].position, transform.position);
                if (targetDist < dist)
                {
                    dist = targetDist;
                    targetNum = i;
                }
            }

            //제일 가까운 적과 나의 거리를 계산함
            //float dist = Vector3.Distance(_enemy[targetNum].position, transform.position);

            //공격범위안에 적이 들어오면
            if(dist <= _attackDist)
            {
                //방향을 적에게 맞춰 공격
                Vector3 direction = _enemy[targetNum].position - transform.position;

                Quaternion rot = Quaternion.LookRotation(direction.normalized);

                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.2f);

                _anim.PlayAnimation(CSoldierStat.STATE.SHOOT);
            }
            else
            {
                _anim.PlayAnimation(CSoldierStat.STATE.IDLE);
            }

            yield return null;
        }

        for(int i = 0 ; i < _enemy.Count ; i++)
        {
            _enemy[i].SendMessage("DeletePlayerList",gameObject);
        }

    }

    void DeleteEnemyList(GameObject enemy)
    {
        _enemy.Remove(enemy.transform);
    }
}