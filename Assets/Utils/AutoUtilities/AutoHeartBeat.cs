using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoHeartBeat : MonoBehaviour {

    public float startSize = 1f;
    public float interval = 2.2f;

    Sequence heartBeat = DOTween.Sequence();

    void Awake() {
        Vector3 startScaleVec = transform.localScale;
        heartBeat.Append(transform.DOScale(startScaleVec * 1.15f, 0.1f).SetEase(Ease.OutCubic));
        heartBeat.Append(transform.DOScale(startScaleVec * 1f, 0.3f).SetEase(Ease.OutCubic));
        heartBeat.PrependInterval(interval);
        heartBeat.SetLoops(-1, LoopType.Restart);
    }

    void OnDisable() {
        heartBeat.Kill();
    }
}