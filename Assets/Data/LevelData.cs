using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "自定义数据集/LevelData")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class Pattern
    {
        public int size;
        public List<Vector2> positions;
    }

    [System.Serializable]
    public class Level
    {
        public int levelNumber;
        public Pattern pattern;
        public GameObject prefab;
        public GameObject dialogPrefab;
        public List<string> dialogs;
        public List<string> introDialogs;
        public List<string> successBubbleDialogs;
        public List<string> failBubbleDialogs;
        public List<string> idleDialogs;
        public string dialog;
    }

    [SerializeField] private Level[] levels;

    public Level[] GetLevels()
    {
        return levels;
    }
    
    public Level GetLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Length)
        {
            return levels[levelIndex];
        }
        else
        {
            Debug.Log("no levels!!");
            return null;
        }
    }
}
