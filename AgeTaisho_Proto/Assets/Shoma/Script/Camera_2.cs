using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Camera_2 : MonoBehaviour
{
    /*   0   = ゴミ箱の座標
     *  1~13 = 食材や鍋などの座標
     * 14.15 = 14は揚げ物側の皿、15は天ぷら側の皿座標
     * 16.17 = 16は揚げ物側のストック、17は天ぷら側のストック座標*/
    [SerializeField] List<GameObject>Cursor_List = new List<GameObject>();

    private int cursor = 7; // カーソル用
    private GameObject cs_target_M; 

    float speed = 240f; 
    Quaternion target;      // 目的地の座標変数
    private bool gomi_flg = false; // ゴミ箱を向くときに使う
    public bool space_flg = false;
    private const float SPEED = 240; // ここをいじれば移動スピードが変わる！
    [SerializeField] GameObject ClickObj;

    void Start()
    {

        /***最初に正面を向くための処理***************/
        cs_target_M = Cursor_List[7];
        var aim = this.cs_target_M.transform.position - this.transform.position;
        var look = Quaternion.LookRotation(aim);
        this.transform.localRotation = look;
        /*******************************************/


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

        // 目的地に着くとはフラグを立てる
        if (transform.rotation != target) space_flg = false;
        else space_flg = true;

    }


    void DownKeyCheck()
    {
        // ←押したとき
        if (Input.GetKey(KeyCode.LeftArrow))
        {

            if (gomi_flg && (cs_target_M != Cursor_List[14] && cs_target_M != Cursor_List[15])) cursor = 9;
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
            cs_target_M = Cursor_List[cursor];
        }

        // →押したとき
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (gomi_flg && (cs_target_M != Cursor_List[14] && cs_target_M != Cursor_List[15])) cursor = 5;
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
            cs_target_M = Cursor_List[cursor];
        }

        // ↓押したとき
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            /* ゴミフラグがたっている時、
             * 焦げアイテムを持っているときに下を押すした時、
             * 3つの客席を見ている時は、すぐにゴミ箱を向く*/
            if (gomi_flg || (cursor >= 6 && cursor <= 8) || (ClickObj.transform.childCount > 0 && ClickObj.transform.GetChild(0).name == "ItemKoge"))
            {
                speed = SPEED * 2; // ゴミ箱を見る速さ
                cs_target_M = Cursor_List[0];
                gomi_flg = true;
            }
            else
            {
                Debug.Log("うああああああああああ");
                // ストックを見ている時に↓を押したらストックの直前の場所を向く
                if (cs_target_M == Cursor_List[16] || cs_target_M == Cursor_List[17]) cs_target_M = Cursor_List[cursor];
                else
                {
                    //お皿をもりつける場所を見る(揚げ物側)
                    if (cursor >= 9 && cursor <= 13) cs_target_M = Cursor_List[14];
                    //お皿をもりつける場所を見る(油もの側)
                    if (cursor >= 1 && cursor <= 5) cs_target_M = Cursor_List[15];
                    gomi_flg = true;
                }
            }
            
        }

        // ↑押したとき
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            // 盛り付け場を見ている時に↑を押したらストックの直前の場所を向く
            if (cs_target_M == Cursor_List[14] || cs_target_M == Cursor_List[15]) cs_target_M = Cursor_List[cursor];
            // ゴミ箱を見てるときに↑を押すと、真ん中のテーブルを向く
            else if (cs_target_M == Cursor_List[0])
            {
                cursor = 7;
                cs_target_M = Cursor_List[cursor];
            }
            else
            {
                //ストックする場所を見る(揚げ物側)
                if (cursor >= 9 && cursor <= 13) cs_target_M = Cursor_List[16];

                //ストックする場所を見る(油もの側)
                if (cursor >= 1 && cursor <= 5) cs_target_M = Cursor_List[17];
            }

            gomi_flg = false;
        }

    }

}