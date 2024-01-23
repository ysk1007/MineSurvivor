using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyCharacter : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float moveDuration;
    public float idleDuration;

    public float minMoveDuration = 1f; // �ּ� �̵� �ð�
    public float maxMoveDuration = 3f; // �ִ� �̵� �ð�
    public float minIdleDuration = 1f; // �ּ� ���� �ð�
    public float maxIdleDuration = 3f; // �ִ� ���� �ð�

    public float timer = 0f;
    private bool isMoving = true;
    private Vector2 moveDirection;
    private Vector2 nextMoveDirection; // ���� �̵� ����

    BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
        moveDirection = GetRandomDirection();
        SetRandomDurations();
        SetRandomNextMoveDirection();
    }

    void Update()
    {
        // �̵� �Ǵ� ���� ���¿� ���� �ൿ ����
        if (isMoving)
        {
            Move();
        }
        else
        {
            Idle();
        }

        // �浹 üũ
        CheckCollision();
    }

    void Move()
    {
        Debug.Log("������");
        // �̵�
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        // ���� �ð��� ������ ���� ���·� ��ȯ
        timer += Time.deltaTime;
        if (timer >= moveDuration)
        {
            timer = 0f;
            isMoving = false;

            // �浹 �� �ݴ� �������� �̵� �� �ٽ� �̵� ���·� ��ȯ
            StartCoroutine(MoveInOppositeDirection());
        }
    }

    IEnumerator MoveInOppositeDirection()
    {
        Vector2 originalMoveDirection = moveDirection;
        moveDirection = GetOppositeDirection(originalMoveDirection);

        // ��� �ݴ� �������� �̵�
        yield return new WaitForSeconds(0.5f);

        // �̵��� ���߰� ���� �̵� ���� ����
        isMoving = true;
        SetRandomDurations();
        SetRandomNextMoveDirection();
    }

    void Idle()
    {
        Debug.Log("����");
        // ���� �ð��� ������ ���� �ൿ�� �����ϰ� �̵� ���·� ��ȯ
        timer += Time.deltaTime;
        if (timer >= idleDuration)
        {
            timer = 0f;
            isMoving = true;
            SetRandomDurations();
            SetRandomNextMoveDirection();
        }
    }

    void SetRandomDurations()
    {
        moveDuration = Random.Range(minMoveDuration, maxMoveDuration);
        idleDuration = Random.Range(minIdleDuration, maxIdleDuration);
    }

    void SetRandomNextMoveDirection()
    {
        nextMoveDirection = GetRandomDirection();
    }

    void CheckCollision()
    {
        // BoxCollider2D�� ũ�⸦ ����Ͽ� �浹 üũ
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxCollider2D.size, 0f);

        foreach (Collider2D collider in colliders)
        {
            // Ư�� �ݶ��̴��� �浹 �� �̵��� ���߰� ���� �̵� ���� ����
            if (collider.CompareTag("Area"))
            {
                Debug.Log("�浹");
                isMoving = false;
                StartCoroutine(MoveInOppositeDirection());
                break;
            }
        }
    }

    Vector2 GetRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        return Quaternion.Euler(0, 0, angle) * Vector2.right;
    }

    Vector2 GetOppositeDirection(Vector2 direction)
    {
        return -direction;
    }
}
