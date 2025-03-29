using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BrickSpawner : MonoBehaviour
{
    public Tilemap tilemap; // 타일이 그려질 맵
    public TileBase tiles;  // 타일 팔레트에 사용할 타일들

    public int spawnRadius = 5;    // 타일을 배치할 반경
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
        // 타일 맵의 중앙 위치를 계산 (월드 좌표를 셀 좌표로 변환)
        Vector3Int tilemapCenter = tilemap.WorldToCell(transform.position);

        // 최대 블록 수만큼 반복
        for (int index = 0; index < maxBrickCount; index++)
        {
            // 스폰 반경 내에서 랜덤 오프셋을 생성
            Vector3Int randomOffset = new Vector3Int(UnityEngine.Random.Range(-spawnRadius, spawnRadius + 1), 
                UnityEngine.Random.Range(-spawnRadius, spawnRadius + 1), 0);

            // 중앙 위치에서 랜덤 오프셋을 더해 새로운 셀 위치 계산
            Vector3Int cellPosition = tilemapCenter + randomOffset;

            // 이미 타일이 있는지 확인
            if (tilemap.GetTile(cellPosition) == null)
            {
                // 5가지 블럭 패턴중 하나 선택 패턴의 2D 배열을 순회하며 타일 생성
                int pattern = UnityEngine.Random.Range(0, 6);   // 행 탐색
                for (int i = 0; i < BrickPattern[pattern].GetLength(0); i++)    // 열 탐색
                {
                    for (int j = 0; j < BrickPattern[pattern].GetLength(1); j++)
                    {
                        // Unity에서 y는 위가 양수이므로 부호를 반대로 함
                        Vector3Int pos = new Vector3Int(j+ cellPosition.x, -i+ cellPosition.y, 0);

                        // 각 요소의 값에 따라 타일을 그림

                        if (BrickPattern[pattern][i, j] == 0)
                        {
                            continue;
                        }
                        else if (BrickPattern[pattern][i, j] == 1)
                        {
                            // 타일 그리기
                            tilemap.SetTile(pos, tiles);
                        }
                    }
                }
            }
        }

        // NavMesh를 갱신
        GameManager.instance.Bake();
    }
}
