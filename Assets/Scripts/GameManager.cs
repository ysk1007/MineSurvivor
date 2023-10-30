using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# 게임 컨트롤")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# 플레이어 정보")]
    public int curHp;
    public int maxHp = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# 게임 오브젝트")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;

        //임시
        uiLevelUp.Select(0);
    }

    void Update()
    {
        if (!isLive)
        {
            return;
        }

        //게임 시간
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length -1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Pause()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
