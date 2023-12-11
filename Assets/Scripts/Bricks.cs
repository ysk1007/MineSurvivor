using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Cinemachine.DocumentationSortingAttribute;

public class Bricks : MonoBehaviour
{
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void MakeDot (Vector3 Pos)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(Pos);
        if (tilemap.GetTile(cellPosition) == null)
            return;

        tilemap.SetTile(cellPosition, null);
        GameObject stone = GameManager.instance.pool.Get(6, false);
        int itemNum = ran();
        if (itemNum != 0)
        {
            GameObject dropItem = GameManager.instance.pool.Get(itemNum, false);
            dropItem.transform.position = cellPosition;
        }
        stone.transform.position = cellPosition;
        GameManager.instance.UpdateMesh = true;
    }

    int ran()
    {
        // Ȯ�� ���
        float randomValue = Random.value;

        // 75%�� Ȯ���� �ƹ��͵� �������� ����
        if (randomValue < 0.75f)
        {
            return 0;
        }
        // 20%�� Ȯ���� ö ���
        else if (randomValue < 0.95f)
        {
            return 7;
        }
        // 10%�� Ȯ���� �� ���
        else if (randomValue < 0.99f)
        {
            return 8;
        }
        // 5%�� Ȯ���� ���̾Ƹ�� ���
        else
        {
            return 9;
        }
    }
}
