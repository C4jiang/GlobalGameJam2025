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

    public void AddNormalWater() {
        _normalWaterCnt = Math.Min(_normalWaterCnt + 1, MaxWaterCnt);
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
    }

    public void AddUnstableWater() {
        _unstableWaterCnt = Math.Min(_unstableWaterCnt + 1, MaxWaterCnt);
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
    }

    public void RemoveNormalWater() {
        _normalWaterCnt = Math.Max(_normalWaterCnt - 1, 0);
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
    }

    public void RemoveUnstableWater() {
        _unstableWaterCnt = Math.Max(_unstableWaterCnt - 1, 0);
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
    }

    public void ResetWater() {
        _normalWaterCnt = 0;
        _unstableWaterCnt = 0;
        Messenger.Broadcast<int, int>(MsgType.RefreshWaterCnt, _normalWaterCnt, _unstableWaterCnt);
    }

    public void BubbleWater() {
        var stability = CalcStability(_normalWaterCnt, _unstableWaterCnt);
        if (stability <= BubbleStableThreshold) {
            Messenger.Broadcast(MsgType.BubbleSuccess);
        } else {
            Messenger.Broadcast(MsgType.BubbleFail);
        }
    }

    private float CalcStability(int normalWaterCnt, int unstableWaterCnt) {
        // todo
        return _normalWaterCnt * NormalWaterStableRatio + _unstableWaterCnt * UnstableWaterStableRatio;
    }
}