using System;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class BubbleManager: MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private LevelData _fullLevelData;
    [SerializeField] private GameFlow _flow;
    [SerializeField] private TextMeshProUGUI _simularityRate;
    public LevelData.Level CurLevelData => _fullLevelData.GetLevel(_flow.CurLevelNum);
    private Bubble _bubbleScript;
    
    void Start()
    {
        Messenger.AddListener<int>(MsgType.BlowBubble, OnBlowBubble);
        Messenger.AddListener<float>(MsgType.BlowSuccess, OnRenewBubble);
    }
    
    [Button("BlowBubble")]
    public void OnBlowBubble(int stability)
    {
        GameObject bubble = Instantiate(_bubblePrefab, transform);
        _bubbleScript = bubble.GetComponent<Bubble>();
        _bubbleScript.Blow(stability, this);
    }
    
    public void OnRenewBubble(float stability)
    {
        _bubbleScript.Kill();
        OnBlowBubble((int)stability);
    }

    private void Update()
    {
        if (_bubbleScript != null)
        {
            if (_bubbleScript.similarity <= _bubbleScript.similarValue)
            {
                _simularityRate.text = "100%";
            }
            else
            {
                _simularityRate.text = ((int)(100f / ((_bubbleScript.similarity - _bubbleScript.similarValue)
                                              / _bubbleScript.similarValue + 1f))).ToString() + "%";
            }
            
        }
    }
}