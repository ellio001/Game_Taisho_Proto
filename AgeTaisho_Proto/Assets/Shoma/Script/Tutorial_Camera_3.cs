/*****************************************************
**カメラを三方向で固定しカーソルでの選択をさせている
******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Tutorial_Camera_3 : MonoBehaviour
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
    bool gomi_flg = false;   // ゴミ箱を向くときに使う
    bool stock_flg = false;  // ストックを見ているときはフラグがたつ
    public bool pot_flg = false; // 鍋を見るかのフラグ
    [System.NonSerialized] public bool space_flg = false; // 移動中にスペースキーが反応しないようにするフラグ
    bool potfast_flg = false; // 粉から鍋を選択すると左下が選択されるためのフラグ
    bool button_flg = false;  // ボタンが一回だけ押さるようにするフラグ
    /*----------------------------------------------------------------------------------------------------*/

    [System.NonSerialized] public int cursor = 7; // カーソル用
    int tmp_cursor = 0; // cusorを一時的に保存する用


    Quaternion target;      // 目的地の座標変数

    const float SPEED = 420f; // ここをいじれば移動スピードが変わる！
    [SerializeField] GameObject ClickObj;
    [SerializeField] GameObject LightObj; // スポットライトのObjを入れる変数

    //ポーズ画面
    GameObject Pause;
    Pause_Botton_Script script;

    int esc_cursor;
    [System.NonSerialized] public int Pcursor = 0;
    float button_time = 0f; // 次のボタンが押せるまでのインターバルを計る変数

    public GameObject HCB;
    Tutorials_HandControllerButton THCBscript;

    int tmp_Pcursor = -1;
    bool potLight_Flg = false;
    string SceneName; // sceneの名前を記憶する変数
    Vector3 old_direction;

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

        THCBscript = HCB.GetComponent<Tutorials_HandControllerButton>();
        old_direction = THCBscript.direction;

        // 矢印を表示させている
        Vector3 tmp = Cursor_List[cursor].transform.position;
        LightObj.transform.position = new Vector3(tmp.x, tmp.y /*+ 0.2f*/, tmp.z);
    }

    void Update()
    {
        if (button_flg)
        {
            button_time += Time.deltaTime;
            if (button_time >= 0.2)
            {
                button_flg = false;
                button_time = 0;
            }
        }

        if (script.PauseFlag) return;
        else if (!THCBscript.MoveFlg) // スペースを離しているかの判定
        {
            if (pot_flg == false)
            {
                DownKeyCheck(); // 押されたボタンの処理をする
                if (cursor == 1 || cursor == 13) pot_flg = true;
                //MoveLight();    // カーソルの移動についての処理
            }
            if (pot_flg) PotSelect();

            MoveCamera(); // カメラを移動させる処理
            // 移動を滑らかにする処理
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, SPEED * Time.deltaTime);

            //// カーソル移動したときのアウトラインのオンオフを設定
            //if (old_direction != THCBscript.direction)
            //    THCBscript.TargetObj.GetComponent<Outline>().enabled = true;
            //else
            //    THCBscript.TargetObj.GetComponent<Outline>().enabled = false;

            // 目的地に着くとフラグを立てる
            if (transform.rotation != target) space_flg = false;
            else space_flg = true;
        }
    }



    void DownKeyCheck()
    {
        // ←押したとき
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (-1 == Input.GetAxisRaw("XBox_Pad_H") && !button_flg))
        {
            //old_direction = THCBscript.direction;
            button_flg = true;
            // ゴミ箱を向いているときに←押すと、唐揚げの場所を向く
            if (gomi_flg && (cursor != 14 && cursor != 15))
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
                    if (tmp_cursor == 13)
                    {
                        cursor = 13;
                        Pcursor = 4;
                        tmp_cursor = 0;
                    }
                    if (tmp_cursor == 0 && cursor > 13)
                    {
                        //cursor = 1;
                        //pot_flg = true;
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
            //old_direction = THCBscript.direction;
            button_flg = true;
            if (gomi_flg && (cursor != 14 && cursor != 15))
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
                    if (tmp_cursor == 1)
                    {
                        cursor = 1;
                        Pcursor = 1;
                        tmp_cursor = 0;
                    }
                    if (tmp_cursor == 0 && cursor < 1)
                    {
                        //cursor = 13;
                        //pot_flg = true;
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
            //old_direction = THCBscript.direction;
            button_flg = true;
            /* ゴミフラグがたっている時、
             * 焦げアイテムを持っているときに下を押すした時、
             * 3つの客席を見ている時は、すぐにゴミ箱を向く*/
            if (gomi_flg || (cursor >= 6 && cursor <= 8) || (ClickObj.transform.childCount > 0 && ClickObj.transform.GetChild(0).name == "ItemKoge"))
            {
                cursor = 0;
                gomi_flg = true;
            }
            else
            {
                // ストックを見ている時に↓を押したらストックの直前の場所を向く
                if (stock_flg)
                {
                    cursor = tmp_cursor;
                    stock_flg = false;
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
            //old_direction = THCBscript.direction;
            button_flg = true;
            // 盛り付け場を見ている時に↑を押したらストックの直前の場所を向く
            if (cursor == 14 || cursor == 15)
            {
                cursor = tmp_cursor;
            }
            // ゴミ箱を見てるときに↑を押すと、真ん中のテーブルを向く
            else if (gomi_flg)
            {
                tmp_cursor = 7;
                cursor = 7;
            }
            else
            {
                //ストックする場所を見る(油もの側)
                if (cursor >= 1 && cursor <= 5)
                {
                    tmp_cursor = cursor;
                    cursor = 20;
                }
                //ストックする場所を見る(揚げ物側)
                if (cursor >= 9 && cursor <= 13)
                {
                    tmp_cursor = cursor;
                    cursor = 17;
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
                //else 盛り付け場から鍋に行く際のバグ解消のため
                //{
                //    potfast_flg = false;
                //    cursor = 13;
                //    Pcursor = 5;
                //}
                MoveLight();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || (0 > Input.GetAxisRaw("XBox_Pad_V") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 0 && Pcursor != 1) Pcursor -= 2;
                else
                {
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
                //else 鍋からストックに行けないようにコメントしている
                //{
                //    potfast_flg = false;
                //    pot_flg = false;
                //    stock_flg = true;
                //    tmp_cursor = cursor;
                //    cursor = 19;
                //}
                MoveLight();
            }
        }

        else if (cursor == 13)
        {
            if (potfast_flg == false)
            {
                Pcursor = 3;
                potfast_flg = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || (0 > Input.GetAxisRaw("XBox_Pad_H") && !button_flg))
            {
                button_flg = true;
                if (Pcursor != 5 && Pcursor != 7) Pcursor += 1;
                //else
                //{
                //    potfast_flg = false;
                //    cursor = 1;
                //    Pcursor = 1;
                //}
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
                //else
                //{
                //    potfast_flg = false;
                //    pot_flg = false;
                //    stock_flg = true;
                //    tmp_cursor = cursor;
                //    cursor = 18;
                //}
                MoveLight();
            }

        }

        //Tag=Powderを持っている && 見ているところに食材があったら
        if (THCBscript.ItemPowder && THCBscript.TargetTag == "Item")
        {
            if (THCBscript.TargetTag != "Item")// おける場所が見つかった時
            {
                tmp_Pcursor = -1;
                potLight_Flg = true;
            }
            else
            {
                if (tmp_Pcursor == -1) tmp_Pcursor = Pcursor;
                Pcursor += 1;
            }

            if (cursor == 1 && Pcursor == 4) Pcursor = 0;
            else if (cursor == 13 && Pcursor == 8) Pcursor = 5;
            if (tmp_Pcursor == Pcursor)
            {
                cursor = 2;
                Pcursor = -1;
                potfast_flg = false;
                pot_flg = false;
            }
            MoveLight();
        }

        //if (pot_flg)
        //{
        //    Vector3 tmp = PCS_List[Pcursor].transform.position;
        //    LightObj.transform.position = new Vector3(tmp.x, tmp.y + 1f, tmp.z);
        //}
        //else
        //{
        //    Vector3 tmp = Cursor_List[cursor].transform.position;
        //    LightObj.transform.position = new Vector3(tmp.x, tmp.y + 1f, tmp.z);
        //}


    }//PotSelect()


    void MoveCamera()
    {// 三方向にカメラを固定する処理

        /* ゴミ箱側 */
        if (cursor == 0)
        {
            var aim = this.CP_List[3].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }
        /* 天ぷら側 */
        if (cursor >= 1 && cursor <= 5)
        {
            var aim = this.CP_List[0].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }
        /* お客側 */
        else if (cursor >= 6 && cursor <= 8)
        {
            var aim = this.CP_List[1].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }
        /* 揚げ物側 */
        else if (cursor >= 9 && cursor <= 13)
        {
            var aim = this.CP_List[2].transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            target = look; // 目的座標を保存
        }

        // CP_Listごとで移動する処理
        // 左へカメラごとの移動
        if (Input.GetKeyDown("a") || Input.GetButtonDown("PS4_L1"))
        {
            /* 天ぷら側 */
            if ((cursor >= 1 && cursor <= 5) || cursor == 15 || (cursor >= 19 && cursor <= 21))
            {
                var aim = this.CP_List[1].transform.position - this.transform.position;
                var look = Quaternion.LookRotation(aim);
                target = look; // 目的座標を保存
                //old_direction = HCBscript.direction;
                cursor = 7;
                tmp_cursor = 0;
                pot_flg = false;
                potfast_flg = false;
            }
            /* お客側 */
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
            /* お客側 */
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
            /* 揚げ物側 */
            else if ((cursor >= 9 && cursor <= 13) || cursor == 14 || (cursor >= 16 && cursor <= 18))
            {
                var aim = this.CP_List[1].transform.position - this.transform.position;
                var look = Quaternion.LookRotation(aim);
                target = look; // 目的座標を保存
                //old_direction = HCBscript.direction;
                cursor = 7;
                tmp_cursor = 0;
                pot_flg = false;
                potfast_flg = false;
            }
            MoveLight();
        }

    }//MoveCamera()


    void MoveLight()
    {
        if (cursor == 1 || cursor == 13)
        {
            Vector3 tmp = PCS_List[Pcursor].transform.position;
            LightObj.transform.position = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z);
        }
        else
        {
            Vector3 tmp = Cursor_List[cursor].transform.position;
            LightObj.transform.position = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z);
        }
    }

}