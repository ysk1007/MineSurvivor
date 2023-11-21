using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRnage; // 감지하는 범위
    public LayerMask[] targetLayer;
    public RaycastHit2D[] targets;
    public RaycastHit2D[] Bricks;
    public Transform nearestTarget;
    public Transform nearestBrick;
    public Transform FirstTarget;
    public Vector2 dir;

    Player player;

    void Awake()
    {
        player = gameObject.GetComponent<Player>();
    }

    void FixedUpdate()
    {
        // 1. 캐스팅 시작 위치, 2. 반지름 길이, 3. 캐스팅 방향, 4. 캐스팅 길이, 5. 타겟 레이어
        targets = Physics2D.CircleCastAll(transform.position, scanRnage, Vector2.zero, 0, targetLayer[0]);
        Bricks = Physics2D.CircleCastAll(transform.position, scanRnage, Vector2.zero, 0, targetLayer[1]);
        nearestTarget = GetNearest();
        nearestBrick = BricksGetNearest();
        FirstTarget = nearestTarget;
        if (nearestTarget != null && nearestBrick != null)
        {
            FirstTarget = nearestTarget;
        }
        else if(nearestTarget == null && nearestBrick != null)
        {
            FirstTarget = nearestBrick;
        }

        if (player != null && FirstTarget != null)
        {
            Vector2 pointOnCollider1;
            Vector2 pointOnCollider2;

            float distance = FindClosestDistance(player.GetComponent<Collider2D>(), FirstTarget.GetComponent<Collider2D>(), out pointOnCollider1, out pointOnCollider2);
            Debug.Log("Distance between Object1 and Object2: " + distance);

            Vector2 direction = pointOnCollider2 - pointOnCollider1;
            Debug.Log("Direction from Object1 to Object2: " + direction.normalized);
            dir = direction.normalized;
        }
        else
        {
            Debug.LogError("One or both objects are not assigned!");
        }
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }

    Transform BricksGetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in Bricks)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
    float FindClosestDistance(Collider2D collider1, Collider2D collider2, out Vector2 pointOnCollider1, out Vector2 pointOnCollider2)
    {
        pointOnCollider1 = collider1.bounds.ClosestPoint(collider2.transform.position);
        pointOnCollider2 = collider2.bounds.ClosestPoint(collider1.transform.position);

        float distance = Vector2.Distance(pointOnCollider1, pointOnCollider2);

        Debug.DrawLine(pointOnCollider1, pointOnCollider2, Color.red, 0.1f);

        return distance;
    }

}
