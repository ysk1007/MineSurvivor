using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    public float timer;
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Start()
    {
        Init();
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Attack();
                }
                break;
        }  
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 1f;
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    void Batch()
    {
        Transform slash = GameManager.instance.pool.Get(prefabId,false).transform;
        slash.parent = transform;
        slash.GetComponent<Slash>().Init(damage, -1); // -1은 무한 관통 (근접)
    }

    void Attack()
    {
        if (!player.scanner.nearestTarget)
            return;
        Debug.Log("공격");
        Transform slash = GameManager.instance.pool.Get(prefabId,true).transform;
        slash.position = transform.position;
    }
}
