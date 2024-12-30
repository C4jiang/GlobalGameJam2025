using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoMoveX : MonoBehaviour {

    public float toX;
    public float time;
    public Ease ease = Ease.Linear;
    public int loopCount = -1;
    private Tween tween;
    private float initX;
    void OnEnable() {
        initX = transform.localPosition.x;
        tween = transform.DOLocalMoveX(toX, time).SetRelative(true).SetEase(ease).SetLoops(loopCount, LoopType.Yoyo);
    }

    void OnDisable() {
        var pos = transform.localPosition;
        pos.x = initX;
        transform.localPosition = pos;
        if (tween == null || !tween.IsPlaying())
            return;
        tween.Kill();
    }
}