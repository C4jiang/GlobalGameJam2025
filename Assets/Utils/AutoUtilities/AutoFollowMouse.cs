using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoFollowMouse : MonoBehaviour {
    void Update() {
        // 获取鼠标在屏幕上的位置
        Vector2 mousePosition = Input.mousePosition;
        // 将鼠标位置转换为世界坐标
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = -2;
        transform.position = worldPosition;
    }
}