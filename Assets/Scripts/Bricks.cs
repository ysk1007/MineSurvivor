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
        // 확률 계산
        float randomValue = Random.value;

        // 75%의 확률로 아무것도 생성하지 않음
        if (randomValue < 0.75f)
        {
            return 0;
        }
        // 20%의 확률로 철 드랍
        else if (randomValue < 0.95f)
        {
            return 7;
        }
        // 10%의 확률로 금 드랍
        else if (randomValue < 0.99f)
        {
            return 8;
        }
        // 5%의 확률로 다이아몬드 드랍
        else
        {
            return 9;
        }
    }
}
