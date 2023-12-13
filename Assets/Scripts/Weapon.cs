using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public float DamagePer = 1; //데미지 배율
    public int count;
    public float speed;
    public float speedPer = 1; //속도 배율
    public int AttackCount = 0;

    public float timer;
    public Player player;

    public Transform center; // 원 중심 위치
    public float radius = 1.0f; // 원 반지름

    void Awake()
    {
        player = GameManager.instance.player;
        center = player.AttackRange;
    }

/*    void Start()
    {
        Init();
    }*/

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        //특정 함수 호출을 모든 자식에게 방송하는 함수
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:
                timer += Time.deltaTime;

                if (timer > speed * speedPer)
                {
                    timer = 0f;
                    SlashAttack();
                }
                break;
            case 1:
                timer += Time.deltaTime;

                if (timer > speed * speedPer)
                {
                    timer = 0f;
                    DynamiteAttack();
                }
                break;
        }  
    }

    public void Init(ItemData data)
    {
        player = GameManager.instance.player;
        center = player.AttackRange.transform;
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        if (data.itemType == ItemData.ItemType.Pickax)
        {
            this.transform.localPosition = new Vector3(0.5f, 0, 0);
        }

        // Property Set
        id = data.itemId;
        damage = data.baseDamge * Character.Damage;
        count = data.baseCount + Character.Count;
        speed = data.baseCount;

        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 1f * Character.WeaponRate;
                break;
            case 1:
                speed = 1.5f * Character.WeaponRate;
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }

        //특정 함수 호출을 모든 자식에게 방송하는 함수
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver); 
    }

    void Batch()
    {
        Transform slash = GameManager.instance.pool.Get(prefabId,false).transform;
        slash.parent = transform;
        slash.GetComponent<Slash>().Init(damage, -1, Vector3.zero); // -1은 무한 관통 (근접)
    }

    void SlashAttack()
    {
        if (!player.scanner.FirstTarget)
            return;
        player.Attack();
    }

    public void SpawnSlash()
    {
        Vector2 dir = player.scanner.dir;

        // 물체의 위치
        Vector2 objectPosition = player.scanner.TargetPos;

        // 원의 중심 위치
        Vector2 circleCenter = center.position;

        // 원 중심에서 물체까지의 벡터
        Vector2 toObject = objectPosition - circleCenter;

        // 해당 벡터를 원의 반지름만큼 정규화하여 가장 가까운 점을 계산하여 이동
        Vector2 closestPointOnCircle = circleCenter + toObject.normalized * radius;

        Transform slash = GameManager.instance.pool.Get(prefabId, true, closestPointOnCircle).transform;
        // 물체를 가장 가까운 점으로 이동 closestPointOnCircle

        slash.transform.parent = this.transform;
        slash.GetComponent<Slash>().Init(damage * DamagePer, count, dir);
        slash.rotation = Quaternion.FromToRotation(Vector3.up, toObject.normalized);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Melee);
        AttackCount++;

        if (AttackCount == 3)
        {
            for (int i = -1; i <= 1; i++)
            {
                WindSlash(closestPointOnCircle, toObject, i * 15f);
            }
            AttackCount = 0;
        }

    }

    void WindSlash(Vector2 pos,Vector2 dir, float angleOffset)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, angleOffset);
        Vector2 rotatedDir = rotation * dir.normalized;

        Transform WindSlash = GameManager.instance.pool.Get(11, true).transform;
        // 물체를 가장 가까운 점으로 이동
        WindSlash.position = pos;
        WindSlash.transform.parent = GameManager.instance.pool.transform;
        WindSlash.GetComponent<WindSlash>().Init(damage, count, rotatedDir);
        WindSlash.rotation = Quaternion.FromToRotation(Vector3.up, rotatedDir);
        Debug.Log("바람 가르기");
    }

    void DynamiteAttack()
    {
        if (!player.scanner.FirstTarget)
            return;

        // 물체의 위치
        Vector2 objectPosition = player.scanner.TargetPos;

        // 원의 중심 위치
        Vector2 circleCenter = center.position;

        // 원 중심에서 물체까지의 벡터
        Vector2 toObject = objectPosition - circleCenter;

        player.Attack();

        Transform Dynamite = GameManager.instance.pool.Get(prefabId, true).transform;
        // 물체를 가장 가까운 점으로 이동
        Dynamite.position = transform.position;
        Dynamite.transform.parent = GameManager.instance.pool.transform;
        Dynamite.GetComponent<Dynamite>().Init(damage, count, toObject.normalized);
        Dynamite.rotation = Quaternion.FromToRotation(Vector3.up, toObject.normalized);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
