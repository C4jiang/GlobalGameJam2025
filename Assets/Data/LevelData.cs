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
        public Vector3[] positions;
    }

    [System.Serializable]
    public class Level
    {
        public int levelNumber;
        public Pattern pattern;
        public GameObject prefab;
        public string dialog;
    }

    [SerializeField] private Level[] levels;

    public Level[] GetLevels()
    {
        return levels;
    }
}
