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

    float speed = 120f; // 移動の際のスピード
    Quaternion target; // 目的地の座標変数
    private bool gomi_flg = false; // ゴミ箱を向くときに使う

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

        Garbage_can = GameObject.Find("Garbage can");
        //Garbage_can.gameObject.SetActiveRecursively(false);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            DownKeyCheck();

            var aim = this.cs_target_M.transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }

        // 移動を滑らかにする処理
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, speed * Time.deltaTime);

    }


    void DownKeyCheck()
    {
        // 左押したとき
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Garbage_can.gameObject.SetActiveRecursively(false); // ゴミ箱非表示
            cursor += 1;
            if (cursor > 13)
            {
                cursor = 1;
                speed = 240f;
            }
            else speed = 120f;
            gomi_flg = false;
            CameraCursor();
        }

        // 右押したとき
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // Garbage_can.gameObject.SetActiveRecursively(false);
            cursor -= 1;
            if (cursor < 1)
            {
                cursor = 13;
                speed = 240f;
            }
            else speed = 120f;
            gomi_flg = false;
            CameraCursor();
        }

        // 下押したとき
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (gomi_flg) cs_target_M = cs_target_99;
            else
            {
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
            //Garbage_can.gameObject.SetActiveRecursively(false);
            // 盛り付け場を見ている時に上を押したらストックの直前の場所を向く
            if (cs_target_M == Tableware_1 || cs_target_M == Tableware_2) CameraCursor();

            else
            {
                //ストックする場所を見る(揚げ物側)
                if (cursor >= 9 && cursor <= 13) cs_target_M = StockTabl_1;

                //ストックする場所を見る(油もの側)
                if (cursor >= 1 && cursor <= 5) cs_target_M = StockTabl_2;
            }
            gomi_flg = false;
        }

        // ゴミ箱を出す
        else if (Input.GetKey(KeyCode.A))
        {

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