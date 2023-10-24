using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public SpawnData[] spawnData;

    public int level;
    float timer;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(level, false);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        enemy.GetComponentInChildren<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable] //Á÷·ÄÈ­
public class SpawnData
{
    public float spawnTime;
    public int monsterType;
    public int hp;
    public float speed;
}
