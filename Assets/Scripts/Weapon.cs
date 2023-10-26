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

    public Transform center; // �� �߽� ��ġ
    public float radius = 3.0f; // �� ������

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
        slash.GetComponent<Slash>().Init(damage, -1, Vector3.zero); // -1�� ���� ���� (����)
    }

    void SlashAttack()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized; //������ ���� ũ��� 1�� ����

        player.Attack();
        //this.gameObject.transform.position = player.scanner.nearestTarget.position;
        //this.gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir); //�������� ���� (��,����)
        //slash.rotation = Quaternion.FromToRotation(Vector3.up, dir); //�������� ���� (��,����)
        // ��ü�� ��ġ
        Vector2 objectPosition = player.scanner.nearestTarget.position;

        // ���� �߽� ��ġ
        Vector2 circleCenter = center.position;

        // �� �߽ɿ��� ��ü������ ����
        Vector2 toObject = objectPosition - circleCenter;

        // �ش� ���͸� ���� ��������ŭ ����ȭ�Ͽ� ���� ����� ���� ����Ͽ� �̵�
        Vector2 closestPointOnCircle = circleCenter + toObject.normalized * radius;

        // ��ü�� ���� ����� ������ �̵�
        this.gameObject.transform.position = closestPointOnCircle;

        // ��ü�� ���� ����� �� �������� ȸ����Ŵ
        Vector2 lookDirection = closestPointOnCircle - (Vector2)this.gameObject.transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        //this.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        Transform slash = GameManager.instance.pool.Get(prefabId, true).transform;
        slash.GetComponent<Slash>().Init(damage, count, dir);
        slash.rotation = Quaternion.FromToRotation(Vector3.up, toObject.normalized);
    }
}
