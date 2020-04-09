using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_HandControllerButton_S2 : MonoBehaviour {
    //このスクリプトはControllerMouseClickと共存しない
    //このスクリプトはhandにいれる

    public GameObject clickedGameObject;
    public GameObject Resource;
    private GameObject ClickObj;
    float handspeed = 0.1f;

    public bool HoldingFlg;
    //ColliderFlagの説明
    /* 0はEbiBox
     * 1はChickenBox
     * 2FishBox
     * 3PotatoBox
     * 4QuailBox*/
    public int ColliderFlag;
    public Transform ClickObj2;
    private Transform TmpFood; // 手に持っている物を入れる
    private GameObject TmpObj; // レイで当たっているObjを入れる

    GameObject C2;    // Camera_2を入れる変数
    Camera_2 C2_script; // Camera_2のscriptを入れる変数

    //ポーズ画面
    GameObject Pause;
    Pause_Botton_Script script;

    bool ItemSara;  //アイテム名にSaraが含まれているか判定

    void Start() {
        ClickObj = GameObject.Find("ControllerObjClick");
        HoldingFlg = false;
        ColliderFlag = 0;

        C2 = GameObject.Find("Main Camera");
        C2_script = C2.GetComponent<Camera_2>();
        //ポーズ画面
        Pause = GameObject.Find("Main Camera");
        script = Pause.GetComponent<Pause_Botton_Script>();
    }

    void Update() {

        if (script.PauseFlag) {
            return;
        }
        else {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);



            if (Physics.Raycast(ray.origin, ray.direction, out hit, 5f)) {
                // フラグがたっていないとボタンが聞かない
                if (Input.GetKeyDown(KeyCode.Space) && C2_script.space_flg) {
                    if (HoldingFlg != true) // 手に何も持っていない時に入る
                    {
                        if (hit.collider.gameObject.tag == "Box") {
                            switch (hit.collider.gameObject.name) {
                                case "EbiBox":
                                    Resource = (GameObject)Resources.Load("S_Resources/Tutorial_ItemEbi");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ColliderFlag = 0;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    break;

                            }
                            ItemSara = hit.collider.gameObject.name.Contains("Sara");
                        }

                        if (hit.collider.gameObject.tag == "Item") {
                            clickedGameObject = hit.collider.gameObject;                              //タグがなければオブジェクトをclickedGameObjectにいれる
                            clickedGameObject.transform.position = ClickObj.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
                            HoldingFlg = true;
                            ItemSara = hit.collider.gameObject.name.Contains("Sara");

                            //当たり判定をを外す
                            ColliderOut();
                        }

                        //ClickObj2.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = false;
                    }
                    else if ((ItemSara && (hit.collider.gameObject.tag == "Stock" || hit.collider.gameObject.tag == "Seki" || hit.collider.gameObject.tag == "Garbage can")) ||
                            (ItemSara == false && hit.collider.gameObject.tag != "Item" && hit.collider.gameObject.tag != "Box" &&
                            hit.collider.gameObject.tag != "Stock"))

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
    void ColliderOut() {
        clickedGameObject.GetComponent<Collider>().enabled = false;
    }

    //当たり判定を入れる関数
    void ColliderIn() {
        clickedGameObject.GetComponent<Collider>().enabled = true;
    }
}