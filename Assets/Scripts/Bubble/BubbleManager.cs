using UnityEngine;
using Sirenix.OdinInspector;
public class BubbleManager: MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private LevelData _fullLevelData;
    [SerializeField] private GameFlow _flow;
    public LevelData.Level CurLevelData => _fullLevelData.GetLevel(_flow.CurLevelNum);
    private Bubble _bubbleScript;
    
    void Start()
    {
        Messenger.AddListener<int>(MsgType.BlowBubble, OnBlowBubble);
    }
    
    [Button("BlowBubble")]
    public void OnBlowBubble(int stability)
    {
        GameObject bubble = Instantiate(_bubblePrefab, transform);
        _bubbleScript = bubble.GetComponent<Bubble>();
        _bubbleScript.Blow(stability, this);
    }
}