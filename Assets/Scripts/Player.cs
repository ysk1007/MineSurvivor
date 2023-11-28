using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("")]
    public Vector2 inputVec;
    public float speed;
    public GameObject SlashAttack;
    public Transform AttackRange;
    public Scanner scanner;
    public int id;
    public float SkillTimer;
    public bool isSkill;
    public GameObject efc;
    public float[] SkillDuration;
    Rigidbody2D rigid;
    Transform transform;
    Animator anim;

    void Awake()
    {
        /*int id = GameManager.instance.playerID;
        rigid = GetComponentsInChildren<Rigidbody2D>()[id];
        transform = GetComponentsInChildren<Transform>()[id];
        anim = GetComponentsInChildren<Animator>()[id];
        scanner = GetComponentsInChildren<Scanner>()[id];*/
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        id = GameManager.instance.playerID;
    }
    
    
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void Update()
    {
        if (!GameManager.instance.isLive)  
            return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        if (isSkill)
            return;

        switch (id) //ĳ���� �� ��ų
        {
            case 0: //���
                SkillTimer += Time.deltaTime;
                if (SkillTimer > 5)
                {
                    SkillTimer = 0f;
                    isSkill = true;
                    efc.SetActive(true);
                    StartCoroutine(Skill(id));
                }
                break;
            case 1: //���̳ʸ���Ʈ
                SkillTimer += Time.deltaTime;

                if (SkillTimer > 5)
                {
                    SkillTimer = 0f;
                    isSkill = true;
                    //Skill(id);
                }
                break;
        }
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (inputVec.x != 0 || inputVec.y != 0)
        {
            anim.SetFloat("RunState",0.35f);
        }
        else
            anim.SetFloat("RunState",0f);

        if(inputVec.x < 0 )
        {
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
        else if(inputVec.x > 0)
        {
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        }

        if(Input.GetKeyUp(KeyCode.Z))
        {
            anim.SetTrigger("Attack");
        }

    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Enemy") || !GameManager.instance.isLive)
            return;

        GameManager.instance.curHp -= Time.deltaTime * 10;

        if (GameManager.instance.curHp < 0)
        {
            for (int i = 3; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.SetTrigger("Die");
            GameManager.instance.GameOver();
        }
    }

    IEnumerator Skill(int id)
    {
        Debug.Log("��ų����");
        Weapon weapon = GameManager.instance.weapon;
        float timeElapsed = 0f;

        switch (id)
        {
            case 0:
                weapon.DamagePer = 1.5f;
                weapon.speedPer = 0.8f;
                while (timeElapsed < SkillDuration[id])
                {
                    // Mathf.Lerp�� ����Ͽ� 1���� 0���� �ڿ������� �����ϴ� ���� ���
                    float smoothDecreaseValue = Mathf.Lerp(1f, 0f, timeElapsed / SkillDuration[id]);

                    // �ð� ������Ʈ
                    timeElapsed += Time.deltaTime;

                    yield return null;
                }
                weapon.DamagePer = 1f;
                weapon.speedPer = 1f;
                efc.SetActive(false);
                break;
        }
        isSkill = false;
    }
}
