using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public LevelData levelData;
    public int levelNumber;
    public Text dialogText; // 引用到 DialogText 的文本框

    // Start is called before the first frame update
    void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned.");
            return;
        }

        LevelData.Level level = GetLevel(levelNumber);
        if (level != null && dialogText != null)
        {
            dialogText.text = level.dialog;
        }
        else
        {
            Debug.LogError("Level or DialogText is not found.");
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
