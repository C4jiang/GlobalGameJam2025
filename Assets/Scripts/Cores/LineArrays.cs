using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LineArrays : MonoBehaviour
{
    public LevelData levelData;
    public int levelNumber;

    public GameFlow flow;

    LineRenderer lineRenderer;

    public float scale = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        OnDestroyLine();
        Messenger.AddListener(MsgType.BubbleSuccess, OnDestroyLine);
        Messenger.AddListener<int>(MsgType.BlowBubble, OnCreateLine);
    }

    [Button("ConnectLine")]
    public void ConnectLine()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.widthMultiplier = 0.1f;
        
        LevelData.Level level = levelData.GetLevel(levelNumber);
        if (level != null)
        {
            Vector3[] linePositions = new Vector3[level.pattern.positions.Count + 1];

            for (int i = 0; i < level.pattern.positions.Count; i++)
            {
                linePositions[i] = level.pattern.positions[i]*scale;
            }
            // 将最后一个点设置为第一个点，以形成封闭图形
            linePositions[level.pattern.positions.Count] = level.pattern.positions[0]*scale;

            lineRenderer.positionCount = linePositions.Length;
            lineRenderer.SetPositions(linePositions);
        }
    }

    public void OnDestroyLine()
    {
        Destroy(lineRenderer);
    }

    public void OnCreateLine(int stability)
    {
        this.levelNumber = flow.CurLevelNum;
        ConnectLine();
    }
}
