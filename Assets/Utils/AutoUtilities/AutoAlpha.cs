using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoAlpha : MonoBehaviour {

    public float to;
    public float time;
    public Ease ease;
    public SpriteRenderer sr;

    private Tween tween;
    void OnEnable() {
        tween = sr.DOFade(to, time).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

    void OnDisable() {
        if (tween == null || !tween.IsPlaying())
            return;
        tween.Kill();
    }
}