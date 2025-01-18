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
        LevelData.Level level = GetLevel(levelNumber);
        if (level != null)
        {
            lineRenderer.positionCount = level.pattern.positions.Length;
            lineRenderer.SetPositions(level.pattern.positions);
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
                Debug.Log("Level found: " + levelNumber);
                return level;
            }
        }
        return null;
    }
}
