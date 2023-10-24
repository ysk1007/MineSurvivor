using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRnage; // �����ϴ� ����
    public LayerMask targetLayer; 
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    Player player;

    void Awake()
    {
        player = gameObject.GetComponent<Player>();
    }

    void FixedUpdate()
    {
        // 1. ĳ���� ���� ��ġ, 2. ������ ����, 3. ĳ���� ����, 4. ĳ���� ����, 5. Ÿ�� ���̾�
        targets = Physics2D.CircleCastAll(transform.position, scanRnage, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
        //player.Attack();
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

}
