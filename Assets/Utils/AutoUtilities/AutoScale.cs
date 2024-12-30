using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoScale : MonoBehaviour {

    public Vector3 toScale;
    public float time;
    public Ease ease;
    public string curve;
    public int loopCount = -1;
    public LoopType loopType = LoopType.Yoyo;
    public bool isIgnoreTime = false;
    public float interval = 0;

    private Vector3 defaultScale;


    private void Awake() {
        defaultScale = transform.localScale;
    }

    private Sequence tween;
    void OnEnable() {
        if (string.IsNullOrEmpty(curve)) {
            tween = DOTween.Sequence().SetUpdate(isIgnoreTime)
                .Insert(0, transform.DOScale(toScale, time).SetEase(ease));
        }
        else {
            // tween = DOTween.Sequence().SetUpdate(isIgnoreTime)
            //     .Insert(0, transform.DOScale(toScale, time).SetEase(GameRes.Instance.GetCurve(curve)));
        }
        if (interval > Mathf.Epsilon)
            tween.AppendInterval(interval);
        tween.SetLoops(loopCount, loopType);
    }

    void OnDisable() {
        if (tween != null && tween.IsPlaying()) {
            tween.Kill();
            tween = null;
        }

        transform.localScale = defaultScale;
    }
}