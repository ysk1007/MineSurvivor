using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Enemy : MonoBehaviour
{
    public int monsterType;
    public float speed;
    public float currentHp;
    public float maxHp;
    public Rigidbody2D target;
    public float Dir;
    public Navigation navigation;
    public NavMeshAgent agent;
    bool isLive;
    bool Targeting;
    Vector2 curDir;
    Vector3 vc;

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
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
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

        Vector2 dirVec = target.position - rigid.position; //타겟을 향하는 방향
        Dir = dirVec.x;
        curDir = dirVec.normalized;

        agent.SetDestination(target.transform.position);
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

    void OnEnable() //스크립트가 활성화 될 때 호출
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        agent.enabled = true;
        sort.sortingOrder = 5;
        Targeting = true;
        currentHp = maxHp;
    }

    public void Init(SpawnData data)
    {
        monsterType = data.monsterType;
        speed = data.speed;
        agent.speed = data.speed;
        maxHp = data.hp;
        currentHp = data.hp;
    }

    void OnTriggerEnter2D(Collider2D collision) //피격 감지
    {
        if ((!collision.CompareTag("Slash") && !collision.CompareTag("Expolsion") && !collision.CompareTag("WindSlash")) || !isLive)
            return;
        Vector3 vc = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, -1);
        float Damage;
        GameObject DamageText;
        switch (collision.tag)
        {
            case "Slash":
                Damage = collision.GetComponent<Slash>().damage;
                currentHp -= Damage;
                DamageText = GameManager.instance.pool.Get(10, false);
                DamageText.transform.position = vc;
                DamageText.GetComponent<DamageText>().value(Damage);
                //StartCoroutine(KnockBack());
                break;
            case "WindSlash":
                Damage = collision.GetComponent<WindSlash>().damage;
                currentHp -= Damage;
                DamageText = GameManager.instance.pool.Get(10, false);
                DamageText.transform.position = vc;
                DamageText.GetComponent<DamageText>().value(Damage);
                break;
            case "Expolsion":
                Damage = collision.GetComponent<Expolsion>().damage;
                currentHp -= Damage;
                DamageText = GameManager.instance.pool.Get(10, false);
                DamageText.transform.position = vc;
                DamageText.GetComponent<DamageText>().value(Damage);
                //StartCoroutine(KnockBack());
                break;
            default: 
                currentHp -= 0; 
                break;
        }

        if (currentHp > 0) //살아있음
        {
            anim.SetFloat("RunState", 0.7f);
            //anim.SetBool("Hit", true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else //죽음
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            agent.enabled = false;
            sort.sortingOrder = 4;
            anim.SetTrigger("Die");
            GameManager.instance.kill++;
            //GameManager.instance.GetExp();
            GameObject ExpGem = GameManager.instance.pool.Get(12, false);
            ExpGem.transform.position = this.gameObject.transform.position;
            if (GameManager.instance.player.id == 0)
            {
                GameManager.instance.player.Berserker();
            }
            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        agent.SetDestination(transform.position);
        Vector3 playerPos = GameManager.instance.player.transform.position;
        rigid.AddForce(-curDir * 1f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Hit", false);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
