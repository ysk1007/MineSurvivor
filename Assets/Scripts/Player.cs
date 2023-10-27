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

    Rigidbody2D rigid;
    Transform transform;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void Start()
    {
        
    }
    
    
    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void Update()
    {
        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void LateUpdate()
    {
        if(inputVec.x != 0 || inputVec.y != 0)
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
}
