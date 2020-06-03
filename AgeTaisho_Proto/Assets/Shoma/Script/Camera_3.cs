﻿/*****************************************************
**カメラを三方向で固定しカーソルでの選択をさせている
******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Camera_3 : MonoBehaviour
{
    /*------------------------------------------＜リスト＞--------------------------------------------------*/
    //   0   = ゴミ箱の座標
    // 1~13 = 食材や鍋などの座標
    // 14.15 = 14は揚げ物側の皿、15は天ぷら側の皿座標
    // 16.17 = 16は揚げ物側のストック、17は天ぷら側のストック座標
    public List<GameObject> Cursor_List = new List<GameObject>();
    public List<GameObject> PCS_List = new List<GameObject>(); // 鍋用のカーソル座標リスト
    // CP=CameraPosition　　0＝天ぷら側　1＝客側　2＝揚げ物側
    [SerializeField] List<GameObject> CP_List = new List<GameObject>(); // カメラの場所を入れるリスト
    /*----------------------------------------------------------------------------------------------------*/

    /*------------------------------------------＜フラグ＞--------------------------------------------------*/
    [System.NonSerialized] public bool gomi_flg = false;   // ゴミ箱を向くときに使う
    [System.NonSerialized] public bool stock_flg = false;  // ストックを見ているときはtrue
    [System.NonSerialized] public bool pot_flg = false; // 鍋を見るかのフラグ
    [System.NonSerialized] public bool space_flg = false; // 移動中にスペースキーが反応しないようにするフラグ
    bool potfast_flg = false; // 粉から鍋を選択すると左下が選択されるためのフラグ
    bool button_flg = false;  // ボタンが一回だけ押さるようにするフラグ
    bool AutSelect_flg = true; // 自動選択をするときにtrue
    bool AutPSelect_flg = true; // 自動選択をするときにtrue
    [System.NonSerialized] public bool StockEX_flg = false;  // ストックに物がある時はtrue
    [System.NonSerialized] public bool PotEX_flg = false;  // 鍋に物がある時はtrue  
    /*----------------------------------------------------------------------------------------------------*/

    int AutCount = 0;

    [System.NonSerialized] public int cursor = 7; // カーソル用
    int tmp_cursor = 0; // cusorを一時的に保存する用


    Quaternion target;      // 目的地の座標変数

    const float SPEED = 480f; // ここをいじれば移動スピードが変わる！
    [SerializeField] GameObject ClickObj;
    [SerializeField] GameObject CursorObj; // スポットライトのObjを入れる変数

    //ポーズ画面
    GameObject Pause;
    Pause_Botton_Script script;

    int esc_cursor;
    [System.NonSerialized] public int Pcursor = 0;
    float button_time = 0f; // 次のボタンが押せるまでのインターバルを計る変数

    /* HandControllerButton_Ryoma */
    public GameObject HCB;
    HandControllerButton_S2 HCBscript;

    int tmp_Pcursor = -1;
    bool potLight_Flg = false;
    string SceneName; // sceneの名前を記憶する変数
    Vector3 old_direction;

    [SerializeField] GameObject ScoreText; // 正面を向いた時だけスコアを出すようにする
    [SerializeField] GameObject StockText_T; // 天ぷら側向いてるときだけ、ストックを出す
    [SerializeField] GameObject StockText_A; // 揚げ物側向いてるときだけ、ストックを出す

    void Start()
    {
        /***最初に正面を向くための処理***************/
        var aim = this.CP_List[1].transform.position - this.transform.position;
        var look = Quaternion.LookRotation(aim);
        this.transform.localRotation = look;
        /*******************************************/

        //ポーズ画面
        Pause = GameObject.Find("Main Camera");
        script = Pause.GetComponent<Pause_Botton_Script>();

        // <HandControllerButton_S2>の変数を使えるようにしている
        HCBscript = HCB.GetComponent<HandControllerButton_S2>();
        old_direction = HCBscript.direction;

        // 矢印を表示させている
        Vector3 tmp = Cursor_List[cursor].transform.position;
        CursorObj.transform.position = new Vector3(tmp.x, tmp.y, tmp.z);

    }

    void Update()
    {
        if (button_flg) // 連続でボタンを押せないようにインターバルを設定
        {
            button_time += Time.deltaTime;
            if (button_time >= 0.1)
            {
                button_flg = false;
                button_time = 0;
            }
        }

        if (script.PauseFlag) return;
        else if (!HCBscript.MoveFlg) // スペースを離しているかの判定
        {
            if (!pot_flg)
            {
                AutPSelect_flg = true;
                DownKeyCheck(); // 押されたボタンの処理をする
                if (cursor == 1 || cursor == 13) pot_flg = true;
                //MoveLight();    // カーソルの移動についての処理
            }
            // 鍋を見ているときに４つの座標から選択できるようにしている
            if (pot_flg) PotSelect();
            if (!stock_flg) AutSelect_flg = true;

            MoveCamera(); // カメラを移動させる処理
            // 移動を滑らかにする処理
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, SPEED * Time.deltaTime);

            // ストック自動選択の処理
            if (StockEX_flg) AutoCursorSelect();

            // 目的地に着くとフラグを立てる
            if (transform.rotation != target) space_flg = false;
            else space_flg = true;
        }
    }

    /*----------------------------------------------------------------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------------------------------------------------------------*/

    void AutoCursorSelect()
    {
        // ストックから物を取るときの自動選択
        if (!HCBscript.HoldingFlg)
        {
            if (stock_flg && AutSelect_flg){
                if (HCBscript.TargetTag != "Item")
                {// その場に取れるものがなかった時
                    if (cursor <= 21 && cursor >= 19)// 油もの側
                    {
                        if (cursor != 19) cursor -= 1;
                        else // ストック全体に取れるものがなかった場合
                        {
                            cursor = 21;
                            AutSelect_flg = false;
                            StockEX_flg = false;
                        }
                    }
                    else if (cursor <= 18 && cursor >= 16)// 揚げ物側
                    {
                        if (cursor != 16) cursor -= 1;
                        else
                        {
                            cursor = 18;
                            AutSelect_flg = false;
                            StockEX_flg = false;
                        }
                    }
                }
                else AutSelect_flg = false;
            }
        }

        // ストックする時空いてる場所を自動選択
        if (HCBscript.HoldingFlg)
        {
            if (stock_flg && AutSelect_flg){
                if (HCBscript.TargetTag != "Stock")
                {
                    if (cursor <= 21 && cursor >= 19)
                    {
                        if (cursor != 19) cursor -= 1;
                        else
                        {
                            cursor = 21;
                            AutSelect_flg = false;
                        }
                    }
                    else if (cursor <= 18 && cursor >= 16)
                    {
                        if (cursor != 16) cursor -= 1;
                        else
                        {
                            cursor = 18;
                            AutSelect_flg = false;
                        }
                    }
                }
                else AutSelect_flg = false;
            }
        }

    } //AutoCursorSelect()

    void DownKeyCheck()
    {
        // ←押したとき
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (-1 == Input.GetAxisRaw("XBox_Pad_H") && !button_flg))
        {
            button_flg = true;
            // ゴミ箱を向いて←押したとき処理
            if (cursor == 22)// 油側のごみ箱
            {
                tmp_cursor = 7;
                cursor = 7;// 真ん中の客席を向く
            }
            else if (cursor == 23)// 揚げ物側のごみ箱
            {
                tmp_cursor = 12;
                cursor = 12;// うずらの液を向く
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
                    if (tmp_cursor == 13)
                    {
                        cursor = 13;
                        Pcursor = 4;
                        tmp_cursor = 0;
                    }
                    else tmp_cursor = 0;

                }
            }

            gomi_flg = false;
            MoveLight();
        }

        // →押したとき
        else if (Input.GetKeyDown(KeyCode.RightArrow) || (1 == Input.GetAxisRaw("XBox_Pad_H") && !button_flg))
        {
            button_flg = true;
            // ゴミ箱を向いて→押したとき処理
            if (cursor == 22)// 油側のごみ箱
            {
                tmp_cursor = 2;
                cursor = 2;// てんぷら粉を向く
            }
            else if (cursor == 23)// 揚げ物側のごみ箱
            {
                tmp_cursor = 7;
                cursor = 7;// 真ん中の客席を向く
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
                    if (tmp_cursor == 1)
                    {
                        cursor = 1;
                        Pcursor = 1;
                        tmp_cursor = 0;
                    }
                    else tmp_cursor = 0;

                }
            }
            gomi_flg = false;
            MoveLight();
        }

        // ↓押したとき
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (0 > Input.GetAxisRaw("XBox_Pad_V") && !button_flg))
        {
            button_flg = true;
             /* ・焦げアイテムを持っているときに下を押すした時、
              * ・盛り付け場を見ているときに下を押した時
              *　　ゴミ箱を向く  */
            if (cursor == 15 || (HCBscript.HoldingFlg && ClickObj.transform.GetChild(0).name == "ItemKoge"))
            {
                cursor = 22;
                gomi_flg = true;
            }
            else if (cursor == 14 || (HCBscript.HoldingFlg && ClickObj.transform.GetChild(0).name == "ItemKoge"))
            {
                cursor = 23;
                gomi_flg = true;
            }
            else
            {
                // ストックを見ている時に↓を押したらストックの直前の場所を向く
                if (stock_flg)
                {
                    cursor = tmp_cursor;
                    stock_flg = false;
                    AutSelect_flg = true;
                }
                else
                {
                    //お皿をもりつける場所を見る(油もの側)
                    if (cursor >= 1 && cursor <= 5)
                    {
                        tmp_cursor = cursor;
                        cursor = 15;
                    }
                    //お皿をもりつける場所を見る(揚げ物側)
                    else if (cursor >= 9 && cursor <= 13)
                    {
                        tmp_cursor = cursor;
                        cursor = 14;
                    }

                    gomi_flg = true;
                }
            }
            MoveLight();
        }

        // ↑押したとき
        else if (Input.GetKeyDown(KeyCode.UpArrow) || (0 < Input.GetAxisRaw("XBox_Pad_V") && !button_flg))
        {
            button_flg = true;
            // 盛り付け場を見ている時に↑を押したらストックの直前の場所を向く
            if (cursor == 14 || cursor == 15)
            {
                if (tmp_cursor == 1) cursor = 2; // 直前に鍋を見ていたら鍋の隣を向く
                else if (tmp_cursor == 13) cursor = 12;
                else cursor = tmp_cursor;
            }
            else if (cursor == 22)// 油側のごみ箱
                cursor = 15;
            else if (cursor == 23)// 揚げ物側のごみ箱
                cursor = 14;
            
            else
            {
                //ストックする場所を見る(油もの側)
                if (cursor >= 1 && cursor <= 5)
                {
                    tmp_cursor = cursor;
                    cursor = 21;
                }
                //ストックする場所を見る(揚げ物側)
                if (cursor >= 9 && cursor <= 13)
                {
                    tmp_cursor = cursor;
                    cursor = 18;
                }
                stock_flg = true;
            }
            gomi_flg = false;
            MoveLight();
        }

    }//DownKeyCheck()



    void PotSelect()
    {
        if (cursor == 1)
        {
            if (potfast_flg == false)
            {
                Pcursor = 0;
                potfast_flg = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || (-1 == Input.GetAxisRaw("XBox_Pad_H") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 0 && Pcursor != 2) Pcursor -= 1;
                else
                {
                    potfast_flg = false;
                    pot_flg = false;
                    cursor = 2;
                }
                MoveLight();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || (1 == Input.GetAxisRaw("XBox_Pad_H") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 1 && Pcursor != 3) Pcursor += 1;
                MoveLight();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || (0 > Input.GetAxisRaw("XBox_Pad_V") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 0 && Pcursor != 1) Pcursor -= 2;
                else
                {
                    gomi_flg = true;
                    potfast_flg = false;
                    pot_flg = false;
                    tmp_cursor = cursor;
                    cursor = 15;
                }
                MoveLight();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || (0 < Input.GetAxisRaw("XBox_Pad_V") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 2 && Pcursor != 3) Pcursor += 2;
                MoveLight();
            }
        }

        else if (cursor == 13)
        {
            if (potfast_flg == false)
            {
                Pcursor = 4;
                potfast_flg = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || (0 > Input.GetAxisRaw("XBox_Pad_H") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 5 && Pcursor != 7) Pcursor += 1;
                MoveLight();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || (0 < Input.GetAxisRaw("XBox_Pad_H") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 4 && Pcursor != 6) Pcursor -= 1;
                else
                {
                    potfast_flg = false;
                    pot_flg = false;
                    cursor = 12;
                }
                MoveLight();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || (0 > Input.GetAxisRaw("XBox_Pad_V") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 4 && Pcursor != 5) Pcursor -= 2;
                else
                {
                    gomi_flg = true;
                    potfast_flg = false;
                    pot_flg = false;
                    tmp_cursor = cursor;
                    cursor = 14;
                }
                MoveLight();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || (0 < Input.GetAxisRaw("XBox_Pad_V") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 6 && Pcursor != 7) Pcursor += 2;
                MoveLight();
            }

        }

        //Tag=Powderを持っている && 見ているところに食材があったら
        if (HCBscript.ItemPowder && HCBscript.TargetTag == "Item")
        {
            if (HCBscript.TargetTag != "Item")// おける場所が見つかった時
                tmp_Pcursor = -1;
            else
            {
                if (tmp_Pcursor == -1) tmp_Pcursor = Pcursor;
                Pcursor += 1;
            }

            if (cursor == 1 && Pcursor == 4) Pcursor = 0;
            else if (cursor == 13 && Pcursor == 8) Pcursor = 5;
            if (tmp_Pcursor == Pcursor)
            { // 置ける場所がなかった時
                cursor = 2;
                Pcursor = -1;
                potfast_flg = false;
                pot_flg = false;
            }
            MoveLight();
        }
        else if (PotEX_flg && AutPSelect_flg &&
                !HCBscript.HoldingFlg && HCBscript.TargetTag != "Item")
        {
            Pcursor += 1;

            if (cursor == 1 && Pcursor == 4)
            {
                cursor = 1;
                Pcursor = 0;
                AutPSelect_flg = false;
                PotEX_flg = false;
            }
            else if (cursor == 13 && Pcursor == 8)
            {
                cursor = 13;
                Pcursor = 4;
                AutPSelect_flg = false;
                PotEX_flg = false;
            }
            MoveLight();
        }
        else
        {
            AutPSelect_flg = false;
            MoveLight();
        }

    }//PotSelect()


    void MoveCamera()
    {// 三方向にカメラを固定する処理

        /* 天ぷら側 */
        if (cursor >= 1 && cursor <= 5 || cursor == 15)
        {
            var aim = this.CP_List[0].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
            ScoreText.gameObject.SetActive(false);
            StockText_T.gameObject.SetActive(true);
        }
        /* お客側 */
        else if (cursor >= 6 && cursor <= 8)
        {
            var aim = this.CP_List[1].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
            ScoreText.gameObject.SetActive(true);
            StockText_T.gameObject.SetActive(false);
            StockText_A.gameObject.SetActive(false);
        }
        /* 揚げ物側 */
        else if (cursor >= 9 && cursor <= 13 || cursor == 14)
        {
            var aim = this.CP_List[2].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
            ScoreText.gameObject.SetActive(false);
            StockText_A.gameObject.SetActive(true);
        }
        /* 油もの側のゴミ箱 */
        if (cursor == 22)
        {
            var aim = this.CP_List[3].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }
        /* 揚げ物側ゴミ箱側 */
        if (cursor == 23)
        {
            var aim = this.CP_List[4].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }


        // CP_Listごとで移動する処理
        // 左へカメラごとの移動
        if (Input.GetKeyDown("a") || Input.GetButtonDown("PS4_L1"))
        {
            /* 天ぷら側～お客側へ */
            if ((cursor >= 1 && cursor <= 5) || cursor == 15 || (cursor >= 19 && cursor <= 21) || cursor == 22)
            {
                var aim = this.CP_List[1].transform.position - this.transform.position;
                var look = Quaternion.LookRotation(aim);
                target = look; // 目的座標を保存
                //old_direction = HCBscript.direction;
                cursor = 7;
                tmp_cursor = 0;
                potfast_flg = false;
                gomi_flg = false;
                stock_flg = false;
                pot_flg = false;

            }
            /* お客側～揚げ物側へ */
            else if (cursor == 0 || (cursor >= 6 && cursor <= 8))
            {
                var aim = this.CP_List[2].transform.position - this.transform.position;
                var look = Quaternion.LookRotation(aim);
                target = look; // 目的座標を保存
                //old_direction = HCBscript.direction;
                cursor = 11;
                tmp_cursor = 0;
                gomi_flg = false;
            }
            MoveLight();
        }

        // 右へカメラごとの移動
        else if (Input.GetKeyDown("d") || Input.GetButtonDown("PS4_R1"))
        {
            /* お客側～油もの側へ */
            if (cursor == 0 || (cursor >= 6 && cursor <= 8))
            {
                var aim = this.CP_List[0].transform.position - this.transform.position;
                var look = Quaternion.LookRotation(aim);
                target = look; // 目的座標を保存
                //old_direction = HCBscript.direction;
                cursor = 3;
                tmp_cursor = 0;
                gomi_flg = false;
            }
            /* 揚げ物側～お客側へ */
            else if ((cursor >= 9 && cursor <= 13) || cursor == 14 || (cursor >= 16 && cursor <= 18) || cursor == 23)
            {
                var aim = this.CP_List[1].transform.position - this.transform.position;
                var look = Quaternion.LookRotation(aim);
                target = look; // 目的座標を保存
                //old_direction = HCBscript.direction;
                cursor = 7;
                tmp_cursor = 0;
                potfast_flg = false;
                gomi_flg = false;
                stock_flg = false;
                pot_flg = false;
            }
            MoveLight();
        }
    }//MoveCamera()


    void MoveLight()
    {
        //if (cursor == 1 || cursor == 13)
        //{
        //    Vector3 tmp = PCS_List[Pcursor].transform.position;
        //    CursorObj.transform.position = new Vector3(tmp.x, tmp.y, tmp.z);
        //}
        //else
        //{
        //    Vector3 tmp = Cursor_List[cursor].transform.position;
        //    CursorObj.transform.position = new Vector3(tmp.x, tmp.y, tmp.z);
        //}
    }

}
