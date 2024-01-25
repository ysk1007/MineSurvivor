using NavMeshPlus;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface surface;

    public Vector3 BoundsCenter;
    public Vector3 BoundsSize;

    public LayerMask BuildMask;
    public LayerMask NullMask;

    public NavMeshData NavMeshData;
    NavMeshDataInstance NavMeshDataInstance;

    void Start()
    {
        AddNavMeshData();
        BuildMask = ~0;
        NullMask = 0;
    }

    void Update()
    {
        BoundsCenter = GameManager.instance.player.transform.position;
        Debug.Log("BoundsCenter: " + BoundsCenter.ToString());
        Debug.Log("BoundsSize: " + BoundsSize.ToString());

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Build " + Time.realtimeSinceStartup.ToString());
            Build();
            Debug.Log("Build finished " + Time.realtimeSinceStartup.ToString());
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Update " + Time.realtimeSinceStartup.ToString());
            UpdateNavmeshData();
        }
    }

    void AddNavMeshData()
    {
        if (NavMeshData != null)
        {
            if (NavMeshDataInstance.valid)
            {
                NavMesh.RemoveNavMeshData(NavMeshDataInstance);
            }
            NavMeshDataInstance = NavMesh.AddNavMeshData(NavMeshData);
        }

        if (surface != null)
        {
            surface.navMeshData = NavMeshData;
        }
    }

    void UpdateNavmeshData()
    {
        StartCoroutine(UpdateNavmeshDataAsync());
    }

    IEnumerator UpdateNavmeshDataAsync()
    {
        AsyncOperation op = NavMeshBuilder.UpdateNavMeshDataAsync(
            NavMeshData,
            NavMesh.GetSettingsByID(0),
            GetBuildSources(BuildMask),
            new Bounds(BoundsCenter, BoundsSize));
        yield return op;

        AddNavMeshData();
        Debug.Log("Update finished " + Time.realtimeSinceStartup.ToString());
    }

    void Build()
    {
        Vector3 eulerAngles = new Vector3(-90f, 0f, 0f);
        Quaternion rotation = Quaternion.Euler(eulerAngles);
        NavMeshData = NavMeshBuilder.BuildNavMeshData(
            NavMesh.GetSettingsByID(0),
            GetBuildSources(NullMask),
            new Bounds(BoundsCenter, BoundsSize),
            Vector3.zero,
            rotation);
        AddNavMeshData();
    }

    List<NavMeshBuildSource> GetBuildSources(LayerMask mask)
    {
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(
            new Bounds(BoundsCenter, BoundsSize),
            mask,
            NavMeshCollectGeometry.PhysicsColliders,
            0,
            new List<NavMeshBuildMarkup>(),
            sources);
        // 디버그: 수집된 소스의 개수와 각 Source의 정보 출력
        Debug.Log("Sources found: " + sources.Count.ToString());
        foreach (var source in sources)
        {
            Debug.Log("Source: " + source.sourceObject.name + ", Type: " + source.component);
        }
        return sources;
    }
}
