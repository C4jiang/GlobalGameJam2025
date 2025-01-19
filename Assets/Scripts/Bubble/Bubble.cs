using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

using System.Linq;
using System.Timers;
using DG.Tweening;
using FMODUnity;

public class Bubble : MonoBehaviour
{
    public MeshFilter targetMeshFilter;
    private Mesh targetMesh;
    MeshCollider meshCollider;

    public Camera mainCamera;

    private Vector3[] originalVertices, displacedVertices, vertexVelocities;
    private Vector3 centerPoint;

    private int verticesCount;

    public float force = 10;
    public float forceOffset = 0.1f;
    public float springForce = 20f;
    public float damping = 5f;
    public float maxBounceForce = 3f;
    public float edgePointMinDistance = 0.1f;

    public int timer = 0;
    public int refreshTime = 2;
    
    public float judgeTimer = 0;
    public float judgeMaxTimer = 1;
    
    private float totalOriginalDistance;
    private float averageOriginalDistance;
    
    private Vector3 lightDirection;
    
    private List<Vector2> bubbleEdgePoints = new List<Vector2>();
    public List<Vector2> myEdgePoints = new List<Vector2>();

    private BubbleManager _bubbleManager;
    private LevelData.Level _curLevelData => _bubbleManager.CurLevelData;
    
    
    public float similarValue = 0.0015f;
    public int similarCnt = 0;
    public int similarCntMax = 3;
    public float similarity = 10;
    
    public EventReference bubbleEffect;
    public EventReference bubbleKill;
    public EventReference bubbleBlow;
    public EventReference bubbleSuccess;
    
    public void Blow(float stability, BubbleManager manager)
    {
        Messenger.Broadcast(MsgType.PlaySE, bubbleBlow);
        _bubbleManager = manager;
        mainCamera = Camera.main;
        lightDirection = Vector3.forward;
        Debug.Log(lightDirection);
        meshCollider = GetComponent<MeshCollider>();
        targetMesh = targetMeshFilter.mesh;

        verticesCount = targetMesh.vertices.Length;

        originalVertices = targetMesh.vertices;
        displacedVertices = targetMesh.vertices;
        meshCollider.sharedMesh = targetMesh;
        vertexVelocities = new Vector3[verticesCount];
        
        // 计算中心点
        centerPoint = Vector3.zero;
        foreach (var vertex in originalVertices)
        {
            centerPoint += vertex;
        }
        centerPoint /= verticesCount;
        averageOriginalDistance = (originalVertices[0] - centerPoint).magnitude;

        transform.localScale = Vector3.one * 1;
        transform.DOScale(Vector3.one * 240f, 5f).SetEase(Ease.OutElastic);
    }

    void Update()
    {
        ShapeBubble();
        JudgeSimularity();
    }

