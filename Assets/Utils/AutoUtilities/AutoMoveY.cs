using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoMoveY : MonoBehaviour {

    public float toY;
    public float time;
    public Ease ease = Ease.Linear;
    public int loopCount = -1;
    private Tween tween;
    private float initY;
    void OnEnable() {
        initY = transform.localPosition.y;
        tween = transform.DOLocalMoveY(toY, time).SetRelative(true).SetEase(ease).SetLoops(loopCount, LoopType.Yoyo);
    }

    void OnDisable() {
        var pos = transform.localPosition;
        pos.y = initY;
        transform.localPosition = pos;
        if (tween == null || !tween.IsPlaying())
            return;
        tween.Kill();
    }
}