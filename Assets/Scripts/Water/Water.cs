using System;
using UnityEngine;

public class Water : MonoBehaviour {
    public const int MaxWaterCnt = 4;

    [Header("稳定魔药系数(填负数)")]
    public float NormalWaterStableRatio;
    [Header("不稳定魔药系数")]
    public float UnstableWaterStableRatio;
    [Header("泡泡稳定阈值")]
    public float BubbleStableThreshold;
    private int _normalWaterCnt = 0;
    private int _unstableWaterCnt = 0; 

    /// <summary>
    /// 如果有需求，直接从外部设置这三个参数
    /// </summary>
    /// <param name="NormalWaterStableRatio">稳定魔药系数(填负数)</param>
    /// <param name="UnstableWaterStableRatio">不稳定魔药系数"</param>
    /// <param name="BubbleStableThreshold">泡泡稳定阈值</param>
    public void SetWaterArgs(float NormalWaterStableRatio, float UnstableWaterStableRatio, float BubbleStableThreshold) {
        this.NormalWaterStableRatio = NormalWaterStableRatio;
        this.UnstableWaterStableRatio = UnstableWaterStableRatio;
        this.BubbleStableThreshold = BubbleStableThreshold;
    }

    public void AddNormalWater() {
        _normalWaterCnt = Math.Min(_normalWaterCnt + 1, MaxWaterCnt);
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
        Debug.Log("RefreshWaterCnt " + _normalWaterCnt + " " + _unstableWaterCnt);
    }

    public void AddUnstableWater() {
        _unstableWaterCnt = Math.Min(_unstableWaterCnt + 1, MaxWaterCnt);
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
        Debug.Log("RefreshWaterCnt " + _normalWaterCnt + " " + _unstableWaterCnt);
    }

    public void RemoveNormalWater() {
        _normalWaterCnt = Math.Max(_normalWaterCnt - 1, 0);
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
        Debug.Log("RefreshWaterCnt " + _normalWaterCnt + " " + _unstableWaterCnt);
    }

    public void RemoveUnstableWater() {
        _unstableWaterCnt = Math.Max(_unstableWaterCnt - 1, 0);
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
        Debug.Log("RefreshWaterCnt " + _normalWaterCnt + " " + _unstableWaterCnt);
    }

    public void ResetWater() {
        _normalWaterCnt = 0;
        _unstableWaterCnt = 0;
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
        Debug.Log("RefreshWaterCnt " + _normalWaterCnt + " " + _unstableWaterCnt);
    }

    public void Blow() {
        var stability = CalcStability(_normalWaterCnt, _unstableWaterCnt);
        if (stability <= BubbleStableThreshold) {
            Messenger.Broadcast<float>(MsgType.BlowSuccess, stability);
            Debug.Log("BubbleSuccess " + stability);
        } else {
            Messenger.Broadcast<float>(MsgType.BlowFail, stability);
            Debug.Log("BubbleFail " + stability);
        }
        ResetWater();
    }

    private float CalcStability(int normalWaterCnt, int unstableWaterCnt) {
        return _normalWaterCnt * NormalWaterStableRatio + _unstableWaterCnt * UnstableWaterStableRatio;
    }
}