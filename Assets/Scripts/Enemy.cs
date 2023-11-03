using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
    Collider2D coll;
    public static Transform transform;
    SortingGroup sort;
    Animator anim;
    WaitForFixedUpdate wait;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        sort = GetComponent<SortingGroup>();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetBool("Hit"))
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
        if (!GameManager.instance.isLive)
            return;

        if (Dir < 0)
        {
            this.gameObject.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (Dir > 0)
        {
            this.gameObject.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }

    void OnEnable() //��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ��
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        sort.sortingOrder = 5;
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
        if ((!collision.CompareTag("Slash") && !collision.CompareTag("Expolsion")) || !isLive)
            return;

        switch (collision.tag)
        {
            case "Slash":
                Debug.Log("Į ����");
                currentHp -= collision.GetComponent<Slash>().damage;
                StartCoroutine(KnockBack(collision.GetComponent<Slash>().dir));
                break;
            case "Expolsion":
                Debug.Log("��ź ����");
                currentHp -= collision.GetComponent<Expolsion>().damage;
                break;
            default: 
                currentHp -= 0; 
                break;
        }

        if (currentHp > 0) //�������
        {
            anim.SetFloat("RunState", 0.7f);
            anim.SetBool("Hit", true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else //����
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            sort.sortingOrder = 4;
            anim.SetTrigger("Die");
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack(Vector3 dir)
    {
        yield return wait; // ���� �ϳ��� ���� ������ ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dir * 1.5f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Hit", false);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
