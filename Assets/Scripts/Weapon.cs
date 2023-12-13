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
    public float DamagePer = 1; //������ ����
    public int count;
    public float speed;
    public float speedPer = 1; //�ӵ� ����
    public int AttackCount = 0;

    public float timer;
    public Player player;

    public Transform center; // �� �߽� ��ġ
    public float radius = 1.0f; // �� ������

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

        //Ư�� �Լ� ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
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

        //Ư�� �Լ� ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver); 
    }

    void Batch()
    {
        Transform slash = GameManager.instance.pool.Get(prefabId,false).transform;
        slash.parent = transform;
        slash.GetComponent<Slash>().Init(damage, -1, Vector3.zero); // -1�� ���� ���� (����)
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

        // ��ü�� ��ġ
        Vector2 objectPosition = player.scanner.TargetPos;

        // ���� �߽� ��ġ
        Vector2 circleCenter = center.position;

        // �� �߽ɿ��� ��ü������ ����
        Vector2 toObject = objectPosition - circleCenter;

        // �ش� ���͸� ���� ��������ŭ ����ȭ�Ͽ� ���� ����� ���� ����Ͽ� �̵�
        Vector2 closestPointOnCircle = circleCenter + toObject.normalized * radius;

        Transform slash = GameManager.instance.pool.Get(prefabId, true, closestPointOnCircle).transform;
        // ��ü�� ���� ����� ������ �̵� closestPointOnCircle

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
        // ��ü�� ���� ����� ������ �̵�
        WindSlash.position = pos;
        WindSlash.transform.parent = GameManager.instance.pool.transform;
        WindSlash.GetComponent<WindSlash>().Init(damage, count, rotatedDir);
        WindSlash.rotation = Quaternion.FromToRotation(Vector3.up, rotatedDir);
        Debug.Log("�ٶ� ������");
    }

    void DynamiteAttack()
    {
        if (!player.scanner.FirstTarget)
            return;

        // ��ü�� ��ġ
        Vector2 objectPosition = player.scanner.TargetPos;

        // ���� �߽� ��ġ
        Vector2 circleCenter = center.position;

        // �� �߽ɿ��� ��ü������ ����
        Vector2 toObject = objectPosition - circleCenter;

        player.Attack();

        Transform Dynamite = GameManager.instance.pool.Get(prefabId, true).transform;
        // ��ü�� ���� ����� ������ �̵�
        Dynamite.position = transform.position;
        Dynamite.transform.parent = GameManager.instance.pool.transform;
        Dynamite.GetComponent<Dynamite>().Init(damage, count, toObject.normalized);
        Dynamite.rotation = Quaternion.FromToRotation(Vector3.up, toObject.normalized);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
