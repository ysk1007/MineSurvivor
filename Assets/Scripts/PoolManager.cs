using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("������ ����")]
    public GameObject[] prefabs;

    [Header("Ǯ ��� ����Ʈ")]
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

        // ��Ȱ��ȭ �� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ���ٸ�
        if (select == null)
        {
            Transform trans = transform;
            if (mine)
            {
                trans = player;
            }
            // �����ϰ� �Ҵ�
            select = Instantiate(prefabs[index], trans);
            pools[index].Add(select);
        }

        return select;
    }

    public GameObject Get(int index, bool mine, Vector3 pos)
    {
        GameObject select = null;

        // ��Ȱ��ȭ �� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.transform.position = pos;
                select.SetActive(true);
                break;
            }
        }

        // ���ٸ�
        if (select == null)
        {
            Transform trans = transform;
            if (mine)
            {
                trans = player;
            }
            // �����ϰ� �Ҵ�
            select = Instantiate(prefabs[index], trans);
            select.transform.position = pos;
            pools[index].Add(select);
        }

        return select;
    }
}
