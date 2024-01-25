using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyCharacter : MonoBehaviour
{
    public float moveSpeed = 3f; // 이동 속도
    Vector2 Dir;
    float Angle;
    Animator anim;
    bool moving;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    private void Start()
    {
        // 게임 시작시 처음으로 움직이도록 설정
        MoveRandomDirection();
    }

    private void Update()
    {
        if (!moving)
            return;
        if (Dir.x < 0)
        {
            transform.localScale = new Vector3(1.3f, transform.localScale.y, transform.localScale.z);
        }
        else if (Dir.x > 0)
        {
            transform.localScale = new Vector3(-1.3f, transform.localScale.y, transform.localScale.z);
        }
    }

    private void MoveRandomDirection()
    {
        moving = true;
        anim.SetFloat("RunState", 0.2f);
        // 랜덤한 방향으로 이동
        Angle = Random.Range(0f, 360f);
        Dir = new Vector2(Mathf.Cos(Angle * Mathf.Deg2Rad), Mathf.Sin(Angle * Mathf.Deg2Rad));
        GetComponent<Rigidbody2D>().velocity = Dir * moveSpeed;
        // 일정 시간 후에 쉬기
        Invoke("StopMoving", Random.Range(2f, 4f));
    }

    private void StopMoving()
    {
        moving = false;
        anim.SetFloat("RunState", 0f);
        // 움직임을 멈추고 일정 시간 후에 다시 움직이기
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Invoke("MoveRandomDirection", Random.Range(3f, 4f));
    }
}
