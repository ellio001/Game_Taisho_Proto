using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty_Cursor : MonoBehaviour
{
    RectTransform rectTransform = null;
    [SerializeField] Vector3 _maxScale; // 鍋以外でのUIカーソルのScale

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // カーソルアイコンの拡縮制御
        rectTransform.localScale =
            (Mathf.Sin(1 * Mathf.PI * Time.time) + 3.5f) * 0.55f * _maxScale;
    }
}
