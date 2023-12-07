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

    private List<int[,]> BrickPattern = new List<int[,]> { };

    // 2차원 배열
    private int[,] Brick0 = {
        {1, 0, 0,},
        {1, 0, 0,},
        {1, 1, 1,}
    };

    private int[,] Brick1 = {
        {0, 1, 0,},
        {0, 1, 0,},
        {0, 1, 0,}
    };

    private int[,] Brick2 = {
        {1, 1, 1,},
        {0, 0, 1,},
        {0, 0, 1,}
    };

    private int[,] Brick3 = {
        {0, 1, 0,},
        {1, 1, 1,},
        {0, 1, 0,}
    };

    private int[,] Brick4 = {
        {0, 0, 0,},
        {1, 1, 1,},
        {0, 0, 0,}
    };

    private int[,] Brick5 = {
        {1, 1, 0,},
        {1, 1, 0,},
        {0, 0, 0,}
    };

    private void Awake()
    {
        BrickPattern.Add(Brick0);
        BrickPattern.Add(Brick1);
        BrickPattern.Add(Brick2);
        BrickPattern.Add(Brick3);
        BrickPattern.Add(Brick4);
        BrickPattern.Add(Brick5);
    }

    void Start()
    {
        SpawnTiles();
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
                int pattern = UnityEngine.Random.Range(0, 6);
                for (int i = 0; i < BrickPattern[pattern].GetLength(0); i++)
                {
                    for (int j = 0; j < BrickPattern[pattern].GetLength(1); j++)
                    {
                        Vector3Int pos = new Vector3Int(j+ cellPosition.x, -i+ cellPosition.y, 0); // Unity에서 y는 위가 양수이므로 부호를 반대로 함

                        // 각 요소의 값에 따라 타일을 그림
                        if (BrickPattern[pattern][i, j] == 0)
                        {
                            continue;
                        }
                        else if (BrickPattern[pattern][i, j] == 1)
                        {
                            tilemap.SetTile(pos, tiles);
                        }
                    }
                }
            }
        }
    }
}
