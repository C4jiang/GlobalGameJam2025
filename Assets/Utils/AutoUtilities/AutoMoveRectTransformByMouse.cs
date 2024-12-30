using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoMoveRectTransformByMouse : MonoBehaviour {
    public float followRate = 1f;
    RectTransform rect = null;
    public void Update() {
        if(rect == null) {
            rect = GetComponent<RectTransform>();
            if(rect == null) {
                return;
            }
        }

        Vector2 mouse = Input.mousePosition;
        rect.anchoredPosition = mouse * followRate;
    }
}