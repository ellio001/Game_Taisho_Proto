using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_CusorUI : MonoBehaviour
{
    GameObject C3;
    Camera_3 C3_script;
    Vector2 CursorPos;

    //　アイコンのサイズ取得で使用
    private RectTransform rect;
    //　アイコンが画面内に収まる為のオフセット値
    private Vector2 offset;

    int count = 0;
    void Start()
    {
        rect = GetComponent<RectTransform>();
        //　オフセット値をアイコンのサイズの半分で設定
        offset = new Vector2(rect.sizeDelta.x / 2f, rect.sizeDelta.y / 2f);

        C3 = GameObject.Find("Main Camera");
        C3_script = C3.GetComponent<Camera_3>();
    }

    void Update()
    {
        CursorPos = C3_script.Cursor_List[C3_script.cursor].transform.position;

        //　移動先を計算
        //var pos = rect.anchoredPosition + new Vector2(CursorPos.x, CursorPos.y);

        var pos = new Vector2(CursorPos.x, CursorPos.y);
        Debug.Log("CursorPos.x:" + CursorPos.x);
        Debug.Log("CursorPos.y:" + CursorPos.y);

        
        //　アイコンが画面外に出ないようにする
        //pos.x = Mathf.Clamp(pos.x, -Screen.width * 0.5f + offset.x, Screen.width * 0.5f - offset.x);
        //pos.y = Mathf.Clamp(pos.y, -Screen.height * 0.5f + offset.y, Screen.height * 0.5f - offset.y);
        //　アイコン位置を設定
        rect.anchoredPosition = pos;


        //ScalOperation();
    }

    void ScalOperation()
    {// UIの拡大縮小をしている
        if (count <= 60)
        {
            rect.localScale += new Vector3(0.001f, 0.001f, 0.001f);
            count += 1;
        }
        else if (count <= 120)
        {
            rect.localScale -= new Vector3(0.001f, 0.001f, 0.001f);
            count += 1;
            if (count == 120) count = 0;
        }
    }
    //GameObject C3;
    //Camera_3 C3_script;

    //Vector2 CursorPos;

    //void Start()
    //{
    //    C3 = GameObject.Find("Main Camera");
    //    C3_script = C3.GetComponent<Camera_3>();
    //}

    //void Update()
    //{
    //    CursorPos = C3_script.Cursor_List[C3_script.cursor].transform.position;
    //    this.transform.position = new Vector2(CursorPos.x, CursorPos.y);

    //    //GetComponent<RectTransform>().anchoredPosition = new Vector2(CursorPos.x, CursorPos.y);
    //    //Debug.Log("CursorPos.x:" + CursorPos.x);
    //    //Debug.Log("CursorPos.y:" + CursorPos.y);
    //    Debug.Log("cursor座標:" + C3_script.Cursor_List[C3_script.cursor]);
    //}
}
