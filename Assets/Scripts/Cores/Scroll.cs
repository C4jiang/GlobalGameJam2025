using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scroll : MonoBehaviour
{
    public float scrollSpeed = 20f; // 滚动速度
    public TextMeshProUGUI textMeshPro; // 引用到 TextMeshProUGUI 组件

    private RectTransform textRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI is not assigned.");
            return;
        }

        textRectTransform = textMeshPro.GetComponent<RectTransform>();
        if (textRectTransform == null)
        {
            Debug.LogError("RectTransform is not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (textRectTransform != null)
        {
            textRectTransform.anchoredPosition -= Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }
}