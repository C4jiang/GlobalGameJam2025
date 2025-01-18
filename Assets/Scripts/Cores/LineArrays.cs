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
        lineRenderer.widthMultiplier = 1.0f;
        
        LevelData.Level level = GetLevel(levelNumber);
        if (level != null)
        {
            lineRenderer.positionCount = level.pattern.positions.Length;
            lineRenderer.SetPositions(level.pattern.positions);
            lineRenderer.SetPosition(0, level.pattern.positions[0]);
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
