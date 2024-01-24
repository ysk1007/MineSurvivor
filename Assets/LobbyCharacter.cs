using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyCharacter : MonoBehaviour
{
    public float moveSpeed = 3f; // �̵� �ӵ�
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
        // ���� ���۽� ó������ �����̵��� ����
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
        // ������ �������� �̵�
        Angle = Random.Range(0f, 360f);
        Dir = new Vector2(Mathf.Cos(Angle * Mathf.Deg2Rad), Mathf.Sin(Angle * Mathf.Deg2Rad));
        GetComponent<Rigidbody2D>().velocity = Dir * moveSpeed;
        // ���� �ð� �Ŀ� ����
        Invoke("StopMoving", Random.Range(2f, 4f));
    }

    private void StopMoving()
    {
        moving = false;
        anim.SetFloat("RunState", 0f);
        // �������� ���߰� ���� �ð� �Ŀ� �ٽ� �����̱�
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Invoke("MoveRandomDirection", Random.Range(3f, 4f));
    }
}
