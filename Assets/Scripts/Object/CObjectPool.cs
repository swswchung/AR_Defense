using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CObjectPool : MonoBehaviour {

    public static CObjectPool current;           //오브젝트 풀 static 참조 변수
    public GameObject[] prefabs;                 //오브젝트 풀을 사용할 프리팹 배열
    public List<GameObject>[] pooledObjects;    //오브젝트 풀 리스트
    public int[] amountToBuffer;                //인덱스와 일치하는 프리팹의 오브젝트 풀 크기 배열
    public int defaultBufferAmount = 10;        //기본 오브젝트 풀 크기
    public bool canGrow = true;                 //풀 크기 확장 여부

    GameObject containerObject;                 //컨테이너 오브젝트

    void Awake()
    {

        if (current == null)
        {
            current = this;     //오브젝트 풀 객체의 static 참조를 함
        }
        else
        {
            Destroy(gameObject);    //재생성 시 이전 오브젝트를 파괴함.
        }

        //빈 오브젝트 형태로(new GameObject) 오브젝트 풀을 생성함.
        containerObject = new GameObject("ObjectPool");

        //오브젝트 풀 리스트 배열을 생성함(프리팹 배열 크기와 일치)
        pooledObjects = new List<GameObject>[prefabs.Length];

        int index = 0;
        //오브젝트 풀 리스트 배열에 리스트를 생성함.
        foreach (GameObject objectPrefab in prefabs)
        {
            pooledObjects[index] = new List<GameObject>();

            //버퍼 크기를 로드함.
            int bufferAmount;

            //인덱스가 오브젝트 버퍼 크기 배열의 길이보다 작다면
            if (index < amountToBuffer.Length)
            {
                bufferAmount = amountToBuffer[index];
            }
            else
            {
                bufferAmount = defaultBufferAmount; //기본 버퍼 크기를 설정함.
            }

            //버퍼에 오브젝트를 생성함.
            for (int i = 0; i < bufferAmount; i++)
            {
                //오브젝트 생성.
                GameObject obj = (GameObject)Instantiate(objectPrefab);

                //생성한 오브젝트의 이름을 프리팹으로 설정함.
                obj.name = objectPrefab.name;

                //오브젝트 풀에 생성한 오브젝트를 넣음.
                PoolObject(obj);
            }
            index++;
        }
    }

    public GameObject GetObject(GameObject objectType)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            //프리팹 로드
            GameObject prefab = prefabs[i];

            //프리팹 이름과 생성하려는 오브젝트의 이름이 같고,
            if (prefab.name == objectType.name)
            {
                //프리팹 배열의 인덱스와 일치하는 오브젝트 풀 리스트 갯수가 0보다 크다면
                if (pooledObjects[i].Count > 0)
                {
                    //프리팹 배열의 인덱스와 일치하는 오브젝트 풀 리스트의 첫번째 오브젝트를 추출하고.
                    GameObject pooledObject = pooledObjects[i][0];

                    //추출한 오브젝트를 오브젝트 풀에서 제거.
                    pooledObjects[i].RemoveAt(0);

                    //오브젝트 풀 오브젝트의 자식여부를 해제함.
                    pooledObject.transform.parent = null;

                    //요청한 오브젝트를 리턴
                    return pooledObject;
                }

                //프리팹 배열의 인덱스와 일치하는 오브젝트 풀의 리스트 갯수가 0보다 작거나같고
                //오브젝트 풀 확장을 가능하도록 했다면.
                else if (canGrow)
                {
                    //새로 생성해서 돌려줌
                    GameObject obj = Instantiate(prefabs[i]) as GameObject;
                    obj.name = prefabs[i].name;
                    return obj;
                }
                //확장을 허용하지 않았다면 return null
                break;
            }
        }

        return null;
    }

    //생성한 오브젝트 또는 재사용할 게임 오브젝트르 오브젝트 풀에 추가함
    //다쓴 오브젝트를 이곳에 추가
    public void PoolObject(GameObject obj)
    {
        //설정된 프리팹 갯수만큼
        for (int i = 0; i < prefabs.Length; i++)
        {
            //프리팹 배열 요소의 이름이랑 생성할 오브젝트 이름이랑 같다면
            if (prefabs[i].name == obj.name)
            {
                //오브젝트를 비활성해 다음에 쓰기위해 저장
                obj.SetActive(false);
                //비활성한 오브젝트의 부모를 컨테이너 오브젝트의 자식으로 등록
                obj.transform.parent = containerObject.transform;
                //프리팹 배열 요소의 인덱스와 일치하는 오브젝트 풀에 오브젝트를 추가함.
                pooledObjects[i].Add(obj);
                return;
            }
        }
    }
}