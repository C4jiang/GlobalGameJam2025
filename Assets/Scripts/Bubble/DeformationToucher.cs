using System;
using UnityEngine;

public class DeformationToucher : MonoBehaviour
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
    
    private float totalOriginalDistance;
    private float averageOriginalDistance;

    void Start()
    {
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
    }

    void Update()
    {
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

        float currentAverageDistance = 0f;
        for (int i = 0; i < verticesCount; i++)
        {
            currentAverageDistance += (displacedVertices[i] - centerPoint).magnitude;
        }
        currentAverageDistance /= verticesCount;

        for (int i = 0; i < verticesCount; i++)
        {
            float centerForce = (currentAverageDistance - averageOriginalDistance) / averageOriginalDistance;
            vertexVelocities[i] += centerForce * springForce * Time.deltaTime * (centerPoint - displacedVertices[i]);//加上+顶点当前位置指向顶点初始位置的速度向量==回弹力
            vertexVelocities[i] *= 1f - damping * Time.deltaTime;//乘上阻力
            displacedVertices[i] += vertexVelocities[i] * Time.deltaTime;//算出顶点的下一个位置
        }

        targetMesh.vertices = displacedVertices;
        targetMesh.RecalculateBounds();
        targetMesh.RecalculateNormals();
        targetMeshFilter.mesh = targetMesh;
        meshCollider.sharedMesh = targetMesh;
    }
}