using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Scanner : MonoBehaviour
{
    public float scanRnage; // 감지하는 범위
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
        // 1. 캐스팅 시작 위치, 2. 반지름 길이, 3. 캐스팅 방향, 4. 캐스팅 길이, 5. 타겟 레이어
        targets = Physics2D.CircleCastAll(transform.position, scanRnage, Vector2.zero, 0, targetLayer[0]);
        Bricks = Physics2D.CircleCastAll(transform.position, scanRnage, Vector2.zero, 0, targetLayer[1]);
        items = Physics2D.CircleCastAll(transform.position, getRange, Vector2.zero, 0, targetLayer[2]);
        itemMagnet(items);

        nearestTarget = GetNearest();
        nearestBrick = BricksGetNearest();
        FirstTarget = nearestTarget;

        // 적과 브릭 둘 다 있을때
        if (nearestTarget != null && nearestBrick != null)
        {
            //적을 우선 타겟
            FirstTarget = nearestTarget;
            TargetPos = FirstTarget.position;
        }
        // 근처에 브릭만 있을때
        else if(nearestTarget == null && nearestBrick != null)
        {
            FirstTarget = nearestBrick;
            tilemap = FirstTarget.GetComponent<Tilemap>();
            GetClosestTilePositionAndDirection(player.transform.position);
        }
        // 적만 있을 때
        else if(nearestTarget != null && nearestBrick == null)
        {
            FirstTarget = nearestTarget;
            TargetPos = FirstTarget.position;
        }
        else
        {
            Debug.LogError("근처 아무것도 없음");
        }
        // 타겟 디버그
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

        Vector3Int closestTilePosition = Vector3Int.zero; // 초기화
        float closestDistance = float.MaxValue; // 초기화
        int radiusInt = Mathf.RoundToInt(scanRnage);
        // 주변 타일을 검사합니다.
        for (int x = -radiusInt; x <= radiusInt; x++)
        {
            for (int y = -radiusInt; y <= radiusInt; y++)
            {
                Vector3Int cellPosition = new Vector3Int(playerCellPosition.x + x, playerCellPosition.y + y, playerCellPosition.z);

                // 타일이 존재하는지 확인하고, 존재하면 해당 타일의 월드 좌표를 구합니다.
                TileBase tile = tilemap.GetTile(cellPosition);
                if (tile != null)
                {
                    Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(cellPosition);

                    // 현재 위치에서 가장 가까운 타일을 찾습니다.
                    float distance = Vector3.Distance(currentPosition, tileWorldPosition);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTilePosition = cellPosition;
                    }
                }
            }
        }

        // 가장 가까운 타일의 방향을 구합니다.
        Vector3 closestTileWorldPosition = tilemap.GetCellCenterWorld(closestTilePosition);
        Vector3 direction = closestTileWorldPosition - currentPosition;
        direction.Normalize(); // 방향을 정규화합니다.

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
