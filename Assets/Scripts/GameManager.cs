using JetBrains.Annotations;
using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# 게임 컨트롤")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public bool UpdateMesh = false;
    public NavMeshSurface surface;

    [Header("# 플레이어 정보")]
    public int playerID;
    public float curHp;
    public float maxHp = 100;
    public int level;
    public int kill;
    public int IronCount;
    public int GoldCount;
    public int DiamondCount;
    public float exp;
    public float GetXp = 0;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public Weapon weapon;
    public ItemData[] data;

    [Header("# 게임 오브젝트")]
    public Player player;
    public GameObject[] players;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public void GameStart(int id)
    {
        playerID = id;
        curHp = maxHp;

        for (int i = 0; i < players.Length; i++)
        {
            if (i == id)
            {
                players[i].gameObject.SetActive(true);
                players[i].gameObject.transform.SetParent(player.transform);
                players[i].gameObject.transform.SetAsFirstSibling();
                break;
            }
        }
        player.gameObject.SetActive(true);
        //uiLevelUp.Select(playerID % 2); //기본 무기 지급
        GameObject newWeapon = new GameObject();
        weapon = newWeapon.AddComponent<Weapon>();
        weapon.Init(data[id]);
        isLive = true;
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(1f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Pause();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(1f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Pause();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
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
            GameVictory();
        }
    }

    private void LateUpdate()
    {
        if (UpdateMesh)
        {
            Bake();
        }
    }

    public void Bake()
    {
        UpdateMesh = false;
        Debug.Log("Mesh 갱신");
        surface.UpdateNavMesh(surface.navMeshData);
    }


    public void GetExp()
    {
        if (!isLive)
            return;
        exp += 1 + (1 * GetXp);

        if (exp >= nextExp[Mathf.Min(level, nextExp.Length -1)])
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
