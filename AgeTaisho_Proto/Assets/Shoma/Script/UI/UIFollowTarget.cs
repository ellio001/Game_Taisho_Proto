using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIFollowTarget : MonoBehaviour
{
    RectTransform rectTransform = null;
    Transform target = null;
    [SerializeField]
    Canvas canvas;

    GameObject C3;
    Camera_3 C3_script;

    GameObject TC3;
    Tutorial_Camera_3 TC3_script;

    [SerializeField] Vector3 _maxScale; // 鍋以外でのUIカーソルのScale
    Vector3 _PmaxScale;     // 鍋でのUIカーソルのScale
    Vector3 _FInScale; // 最終的に決まったスケールを入れる

    const float Scale_val = 0.00001f; // 拡大縮小する値

    bool Tuto_flg = false; // 今このsceneがチュートリアルならフラグを立てる

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponent<Graphic>().canvas;
        if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            TC3 = GameObject.Find("Main Camera");
            TC3_script = TC3.GetComponent<Tutorial_Camera_3>();
            Tuto_flg = true;
        }
        else
        {
            C3 = GameObject.Find("Main Camera");
            C3_script = C3.GetComponent<Camera_3>();
            Tuto_flg = false;
        }

        _PmaxScale = new Vector3(_maxScale.x - 0.0002f, _maxScale.y - 0.0002f, _maxScale.z);
    }

    void Update()
    {
        TargetSelect();
        
        var pos = Vector2.zero;
        var uiCamera = Camera.main;
        var worldCamera = Camera.main;
        var canvasRect = canvas.GetComponent<RectTransform>();

        var screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out pos);
        rectTransform.localPosition = pos;
        
        // カーソルアイコンの拡縮制御
        rectTransform.localScale =
            (Mathf.Sin(1 * Mathf.PI * Time.time) + 3.5f) * 0.55f * _FInScale;

    }



    void TargetSelect()
    {
        if (Tuto_flg)
        {
            if (TC3_script.pot_flg)
            {
                target = TC3_script.PCS_List[TC3_script.Pcursor].transform;
                _FInScale = new Vector3(_PmaxScale.x, _PmaxScale.y, _PmaxScale.z);
            }
            else
            {
                target = TC3_script.Cursor_List[TC3_script.cursor].transform;
                _FInScale = new Vector3(_maxScale.x, _maxScale.y, _maxScale.z);
            }
        }
        else
        {
            if (C3_script.pot_flg)
            {
                target = C3_script.PCS_List[C3_script.Pcursor].transform;
                _FInScale = new Vector3(_PmaxScale.x, _PmaxScale.y, _PmaxScale.z);
            }
            else
            {
                target = C3_script.Cursor_List[C3_script.cursor].transform;
                _FInScale = new Vector3(_maxScale.x, _maxScale.y, _maxScale.z);
            }
        }
    }

}