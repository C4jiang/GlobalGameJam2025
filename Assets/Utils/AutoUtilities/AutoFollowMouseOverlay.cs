using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoFollowMouseOverlay : MonoBehaviour {
    public RectTransform rectTransform;
    void Update() {
        // 获取鼠标在屏幕上的位置
        rectTransform.anchoredPosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
    }
}