using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BrickSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tiles; // Ÿ�� �ȷ�Ʈ�� ����� Ÿ�ϵ�

    public int spawnRadius = 5; // Ÿ���� ��ġ�� �ݰ�
    public int maxBrickCount = 10; // �ִ� Ÿ�� ����

    // 2���� �迭
    private int[,] Brick = {
        {0, 0, 0, 0, 0 },
        {0, 1, 1, 1, 0 },
        {0, 1, 1, 1, 0 },
        {0, 1, 1, 1, 0 },
        {0, 0, 0, 0, 0 }
    };

    private int[,] Brick2 = {
        {1, 1, 1,},
        {1, 1, 1,},
        {1, 1, 1,}
    };

    void Start()
    {
        SpawnTiles();
    }

    void ranDrawTiles()
    {
        // 0 �Ǵ� -1 �� �ϳ��� �������� ����
        int plusX = (UnityEngine.Random.Range(0, 2) == 0) ? 5 : -10;
        // -10���� 10 �� �ϳ��� �������� ����
        int plusY = UnityEngine.Random.Range(-10, 11);
        Debug.Log("plusX: " + plusX);
        Debug.Log("plusY: " + plusY);

        Vector2Int vc = new Vector2Int(Mathf.RoundToInt(GameManager.instance.player.transform.position.x), Mathf.RoundToInt(GameManager.instance.player.transform.position.y));
        for (int i = 0; i < Brick.GetLength(0); i++)
        {
            for (int j = 0; j < Brick.GetLength(1); j++)
            {
                Vector3Int cellPosition = new Vector3Int(j+ vc.x+ plusX, -i+ vc.y+ plusY, 0); // Unity���� y�� ���� ����̹Ƿ� ��ȣ�� �ݴ�� ��

                // �� ����� ���� ���� Ÿ���� �׸�
                if (Brick[i, j] == 0)
                {
                    continue;
                }
                else if (Brick[i, j] == 1)
                {
                    tilemap.SetTile(cellPosition, tiles);
                }
            }
        }

        Invoke("ranDrawTiles", 3f);
    }

    void DrawTiles()
    {
        Vector2Int gridCenter = new Vector2Int(Mathf.RoundToInt(GameManager.instance.player.transform.position.x), Mathf.RoundToInt(GameManager.instance.player.transform.position.y));
        Vector3Int tilemapCenter = tilemap.WorldToCell(new Vector3(gridCenter.x, gridCenter.y, 0));

        for (int i = 0; i < Brick.GetLength(0); i++)
        {
            for (int j = 0; j < Brick.GetLength(1); j++)
            {
                // �� ����� ���� ���� Ÿ���� �׸�
                if (Brick[i, j] == 1)
                {
                    Vector3Int cellPosition = new Vector3Int(tilemapCenter.x + j, tilemapCenter.y - i, tilemapCenter.z);
                    // �̹� Ÿ���� �ִ��� Ȯ��
                    if (tilemap.GetTile(cellPosition) == null)
                    {
                        tilemap.SetTile(cellPosition, tiles);
                    }
                }
            }
        }
    }

    void SpawnTiles()
    {
        Vector3Int tilemapCenter = tilemap.WorldToCell(transform.position);

        for (int index = 0; index < maxBrickCount; index++)
        {
            Vector3Int randomOffset = new Vector3Int(UnityEngine.Random.Range(-spawnRadius, spawnRadius + 1), UnityEngine.Random.Range(-spawnRadius, spawnRadius + 1), 0);
            Vector3Int cellPosition = tilemapCenter + randomOffset;

            // �̹� Ÿ���� �ִ��� Ȯ��
            if (tilemap.GetTile(cellPosition) == null)
            {
                for (int i = 0; i < Brick2.GetLength(0); i++)
                {
                    for (int j = 0; j < Brick2.GetLength(1); j++)
                    {
                        Vector3Int pos = new Vector3Int(j+ cellPosition.x, -i+ cellPosition.y, 0); // Unity���� y�� ���� ����̹Ƿ� ��ȣ�� �ݴ�� ��

                        // �� ����� ���� ���� Ÿ���� �׸�
                        if (Brick2[i, j] == 0)
                        {
                            continue;
                        }
                        else if (Brick2[i, j] == 1)
                        {
                            tilemap.SetTile(pos, tiles);
                        }
                    }
                }
            }
        }
    }
}
