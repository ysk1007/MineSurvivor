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
    public float SkillDurTimer;
    public float[] SkillCoolTime;
    public bool isSkill;
    public GameObject efc;
    public float[] SkillDuration;
    public bool SkillMaster1, SkillMaster2, SkillMaster3;
    public int BSK_Level;
    public float[] BSK_Damage, BSK_Speed;
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

        switch (id) //캐릭터 별 스킬
        {
            case 0: //곡괭이
                SkillTimer += Time.deltaTime;
                if (SkillTimer > SkillCoolTime[id])
                {
                    isSkill = true;
                    efc.SetActive(true);
                    StartCoroutine(Skill(id));
                }
                break;
            case 1: //다이너마이트
                SkillTimer += Time.deltaTime;

                if (SkillTimer > SkillCoolTime[id])
                {
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
        Weapon weapon = GameManager.instance.weapon;
        SkillDurTimer = 0f;

        switch (id)
        {
            case 0:
                weapon.DamagePer = BSK_Damage[BSK_Level];
                weapon.speedPer = BSK_Speed[BSK_Level];
                while (SkillDurTimer < SkillDuration[id])
                {
                    // Mathf.Lerp를 사용하여 1에서 0으로 자연스럽게 감소하는 값을 계산
                    float smoothDecreaseValue = Mathf.Lerp(1f, 0f, SkillDurTimer / SkillDuration[id]);

                    // 시간 업데이트
                    SkillDurTimer += Time.deltaTime;

                    yield return null;
                }
                weapon.DamagePer = 1f;
                weapon.speedPer = 1f;
                efc.SetActive(false);
                break;
        }
        isSkill = false;
        SkillTimer = 0f;
        SkillDurTimer = 0f;
    }

    public void Berserker()
    {
        if (SkillMaster1)
        {
            SkillTimer += 1f;
        }
        if (SkillMaster2 && isSkill)
        {
            GameManager.instance.curHp += 1f;
            if (GameManager.instance.curHp > GameManager.instance.maxHp)
            {
                GameManager.instance.curHp = GameManager.instance.maxHp;
            }
        }
        if (SkillMaster3)
        {
            SkillDurTimer -= 0.2f;
        }
    }
}
