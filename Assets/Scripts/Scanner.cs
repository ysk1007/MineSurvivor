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

}
