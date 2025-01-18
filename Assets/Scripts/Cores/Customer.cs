using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public LevelData levelData;
    public int levelNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned.");
            return;
        }

        LevelData.Level level = GetLevel(levelNumber);
        if (level != null && level.prefab != null)
        {
            GameObject prefabInstance = Instantiate(level.prefab, transform);
            prefabInstance.transform.localPosition = Vector3.zero; // 可根据需要调整位置
        }
        else
        {
            Debug.LogError("Level or Prefab is not found.");
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
