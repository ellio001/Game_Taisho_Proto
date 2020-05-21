using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIFollowTarget : MonoBehaviour
{
    RectTransform rectTransform = null;
    Transform target = null;

    [SerializeField]
    Canvas canvas;

    GameObject C3;
    Camera_3 C3_script;
    Vector3 CursorPos;

    Vector2 IconSize;
    Vector3 CanvasPos;
    RectTransform rect_tra;

    int count;
    const float Scale_val = 0.00001f; // 拡大縮小する値
    const int Count_return = 120; // 拡大縮小の一往復するまでのカウント値

    int tmp_cursor;
    bool flg=false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        IconSize = GetComponent<RectTransform>().sizeDelta;
        //CanvasPos = this.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        rect_tra = this.transform.parent.GetComponent<RectTransform>();
        canvas = GetComponent<Graphic>().canvas;

        C3 = GameObject.Find("Main Camera");
        C3_script = C3.GetComponent<Camera_3>();
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
        
        ScalOperation();
    }


    void TargetSelect()
    {
        if (tmp_cursor != C3_script.cursor || C3_script.pot_flg)
        {
            tmp_cursor = C3_script.cursor;
            if (C3_script.pot_flg)
            { // 鍋の中はPcursorをターゲットにしている
                target = C3_script.PCS_List[C3_script.Pcursor].transform;
                if (!flg)
                {
                    IconSize.x = 390;
                    IconSize.y = 170;
                    GetComponent<RectTransform>().sizeDelta = IconSize;
                    rect_tra.Translate(new Vector3(-0.917f, -0.815f, -0.361f));
                    flg = true;
                }
            }
            else
            {
                target = C3_script.Cursor_List[C3_script.cursor].transform;
                if (flg)
                {
                    IconSize.x = 640;
                    IconSize.y = 420;
                    GetComponent<RectTransform>().sizeDelta = IconSize;
                    rect_tra.Translate(new Vector3(+0.917f, +0.815f, +0.361f));
                    flg = false;
                }
                // Saraを選択したときにカーソルが隠れないように位置調整している
                if (C3_script.cursor == 14 || C3_script.cursor == 15)
                {
                    rect_tra.Translate(new Vector3(-0.917f, -0.815f, -0.361f));
                    flg = true;
                }
                
                this.transform.parent.GetComponent<RectTransform>().anchoredPosition = CanvasPos;
            }
        }
    }

    void ScalOperation()
    {// UIの拡大縮小をしている
        if (count <= Count_return/2)
        {
            rectTransform.localScale += new Vector3(Scale_val, Scale_val, Scale_val);
            count += 1;
        }
        else if (count <= Count_return)
        {
            rectTransform.localScale -= new Vector3(Scale_val, Scale_val, Scale_val);
            count += 1;
            if (count == Count_return) count = 0;
        }
    }
}