using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Camera_2 : MonoBehaviour
{


    private int cursor = 7; // カーソル用
    int old_cursor = 0;
    private GameObject cs_target_M;
    private GameObject cs_target_1;
    private GameObject cs_target_2;
    private GameObject cs_target_3;
    private GameObject cs_target_4;
    private GameObject cs_target_5;
    private GameObject cs_target_6;
    private GameObject cs_target_7;
    private GameObject cs_target_8;
    private GameObject cs_target_9;
    private GameObject cs_target_10;
    private GameObject cs_target_11;
    private GameObject cs_target_12;
    private GameObject cs_target_13;
    private GameObject cs_target_99; // ゴミ箱用番号

    private GameObject Garbage_can;
    private GameObject Tableware_1; //お皿をもりつける場所(揚げ物側)
    private GameObject Tableware_2; //お皿をもりつける場所(油もの側)
    private GameObject StockTabl_1; //ストック場所(揚げ物側)
    private GameObject StockTabl_2; //ストック場所(油もの側)

    float speed = 240f; 
    Quaternion target;      // 目的地の座標変数
    private bool gomi_flg = false; // ゴミ箱を向くときに使う
    public bool space_flg = false;
    private const float SPEED = 240; // ここをいじれば移動スピードが変わる！
    private GameObject ClickObj;

    void Start()
    {
        cs_target_1 = GameObject.Find("CS_1");
        cs_target_2 = GameObject.Find("CS_2");
        cs_target_3 = GameObject.Find("CS_3");
        cs_target_4 = GameObject.Find("CS_4");
        cs_target_5 = GameObject.Find("CS_5");
        cs_target_6 = GameObject.Find("CS_6");
        cs_target_7 = GameObject.Find("CS_7");
        cs_target_8 = GameObject.Find("CS_8");
        cs_target_9 = GameObject.Find("CS_9");
        cs_target_10 = GameObject.Find("CS_10");
        cs_target_11 = GameObject.Find("CS_11");
        cs_target_12 = GameObject.Find("CS_12");
        cs_target_13 = GameObject.Find("CS_13");
        cs_target_99 = GameObject.Find("CS_gomi");

        /***最初に正面を向くための処理***************/
        cs_target_M = cs_target_7;
        var aim = this.cs_target_M.transform.position - this.transform.position;
        var look = Quaternion.LookRotation(aim);
        this.transform.localRotation = look;
        /*******************************************/

        Tableware_1 = GameObject.Find("CS_sara1");
        Tableware_2 = GameObject.Find("CS_sara2");
        StockTabl_1 = GameObject.Find("CS_stock1");
        StockTabl_2 = GameObject.Find("CS_stock2");

        ClickObj = GameObject.Find("ControllerObjClick");
        //Garbage_can.gameObject.SetActiveRecursively(false);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            DownKeyCheck();
            Debug.Log("cs_target_Mの値" + cs_target_M);
            var aim = this.cs_target_M.transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }

        // 移動を滑らかにする処理
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, speed * Time.deltaTime);

        // 目的地に着くとはフラグを立てる
        if (transform.rotation != target) space_flg = false;
        else space_flg = true;

    }


    void DownKeyCheck()
    {
        // 左押したとき
        if (Input.GetKey(KeyCode.LeftArrow))
        {

            if (gomi_flg) cursor = 9;
            else
            {
                cursor += 1;
                if (cursor > 13)
                {
                    cursor = 1;
                    speed = SPEED + 120;
                }
                else speed = SPEED;
            }
            
            gomi_flg = false;
            CameraCursor();
        }

        // 右押したとき
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (gomi_flg) cursor = 5;
            else
            {
                cursor -= 1;
                if (cursor < 1)
                {
                    cursor = 13;
                    speed = SPEED + 120;
                }
                else speed = SPEED;
            }
            gomi_flg = false;
            CameraCursor();
        }

        // 下押したとき
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            /* ゴミフラグがたっている時、
             * 焦げアイテムを持っているときに下を押すした時、
             * 3つの客席を見ている時は、すぐにゴミ箱を向く*/
            if (gomi_flg || (cursor >= 6 && cursor <= 8) || (ClickObj.transform.childCount > 0 && ClickObj.transform.GetChild(0).name == "ItemKoge"))
            {
                speed = SPEED * 2; // ゴミ箱を見る速さ
                cs_target_M = cs_target_99;
                gomi_flg = true;
            }
            else
            {
                Debug.Log("うああああああああああ");
                // ストックを見ている時に下を押したらストックの直前の場所を向く
                if (cs_target_M == StockTabl_1 || cs_target_M == StockTabl_2) CameraCursor();
                else
                {
                    //お皿をもりつける場所を見る(揚げ物側)
                    if (cursor >= 9 && cursor <= 13) cs_target_M = Tableware_1;
                    //お皿をもりつける場所を見る(油もの側)
                    if (cursor >= 1 && cursor <= 5) cs_target_M = Tableware_2;
                    gomi_flg = true;
                }
            }
            
        }

        // 上押したとき
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            // 盛り付け場を見ている時に上を押したらストックの直前の場所を向く
            if (cs_target_M == Tableware_1 || cs_target_M == Tableware_2) CameraCursor();
            // ゴミ箱を見てるときに上を押すと、真ん中のテーブルを向く
            else if (cs_target_M == cs_target_99)
            {
                cursor = 7;
                CameraCursor();
            }
            else
            {

                //ストックする場所を見る(揚げ物側)
                if (cursor >= 9 && cursor <= 13) cs_target_M = StockTabl_1;

                //ストックする場所を見る(油もの側)
                if (cursor >= 1 && cursor <= 5) cs_target_M = StockTabl_2;
            }

            gomi_flg = false;
        }

    }

    void CameraCursor()
    {

        switch (cursor)
        {
            case 1:
                cs_target_M = cs_target_1;
                break;
            case 2:
                cs_target_M = cs_target_2;
                break;
            case 3:
                cs_target_M = cs_target_3;
                break;
            case 4:
                cs_target_M = cs_target_4;
                break;
            case 5:
                cs_target_M = cs_target_5;
                break;
            case 6:
                cs_target_M = cs_target_6;
                break;
            case 7:
                cs_target_M = cs_target_7;
                break;
            case 8:
                cs_target_M = cs_target_8;
                break;
            case 9:
                cs_target_M = cs_target_9;
                break;
            case 10:
                cs_target_M = cs_target_10;
                break;
            case 11:
                cs_target_M = cs_target_11;
                break;
            case 12:
                cs_target_M = cs_target_12;
                break;
            case 13:
                cs_target_M = cs_target_13;
                break;
        }

    }
}