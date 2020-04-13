/*****************************************************
**カメラを三方向で固定しカーソルでの選択をさせている
******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Camera_3 : MonoBehaviour
{
    /*   0   = ゴミ箱の座標
     *  1~13 = 食材や鍋などの座標
     * 14.15 = 14は揚げ物側の皿、15は天ぷら側の皿座標
     * 16.17 = 16は揚げ物側のストック、17は天ぷら側のストック座標*/
    public List<GameObject> Cursor_List = new List<GameObject>();

    private int cursor = 7; // カーソル用
    private int tmp_cursor = 0; // 一時的に保存する用
    private GameObject cs_target_M;

    float speed = 240f;
    Quaternion target;      // 目的地の座標変数
    private bool gomi_flg = false;   // ゴミ箱を向くときに使う
    public bool space_flg = false;
    private bool stock_flg = false;  // ストックを見ているときはフラグがたつ
    private const float SPEED = 240; // ここをいじれば移動スピードが変わる！
    [SerializeField] GameObject ClickObj;

    //ポーズ画面
    GameObject Pause;
    Pause_Botton_Script script;

    /***** カメラ座標関連 *****/
    // CP=CameraPosition
    // 0＝天ぷら側　1＝客側　2＝揚げ物側
    [SerializeField] List<GameObject> CP_List = new List<GameObject>();
    [SerializeField] GameObject LightObj; // スポットライトのObjを入れる変数


    void Start()
    {

        /***最初に正面を向くための処理***************/
        cs_target_M = CP_List[1];
        var aim = this.cs_target_M.transform.position - this.transform.position;
        var look = Quaternion.LookRotation(aim);
        this.transform.localRotation = look;
        /*******************************************/


        //ポーズ画面
        Pause = GameObject.Find("Main Camera");
        script = Pause.GetComponent<Pause_Botton_Script>();

        //Garbage_can.gameObject.SetActiveRecursively(false);
    }

    void Update()
    {
        if (script.PauseFlag)
        {
            return;
        }
        else
        {
            if (Input.anyKeyDown)
            {
                
                DownKeyCheck();
                MoveCamera(); // カメラを移動させる処理
                MoveLight(); // カーソルの移動についての処理
                
                Debug.Log("カーソル番号：" + cursor);

            }

            // 移動を滑らかにする処理
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, speed * Time.deltaTime);

            // 目的地に着くとはフラグを立てる
            if (transform.rotation != target) space_flg = false;
            else space_flg = true;
        }
    }


    void DownKeyCheck()
    {
        // ←押したとき
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // ゴミ箱を向いているときに←押すと、唐揚げの場所を向く
            if (gomi_flg && (cs_target_M != Cursor_List[14] && cs_target_M != Cursor_List[15]))
            {
                tmp_cursor = 9;
                cursor = 9;
            }
            else
            {
                // ストックを内で←を押した時の処理
                if (stock_flg)
                {
                    if (cursor + 1 != 22 && cursor + 1 != 19) cursor += 1;
                }
                else
                {
                    if (tmp_cursor != 0) cursor = tmp_cursor + 1;
                    else cursor += 1;
                    if (tmp_cursor == 0 && cursor > 13)
                    {
                        cursor = 1;
                        speed = SPEED + 180;
                    }
                    else
                    {
                        tmp_cursor = 0;
                        speed = SPEED;
                    }
                }
            }

            gomi_flg = false;
            cs_target_M = Cursor_List[cursor];
        }

        // →押したとき
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (gomi_flg && (cs_target_M != Cursor_List[14] && cs_target_M != Cursor_List[15]))
            {
                tmp_cursor = 5;
                cursor = 5;
            }
            else
            {
                if (stock_flg)
                {
                    if (cursor - 1 != 18 && cursor - 1 != 15) cursor -= 1;
                }
                else
                {
                    if (tmp_cursor != 0) cursor = tmp_cursor - 1;
                    else cursor -= 1;
                    if (tmp_cursor == 0 && cursor < 1)
                    {
                        cursor = 13;
                        speed = SPEED + 120;
                    }
                    else
                    {
                        tmp_cursor = 0;
                        speed = SPEED;
                    }
                }
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
                cursor = 0;
                cs_target_M = Cursor_List[cursor];
                gomi_flg = true;
            }
            else
            {
                // ストックを見ている時に↓を押したらストックの直前の場所を向く
                if (stock_flg)
                {
                    cursor = tmp_cursor;
                    cs_target_M = Cursor_List[cursor];
                    stock_flg = false;
                }
                else
                {
                    //お皿をもりつける場所を見る(油もの側)
                    if (cursor >= 1 && cursor <= 5)
                    {
                        tmp_cursor = cursor;
                        cursor = 15;
                        cs_target_M = Cursor_List[cursor];
                    }
                    //お皿をもりつける場所を見る(揚げ物側)
                    if (cursor >= 9 && cursor <= 13)
                    {
                        tmp_cursor = cursor;
                        cursor = 14;
                        cs_target_M = Cursor_List[cursor];
                    }

                    gomi_flg = true;
                }
            }
            
        }

        // ↑押したとき
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            // 盛り付け場を見ている時に↑を押したらストックの直前の場所を向く
            if (cs_target_M == Cursor_List[14] || cs_target_M == Cursor_List[15])
            {
                cursor = tmp_cursor;
                cs_target_M = Cursor_List[cursor];
            }
            // ゴミ箱を見てるときに↑を押すと、真ん中のテーブルを向く
            else if (gomi_flg)
            {
                tmp_cursor = 7;
                cursor = 7;
                cs_target_M = Cursor_List[cursor];
            }
            else
            {
                //ストックする場所を見る(油もの側)
                if (cursor >= 1 && cursor <= 5)
                {
                    tmp_cursor = cursor;
                    cursor = 20;
                    cs_target_M = Cursor_List[cursor];
                }
                //ストックする場所を見る(揚げ物側)
                if (cursor >= 9 && cursor <= 13)
                {
                    tmp_cursor = cursor;
                    cursor = 17;
                    cs_target_M = Cursor_List[cursor];
                }
                stock_flg = true;
            }

            gomi_flg = false;
        }

    }
    

    void MoveCamera()
    {
        // 三方向にカメラを固定
        if (cursor == 1 || cursor == 5)
        {
            var aim = this.CP_List[0].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }
        else if (cursor == 0 || cursor == 6 || cursor == 8)
        {
            var aim = this.CP_List[1].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }
        else if (cursor == 9 || cursor == 13)
        {
            var aim = this.CP_List[2].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }
    }


    void MoveLight()
    {
        Vector3 tmp = Cursor_List[cursor].transform.position;
        LightObj.transform.position = new Vector3(tmp.x, tmp.y + 1f, tmp.z + 0.1f);
    }
}