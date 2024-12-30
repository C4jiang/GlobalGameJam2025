using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoShake : MonoBehaviour {

    public float strength = 1;
    public int vibrato = 10;
    public float time = 1;
    public float randomness = 90;

    private Tween tween;
    private Vector3 defaultPos;


    private void Awake() {
        defaultPos = transform.localPosition;
    }

    private void OnEnable() {
        if (tween != null && tween.IsActive() && tween.IsPlaying()) {
            tween.Kill();
            tween = null;
        }
        tween = transform.DOShakePosition(time, strength, vibrato, randomness).SetRelative(true).SetLoops(-1);
    }

    private void OnDisable() {
        if (tween != null && tween.IsActive() && tween.IsPlaying()) {
            tween.Kill();
            tween = null;
        }

        transform.localPosition = defaultPos;
    }

    void Update() {
        if (tween != null && tween.IsActive() && !tween.IsPlaying()) {
            tween.Kill();
            tween = null;
            transform.DOLocalMove(defaultPos, 0.1f);
        }
    }
}