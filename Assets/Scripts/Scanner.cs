using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Scanner : MonoBehaviour
{
    public float scanRnage; // �����ϴ� ����
    public float getRange;
    public LayerMask[] targetLayer;
    public RaycastHit2D[] targets;
    public RaycastHit2D[] Bricks;
    public RaycastHit2D[] items;
    public Transform nearestTarget;
    public Transform nearestBrick;
    public Transform FirstTarget;
    public Vector2 TargetPos;
    public Vector2 dir;
    public Tilemap tilemap;

    Player player;

    void Awake()
    {
        player = gameObject.GetComponent<Player>();
    }

    void FixedUpdate()
    {
        // 1. ĳ���� ���� ��ġ, 2. ������ ����, 3. ĳ���� ����, 4. ĳ���� ����, 5. Ÿ�� ���̾�
        targets = Physics2D.CircleCastAll(transform.position, scanRnage, Vector2.zero, 0, targetLayer[0]);
        Bricks = Physics2D.CircleCastAll(transform.position, scanRnage, Vector2.zero, 0, targetLayer[1]);
        items = Physics2D.CircleCastAll(transform.position, getRange, Vector2.zero, 0, targetLayer[2]);
        itemMagnet(items);

        nearestTarget = GetNearest();
        nearestBrick = BricksGetNearest();
        FirstTarget = nearestTarget;

        // ���� �긯 �� �� ������
        if (nearestTarget != null && nearestBrick != null)
        {
            //���� �켱 Ÿ��
            FirstTarget = nearestTarget;
            TargetPos = FirstTarget.position;
        }
        // ��ó�� �긯�� ������
        else if(nearestTarget == null && nearestBrick != null)
        {
            FirstTarget = nearestBrick;
            tilemap = FirstTarget.GetComponent<Tilemap>();
            GetClosestTilePositionAndDirection(player.transform.position);
        }
        // ���� ���� ��
        else if(nearestTarget != null && nearestBrick == null)
        {
            FirstTarget = nearestTarget;
            TargetPos = FirstTarget.position;
        }
        else
        {
            Debug.LogError("��ó �ƹ��͵� ����");
        }
        // Ÿ�� �����
        Debug.DrawLine(transform.position, TargetPos, Color.blue, 0.3f);
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
    void GetClosestTilePositionAndDirection(Vector3 currentPosition)
    {
        Vector3Int playerCellPosition = tilemap.WorldToCell(currentPosition);

        Vector3Int closestTilePosition = Vector3Int.zero; // �ʱ�ȭ
        float closestDistance = float.MaxValue; // �ʱ�ȭ
        int radiusInt = Mathf.RoundToInt(scanRnage);
        // �ֺ� Ÿ���� �˻��մϴ�.
        for (int x = -radiusInt; x <= radiusInt; x++)
        {
            for (int y = -radiusInt; y <= radiusInt; y++)
            {
                Vector3Int cellPosition = new Vector3Int(playerCellPosition.x + x, playerCellPosition.y + y, playerCellPosition.z);

                // Ÿ���� �����ϴ��� Ȯ���ϰ�, �����ϸ� �ش� Ÿ���� ���� ��ǥ�� ���մϴ�.
                TileBase tile = tilemap.GetTile(cellPosition);
                if (tile != null)
                {
                    Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(cellPosition);

                    // ���� ��ġ���� ���� ����� Ÿ���� ã���ϴ�.
                    float distance = Vector3.Distance(currentPosition, tileWorldPosition);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTilePosition = cellPosition;
                    }
                }
            }
        }

        // ���� ����� Ÿ���� ������ ���մϴ�.
        Vector3 closestTileWorldPosition = tilemap.GetCellCenterWorld(closestTilePosition);
        Vector3 direction = closestTileWorldPosition - currentPosition;
        direction.Normalize(); // ������ ����ȭ�մϴ�.

        dir = direction;
        TargetPos = closestTileWorldPosition;
    }

    void itemMagnet(RaycastHit2D[] items)
    {
        foreach (RaycastHit2D item in items)
        {
            item.transform.GetComponent<DropItem>().magnet = true;
        }
    }

}
