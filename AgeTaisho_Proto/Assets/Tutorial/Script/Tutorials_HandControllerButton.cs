﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials_HandControllerButton : MonoBehaviour
{
    //このスクリプトはControllerMouseClickと共存しない
    //このスクリプトはhandにいれる

    public GameObject clickedGameObject;
    public GameObject Resource;
    private GameObject ClickObj;
    float handspeed = 0.1f;

    public bool HoldingFlg;
    public Transform ClickObj2;

    GameObject C2;    // Camera_2を入れる変数
    Camera_2 C2_script; // Camera_2のscriptを入れる変数
    Camera_3 C3_script; // Camera_3のscriptを入れる変数

    [SerializeField] GameObject Player; // プレイヤーの位置を保存
    Vector3 Player_V;                   // プレイヤーの座標を保存する用
    Vector3 direction; // Rayの終点座標

    //ポーズ画面
    GameObject Pause;
    Pause_Botton_Script script;

    public TutorialUI tutorialUI;
    int TextNumber;

    /***** 矢印関連 *****/
    [SerializeField] GameObject ArrowObj; // 矢印のObjを入れる変数
    bool ArrowFlg = false;                // 矢印が今出ているかの確認用
    bool DestroyFlg = false;              // 矢印を消すか判断する用
    Vector3 tmp;                          // カーソルの座標を仮に保存

    void Start()
    {
        ClickObj = GameObject.Find("ControllerObjClick");
        HoldingFlg = false;

        C2 = GameObject.Find("Main Camera");
        C2_script = C2.GetComponent<Camera_2>();
        C3_script = C2.GetComponent<Camera_3>();

        // プレイヤーの座標をVector3に変換
        Player_V.x = Player.transform.position.x;
        Player_V.y = Player.transform.position.y;
        Player_V.z = Player.transform.position.z;
        direction = C3_script.Cursor_List[C3_script.cursor].transform.position;

        //ポーズ画面
        Pause = GameObject.Find("Main Camera");
        script = Pause.GetComponent<Pause_Botton_Script>();
    }

    void Update()
    {
        if (script.PauseFlag) {
            return;
        }
        else {
            TextNumber = tutorialUI.TextNumber; // TextNumberの値を常に更新

            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 今選択しているカーソルの位置を代入している
            if (C3_script.pot_flg)
            {
                direction = C3_script.PCS_List[C3_script.Pcursor].transform.position;
            }
            else
            {
                direction = C3_script.Cursor_List[C3_script.cursor].transform.position;
            }

            if (TextNumber == 3 || TextNumber == 4 ||
                TextNumber == 5 || TextNumber == 7 ||
                TextNumber == 8 || TextNumber == 9) Move_arrow(); // 矢印を表示

            // 天ぷらが生成されたら次のテキストに進むTutorial_ItemTenpura
            if (GameObject.Find("Tutorial_ItemTenpura") && TextNumber == 6) tutorialUI.TextNumber = 7;

            if (Physics.Linecast(Player_V, direction, out hit)){
                Debug.DrawLine(Player_V, direction, Color.red);
                // フラグがたっていないとボタンが聞かな
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("〇")) && C3_script.space_flg)
                {

                    if (HoldingFlg != true) // 手に何も持っていない時に入る
                    {
                        if (hit.collider.gameObject.tag == "Box")
                        {
                            if (hit.collider.gameObject.name == "EbiBox" && TextNumber == 3)
                            {
                                Resource = (GameObject)Resources.Load("S_Resources/Tutorial_ItemEbi");   //Resourceフォルダのプレハブを読み込む
                                clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                HoldingFlg = true;
                                tutorialUI.TextNumber = 4; // テキストを進める
                                DestroyFlg = true; // 矢印を消すフラグを立てる

                                ColliderOut();//当たり判定をを外す
                            }
                        }

                        if (hit.collider.gameObject.tag == "Item" && (TextNumber != 6 && TextNumber != 9))
                        {
                            clickedGameObject = hit.collider.gameObject;                              //タグがなければオブジェクトをclickedGameObjectにいれる
                            clickedGameObject.transform.position = ClickObj.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
                            HoldingFlg = true;
                            //当たり判定をを外す
                            ColliderOut();
                        }

                    }
                    else if ((TextNumber == 4 && hit.collider.gameObject.tag == "kona") || (TextNumber == 5 && hit.collider.gameObject.tag == "tenpuranabe") ||
                             (TextNumber == 7 && hit.collider.gameObject.tag == "Sara") || (TextNumber == 8 && hit.collider.gameObject.name == "Plate2"))
                    // 粉や鍋にすでに食材があるなら食材を置けないようにしている(唐揚げは何個でも置ける)
                    {
                        //当たり判定を入れる
                        ColliderIn();
                        ClickObj2.GetChild(0).gameObject.transform.position = hit.point; // 見ているところに置く
                        clickedGameObject.transform.parent = null;              //手との親子付けを解除

                        clickedGameObject.GetComponent<Rigidbody>().isKinematic = false;    //重力を有効

                        clickedGameObject = null;   //対象を入れる箱を初期化
                        Resource = null;            //生成するプレハブの箱を初期化

                        HoldingFlg = false;
                        tutorialUI.TextNumber += 1; // テキストを進める
                        DestroyFlg = true;
                    }
                    if (clickedGameObject != null)  //nullでないとき処理
                    {
                        clickedGameObject.transform.parent = ClickObj.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
                        clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
                    }
                }
            }
        }
    }

    //当たり判定を切る関数
    void ColliderOut()
    {
        clickedGameObject.GetComponent<Collider>().enabled = false;
    }

    //当たり判定を入れる関数
    void ColliderIn()
    {
        clickedGameObject.GetComponent<Collider>().enabled = true;
    }

    void Move_arrow()
    {
        if (ArrowFlg == false && TextNumber != 9)
        {
            switch (TextNumber)
            {
                case 3:
                    tmp = C3_script.Cursor_List[3].transform.position;
                    break;
                case 4:
                    tmp = C3_script.Cursor_List[2].transform.position;
                    break;
                case 5:
                    tmp = C3_script.Cursor_List[1].transform.position;
                    break;
                case 7:
                    tmp = C3_script.Cursor_List[15].transform.position;
                    break;
                case 8:
                    tmp = C3_script.Cursor_List[6].transform.position;
                    break;
            }
            Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z + 0.1f), Quaternion.identity);
            ArrowFlg = true; // 矢印が表示中のフラグ
        }
        else if (DestroyFlg || TextNumber == 9)
        {
            Destroy(GameObject.Find("Yajirusi(Clone)"));
            DestroyFlg = false;
            ArrowFlg = false;
        }
    }

}




