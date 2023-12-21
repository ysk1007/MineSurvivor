using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public SpawnData[] SR_Enemy; //근거리 몬스터
    public SpawnData[] LR_Enemy; //원거리 몬스터
    public float levelTime;

    public int level;
    float SR_timer;
    float LR_timer;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / SR_Enemy.Length;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        SR_timer += Time.deltaTime;
        LR_timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), SR_Enemy.Length - 1);

        if (SR_timer > SR_Enemy[level].spawnTime)
        {
            SR_timer = 0f;
            Spawn(0);
        }

        if (LR_timer > LR_Enemy[level].spawnTime)
        {
            LR_timer = 0f;
            Spawn(1);
        }
    }

    void Spawn(int type)
    {
        GameObject enemy = GameManager.instance.pool.Get(level, false);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        switch (type)
        {
            case 0:
                enemy.GetComponentInChildren<Enemy>().Init(SR_Enemy[level]);
                break;
            case 1:
                enemy.GetComponentInChildren<Enemy>().Init(LR_Enemy[level]);
                break;
            default:
                break;
        }
    }

    private void Event()
    {
        
    }
}

[System.Serializable] //직렬화
public class SpawnData
{
    public float spawnTime;
    public int monsterType;
    public int hp;
    public float speed;
}
