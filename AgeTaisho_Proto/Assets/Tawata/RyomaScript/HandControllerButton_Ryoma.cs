using UnityEngine;

public class HandControllerButton_Ryoma : MonoBehaviour {
    //このスクリプトはControllerMouseClickと共存しない
    //このスクリプトはhandにいれる

    GameObject clickedGameObject;
    GameObject Resource;
    GameObject ClickObj;
    float handspeed = 0.1f;

    [System.NonSerialized] public bool HoldingFlg;
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
    Control control_script; // Camera_3のscriptを入れる変数

    [SerializeField] GameObject Player; // プレイヤーの位置を保存
    Vector3 Player_V;                   // プレイヤーの座標を保存する用
    [System.NonSerialized] public Vector3 direction; // Rayの終点座標

    //ポーズ画面
    GameObject Pause;
    Pause_Botton_Script script;

    bool ItemSara;  //アイテム名にSaraが含まれているか判定
    bool KonaFlag = false; // 〇を押すと粉に漬け、離すと手元に戻るようにするフラグ
    [System.NonSerialized] public string TargetTag;//今見ているOBJのタグを保存する 
    [System.NonSerialized] public GameObject TargetObj;//今見ているOBJのタグを保存する 

    [System.NonSerialized] public bool ItemPowder; // 粉系を持っているかの判定フラグ
    [System.NonSerialized] public bool MoveFlg = false; // スペースを押している間は移動できないようにするフラグ

    void Start() {
        ClickObj = GameObject.Find("ControllerObjClick");
        HoldingFlg = false;
        ColliderFlag = 0;

        C2 = GameObject.Find("Main Camera");
        C2_script = C2.GetComponent<Camera_2>();
        control_script = C2.GetComponent<Control>();

        // プレイヤーの座標をVector3に変換
        Player_V.x = Player.transform.position.x;
        Player_V.y = Player.transform.position.y;
        Player_V.z = Player.transform.position.z;
        direction = control_script.Cursor_List[control_script.cursor].transform.position;

        //ポーズ画面
        Pause = GameObject.Find("Main Camera");
        script = Pause.GetComponent<Pause_Botton_Script>();
    }

    void Update() {
        //Debug.Log(KonaFlag);
        if (script.PauseFlag) {
            return;
        }
        else {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            // 今選択しているカーソルの位置を代入している
            if (control_script.pot_flg)
                direction = control_script.PCS_List[control_script.Pcursor].transform.position;
            else
                direction = control_script.Cursor_List[control_script.cursor].transform.position;



            // スペースを離したときにカーソル移動ができるようにしている
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("XBox_joystick_B")) MoveFlg = false;
            if (Physics.Linecast(Player_V, direction, out hit)) {
                Debug.DrawLine(Player_V, direction, Color.red);

                TargetTag = hit.collider.gameObject.tag; // 今見ているOBJのタグを保存
                //TargetObj = hit.collider.gameObject; // 今見ているOBJを保存(C3のアウトラインのオンオフで使う)

                // てんぷら粉、ウズラの液と粉、を選択中はフラグを立てる
                if (control_script.Cursor_List[control_script.cursor] == control_script.Cursor_List[2] ||
                    control_script.Cursor_List[control_script.cursor] == control_script.Cursor_List[11] ||
                    control_script.Cursor_List[control_script.cursor] == control_script.Cursor_List[12] ||
                    control_script.Cursor_List[control_script.cursor] == control_script.Cursor_List[14] ||
                    control_script.Cursor_List[control_script.cursor] == control_script.Cursor_List[15]) KonaFlag = true;
                else KonaFlag = false;

                // フラグがたっていないとボタンが効かない
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("XBox_joystick_B")) && control_script.space_flg) {
                    MoveFlg = true;
                    if (!HoldingFlg) // 手に何も持っていない時に入る
                    {
                        if (hit.collider.gameObject.tag == "Box") {
                            switch (hit.collider.gameObject.name) {
                                case "EbiBox":
                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Shrimp");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ColliderFlag = 0;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    break;
                                case "ChickenBox":
                                    Resource = (GameObject)Resources.Load("S_Resources/ItemChicken");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ItemPowder = true;
                                    ColliderFlag = 1;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    break;
                                case "FishBox":
                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Fish_v2");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ColliderFlag = 2;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    break;
                                case "PotatoBox":
                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Potato");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ColliderFlag = 3;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    break;
                                case "QuailBox":
                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Quail");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ColliderFlag = 4;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    break;

                            }
                            ItemSara = false;
                        }

                        if (hit.collider.gameObject.tag == "Item") {
                            clickedGameObject = hit.collider.gameObject;                              //タグがなければオブジェクトをclickedGameObjectにいれる
                            clickedGameObject.transform.position = ClickObj.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
                            HoldingFlg = true;

                            if (hit.collider.gameObject.name.Contains("Dish"))
                                ItemSara = hit.collider.gameObject.name.Contains("Dish");
                            else if (hit.collider.gameObject.name.Contains("Sara"))
                                ItemSara = hit.collider.gameObject.name.Contains("Sara"); // 後で消す
                            else ItemSara = false;

                            //当たり判定をを外す
                            ColliderOut();
                        }

                        //ClickObj2.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = false;
                    }
                    else if ((ItemSara && (hit.collider.gameObject.tag == "Stock" || hit.collider.gameObject.tag == "Seki" || hit.collider.gameObject.tag == "Garbage can")) ||
                            (!ItemSara && hit.collider.gameObject.tag != "Item" && hit.collider.gameObject.tag != "Box" && hit.collider.gameObject.tag != "Stock"))
                    // 粉や鍋にすでに食材があるなら食材を置けないようにしている(唐揚げは何個でも置ける)
                    {
                        ItemPowder = false; // 粉をつけたものを鍋に置いたときにFalse
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

                // 粉系か皿に置くときに、ボタンを離すと手元に戻ってくるようにしている
                if (KonaFlag && (hit.collider.gameObject.name.Contains("Dish") || hit.collider.gameObject.tag == "Item") &&
                    (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("XBox_joystick_B"))) {
                    if (hit.collider.gameObject.name.Contains("Dish")) ItemSara = true;
                    else if (hit.collider.gameObject.tag == "Item") ItemPowder = true;

                    clickedGameObject = hit.collider.gameObject;                              //タグがなければオブジェクトをclickedGameObjectにいれる
                    clickedGameObject.transform.position = ClickObj.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
                    HoldingFlg = true;

                    //当たり判定をを外す
                    ColliderOut();
                    clickedGameObject.transform.parent = ClickObj.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
                    clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
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