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

    public Transform center; // 원 중심 위치
    public float radius = 3.0f; // 원 반지름

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
                    SlashAttack();
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
        slash.GetComponent<Slash>().Init(damage, -1, Vector3.zero); // -1은 무한 관통 (근접)
    }

    void SlashAttack()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized; //방향은 유지 크기는 1로 고정

        player.Attack();
        //this.gameObject.transform.position = player.scanner.nearestTarget.position;
        //this.gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir); //방향으로 돌림 (축,방향)
        //slash.rotation = Quaternion.FromToRotation(Vector3.up, dir); //방향으로 돌림 (축,방향)
        // 물체의 위치
        Vector2 objectPosition = player.scanner.nearestTarget.position;

        // 원의 중심 위치
        Vector2 circleCenter = center.position;

        // 원 중심에서 물체까지의 벡터
        Vector2 toObject = objectPosition - circleCenter;

        // 해당 벡터를 원의 반지름만큼 정규화하여 가장 가까운 점을 계산하여 이동
        Vector2 closestPointOnCircle = circleCenter + toObject.normalized * radius;

        // 물체를 가장 가까운 점으로 이동
        this.gameObject.transform.position = closestPointOnCircle;

        // 물체를 가장 가까운 점 방향으로 회전시킴
        Vector2 lookDirection = closestPointOnCircle - (Vector2)this.gameObject.transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        //this.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        Transform slash = GameManager.instance.pool.Get(prefabId, true).transform;
        slash.GetComponent<Slash>().Init(damage, count, dir);
        slash.rotation = Quaternion.FromToRotation(Vector3.up, toObject.normalized);
    }
}