    void ShapeBubble()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Messenger.Broadcast(MsgType.PlaySE, bubbleEffect);
        }
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
            {
                Vector3 actingForcePoint = targetMeshFilter.transform.InverseTransformPoint(hitInfo.point + hitInfo.normal * forceOffset);//发力点指向球的本地坐标向量

                for (int i = 0; i < verticesCount; i++)
                {
                    Vector3 pointToVertex = displacedVertices[i] - actingForcePoint;//作用力点指向当前顶点位置的向量

                    float actingForce = force / (1f + pointToVertex.sqrMagnitude);//作用力大小
                    vertexVelocities[i] += actingForce * Time.deltaTime * pointToVertex.normalized;//顶点速度向量
                }
            }
        }
        
        timer++;
        if (timer >= refreshTime)
        {
            timer = 0;
        }
        else
        {
            return;
        }
        

        float currentAverageDistance = 0f;
        for (int i = 0; i < verticesCount; i++)
        {
            currentAverageDistance += (displacedVertices[i] - centerPoint).magnitude;
        }
        currentAverageDistance /= verticesCount;

        for (int i = 0; i < verticesCount; i++)
        {
            float centerForce = (currentAverageDistance - averageOriginalDistance) / averageOriginalDistance;
            vertexVelocities[i] += centerForce * springForce * Time.deltaTime * refreshTime * (centerPoint - displacedVertices[i]);//加上+顶点当前位置指向顶点初始位置的速度向量==回弹力
            vertexVelocities[i] *= 1f - damping * Time.deltaTime * refreshTime;//乘上阻力
            displacedVertices[i] += Time.deltaTime * refreshTime * vertexVelocities[i];//算出顶点的下一个位置
        }

        targetMesh.vertices = displacedVertices;
        targetMesh.RecalculateBounds();
        targetMesh.RecalculateNormals();
        targetMeshFilter.mesh = targetMesh;
        meshCollider.sharedMesh = targetMesh;
    }

    void JudgeSimularity()
    {
        judgeTimer += Time.deltaTime;
        if (judgeTimer >= judgeMaxTimer)
        {
            judgeTimer = 0;
            if (_curLevelData != null)
            {
                similarity = CalculateBubbleSimilarity();
                if (similarity < similarValue)
                {
                    similarCnt ++;
                }
                else
                {
                    similarCnt = 0;
                }

                if (similarCnt >= similarCntMax)
                {
                    Success();
                }
            }
        }
    }

    void Success()
    {
        Messenger.Broadcast(MsgType.BubbleSuccess);
        Messenger.Broadcast(MsgType.PlaySE, bubbleSuccess);
        Kill();
    }

    public void Kill()
    {
        Messenger.Broadcast(MsgType.PlaySE, bubbleKill);
        Destroy(gameObject);
    }
    
    [Button("泡泡和目标的相似度")]
    float CalculateBubbleSimilarity()
    {
        ExtractEdgePoints();
        var similarity = PolygonSimilarity.CalculateSimilarity(bubbleEdgePoints, _curLevelData.pattern.positions);
        Debug.Log($"Similarity: {similarity}");
        return similarity;
    }
    
    void ExtractEdgePoints()
    {
        // var stopwatch = new System.Diagnostics.Stopwatch();
        // 简单的边缘检测算法（例如，基于顶点法线方向）
        bubbleEdgePoints.Clear();
        // Debug.Log(verticesCount);
        // stopwatch = new System.Diagnostics.Stopwatch();
        // stopwatch.Start();
        Vector3[] normals = targetMesh.normals;
        // stopwatch.Stop();
        // Debug.Log($"ExtractEdgePoints took {stopwatch.ElapsedTicks} ticks");
        for (int i = 0; i < verticesCount; i++)
        {
            if (Mathf.Abs(normals[i].z) < 0.02f) // 近似判断边缘点
            {
                bubbleEdgePoints.Add(new Vector2(targetMesh.vertices[i].x, targetMesh.vertices[i].y));
            }
        }
        // 对边缘点进行排序
        bubbleEdgePoints = SortEdgePoints(bubbleEdgePoints, edgePointMinDistance);
    }
    
    List<Vector2> SortEdgePoints(List<Vector2> edgePoints, float minDistance)
    {
        // 假设边缘点形成一个闭合曲线，可以使用贪心算法进行排序
        List<Vector2> sortedPoints = new List<Vector2>();
        Vector3 currentPoint = edgePoints[0];
        sortedPoints.Add(currentPoint);
        edgePoints.RemoveAt(0);

        while (edgePoints.Count > 0)
        {
            // 移除所有距离当前点小于阈值的点
            edgePoints.RemoveAll(p => Vector2.Distance(currentPoint, p) <= minDistance);
            if (edgePoints.Count == 0)
            {
                break;
            }
            // 找到距离当前点大于阈值的下一个点
            Vector2 nextPoint = edgePoints
                .OrderBy(p => Vector2.Distance(currentPoint, p))
                .First();
            if (nextPoint != default(Vector2))
            {
                // sortedPoints.Add(new Vector3(nextPoint.x, nextPoint.y, 0));
                sortedPoints.Add(nextPoint);
                currentPoint = nextPoint;
            }
        }

        return sortedPoints;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var point in bubbleEdgePoints)
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(point), 0.5f);
        }

        if (_bubbleManager != null && _curLevelData != null)
        {
            Gizmos.color = Color.blue;
            foreach (var point in _curLevelData.pattern.positions)
            {
                Gizmos.DrawWireSphere(transform.TransformPoint(point), 0.5f);
            }
        }
    }
}