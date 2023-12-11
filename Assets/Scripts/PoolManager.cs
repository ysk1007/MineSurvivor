using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("프리팹 변수")]
    public GameObject[] prefabs;

    [Header("풀 담당 리스트")]
    List<GameObject>[] pools;

    public Transform player;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index, bool mine)
    {
        GameObject select = null;

        // 비활성화 된 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 없다면
        if (select == null)
        {
            Transform trans = transform;
            if (mine)
            {
                trans = player;
            }
            // 생성하고 할당
            select = Instantiate(prefabs[index], trans);
            pools[index].Add(select);
        }

        return select;
    }

    public GameObject Get(int index, bool mine, Vector3 pos)
    {
        GameObject select = null;

        // 비활성화 된 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.transform.position = pos;
                select.SetActive(true);
                break;
            }
        }

        // 없다면
        if (select == null)
        {
            Transform trans = transform;
            if (mine)
            {
                trans = player;
            }
            // 생성하고 할당
            select = Instantiate(prefabs[index], trans);
            select.transform.position = pos;
            pools[index].Add(select);
        }

        return select;
    }
}
