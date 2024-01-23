using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyCharacter : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float moveDuration;
    public float idleDuration;

    public float minMoveDuration = 1f; // 최소 이동 시간
    public float maxMoveDuration = 3f; // 최대 이동 시간
    public float minIdleDuration = 1f; // 최소 멈춤 시간
    public float maxIdleDuration = 3f; // 최대 멈춤 시간

    public float timer = 0f;
    private bool isMoving = true;
    private Vector2 moveDirection;
    private Vector2 nextMoveDirection; // 다음 이동 방향

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
        // 이동 또는 멈춤 상태에 따라 행동 결정
        if (isMoving)
        {
            Move();
        }
        else
        {
            Idle();
        }

        // 충돌 체크
        CheckCollision();
    }

    void Move()
    {
        Debug.Log("움직임");
        // 이동
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        // 일정 시간이 지나면 멈춤 상태로 전환
        timer += Time.deltaTime;
        if (timer >= moveDuration)
        {
            timer = 0f;
            isMoving = false;

            // 충돌 시 반대 방향으로 이동 후 다시 이동 상태로 전환
            StartCoroutine(MoveInOppositeDirection());
        }
    }

    IEnumerator MoveInOppositeDirection()
    {
        Vector2 originalMoveDirection = moveDirection;
        moveDirection = GetOppositeDirection(originalMoveDirection);

        // 잠시 반대 방향으로 이동
        yield return new WaitForSeconds(0.5f);

        // 이동을 멈추고 다음 이동 방향 설정
        isMoving = true;
        SetRandomDurations();
        SetRandomNextMoveDirection();
    }

    void Idle()
    {
        Debug.Log("정지");
        // 일정 시간이 지나면 다음 행동을 결정하고 이동 상태로 전환
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
        // BoxCollider2D의 크기를 사용하여 충돌 체크
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxCollider2D.size, 0f);

        foreach (Collider2D collider in colliders)
        {
            // 특정 콜라이더에 충돌 시 이동을 멈추고 다음 이동 방향 설정
            if (collider.CompareTag("Area"))
            {
                Debug.Log("충돌");
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
