using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoShakeByEvent : MonoBehaviour {

    public float strength = 1;
    public int vibrato = 10;
    public float time = 1;
    public float randomness = 90;

    private Tween tween;
    private Vector3 defaultPos;


    private void Awake() {
        defaultPos = transform.localPosition;
        Messenger.AddListener<int, GameObject>(MsgType.BattleHurt, DamageDisplay);
        Messenger.AddListener<int, GameObject>(MsgType.BattleDefence, ShieldGetAttackedDisplay);
    }

    private void ShieldGetAttackedDisplay(int value, GameObject obj) {
        if (tween != null && tween.IsActive() && tween.IsPlaying()) {
            tween.Kill();
            tween = null;
        }
        tween = transform.DOShakePosition(0.3f, 0.25f, 6, 15).SetRelative(true);
    }

    private void DamageDisplay(int value, GameObject obj) {
        if (tween != null && tween.IsActive() && tween.IsPlaying()) {
            tween.Kill();
            tween = null;
            transform.localPosition = defaultPos;
        }
        tween = transform.DOShakePosition(time, strength, vibrato, randomness).SetRelative(true);
    }
    
    void Update() {
        if (tween != null && tween.IsActive() && !tween.IsPlaying()) {
            tween.Kill();
            tween = null;
            transform.DOLocalMove(defaultPos, 0.1f);
        }
    }
}