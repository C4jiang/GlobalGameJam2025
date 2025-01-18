using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineArrays : MonoBehaviour
{
    public LevelData levelData;
    public int levelNumber;

    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.widthMultiplier = 5.0f;
        
        LevelData.Level level = GetLevel(levelNumber);
        if (level != null)
        {
            Vector3[] linePositions = new Vector3[level.pattern.positions.Length + 1];

            for (int i = 0; i < level.pattern.positions.Length; i++)
            {
                linePositions[i] = level.pattern.positions[i];
            }
            // 将最后一个点设置为第一个点，以形成封闭图形
            linePositions[level.pattern.positions.Length] = level.pattern.positions[0];

            lineRenderer.positionCount = linePositions.Length;
            lineRenderer.SetPositions(linePositions);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private LevelData.Level GetLevel(int levelNumber)
    {
        if (levelData == null || levelData.GetLevels() == null)
        {
            return null;
        }

        foreach (LevelData.Level level in levelData.GetLevels())
        {
            if (level.levelNumber == levelNumber)
            {
                return level;
            }
        }
        return null;
    }
}
