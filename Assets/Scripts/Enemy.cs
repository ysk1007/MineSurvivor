using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int monsterType;
    public float speed;
    public float currentHp;
    public float maxHp;
    public Rigidbody2D target;
    public float Dir;
    bool isLive;
    bool Targeting;

    Rigidbody2D rigid;
    public static Transform transform;
    Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isLive)
            return;
        if (Targeting)
        {
            float animspeed = 0;
            switch (monsterType)
            {
                case 0:
                    animspeed = 0.5f;
                    break;
                case 1:
                    animspeed = 0.3f;
                    break;
                case 2:
                    animspeed = 0.2f;
                    break;
            }
            anim.SetFloat("RunState", animspeed);
        }
        else
        {
            anim.SetFloat("RunState", 0f);
        }

        Vector2 dirVec = target.position - rigid.position; //Ÿ���� ���ϴ� ����
        Dir = dirVec.x;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //Ÿ���� ���� ������

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (Dir < 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (Dir > 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }

    void OnEnable() //��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ��
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        Targeting = true;
        currentHp = maxHp;
    }

    public void Init(SpawnData data)
    {
        monsterType = data.monsterType;
        speed = data.speed;
        maxHp = data.hp;
        currentHp = data.hp;
    }

    void OnTriggerEnter2D(Collider2D collision) //�ǰ� ����
    {
        if (!collision.CompareTag("Slash"))
            return;

        currentHp -= collision.GetComponent<Slash>().damage;

        if (currentHp > 0) //�������
        {

        }
        else //����
        {
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
