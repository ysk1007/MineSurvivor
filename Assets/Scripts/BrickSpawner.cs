using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BrickSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tiles; // 타일 팔레트에 사용할 타일들

    public int spawnRadius = 5; // 타일을 배치할 반경
    public int maxBrickCount = 10; // 최대 타일 개수

    // 2차원 배열
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
        // 0 또는 -1 중 하나를 랜덤으로 선택
        int plusX = (UnityEngine.Random.Range(0, 2) == 0) ? 5 : -10;
        // -10에서 10 중 하나를 랜덤으로 선택
        int plusY = UnityEngine.Random.Range(-10, 11);
        Debug.Log("plusX: " + plusX);
        Debug.Log("plusY: " + plusY);

        Vector2Int vc = new Vector2Int(Mathf.RoundToInt(GameManager.instance.player.transform.position.x), Mathf.RoundToInt(GameManager.instance.player.transform.position.y));
        for (int i = 0; i < Brick.GetLength(0); i++)
        {
            for (int j = 0; j < Brick.GetLength(1); j++)
            {
                Vector3Int cellPosition = new Vector3Int(j+ vc.x+ plusX, -i+ vc.y+ plusY, 0); // Unity에서 y는 위가 양수이므로 부호를 반대로 함

                // 각 요소의 값에 따라 타일을 그림
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
                // 각 요소의 값에 따라 타일을 그림
                if (Brick[i, j] == 1)
                {
                    Vector3Int cellPosition = new Vector3Int(tilemapCenter.x + j, tilemapCenter.y - i, tilemapCenter.z);
                    // 이미 타일이 있는지 확인
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

            // 이미 타일이 있는지 확인
            if (tilemap.GetTile(cellPosition) == null)
            {
                for (int i = 0; i < Brick2.GetLength(0); i++)
                {
                    for (int j = 0; j < Brick2.GetLength(1); j++)
                    {
                        Vector3Int pos = new Vector3Int(j+ cellPosition.x, -i+ cellPosition.y, 0); // Unity에서 y는 위가 양수이므로 부호를 반대로 함

                        // 각 요소의 값에 따라 타일을 그림
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
