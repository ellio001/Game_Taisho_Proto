using System.Collections.Generic;
using UnityEngine;


public class HandControllerButton_S2 : MonoBehaviour {
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
    Camera_3 C3_script; // Camera_3のscriptを入れる変数

    [SerializeField] GameObject Player; // プレイヤーの位置を保存
    Vector3 Player_V;                   // プレイヤーの座標を保存する用
    [System.NonSerialized] public Vector3 direction; // Rayの終点座標

    //ポーズ画面
    GameObject Pause;
    Pause_Botton_Script script;

    bool ItemSara;  //アイテム名にSaraが含まれているか判定
    bool KonaFlag = false; // 〇を押すと粉に漬け、離すと手元に戻るようにするフラグ
    [System.NonSerialized] public string TargetTag;//今見ているOBJのタグを保存する 
    [System.NonSerialized] public GameObject TargetObj;//今見ているOBJを保存する 

    [System.NonSerialized] public bool ItemPowder; // 粉系を持っているかの判定フラグ
    [System.NonSerialized] public bool MoveFlg = false; // スペースを押している間は移動できないようにするフラグ

    /*------- 矢印関連 -------*/
    [SerializeField] GameObject ArrowObj; // 矢印のObjを入れる変数
    bool DestroyFlg = false;              // 矢印を消すか判断する用
    Vector3 tmp;                          // カーソルの座標を仮に保存
    [System.NonSerialized] public int AgeCount;//鍋でFrideになっている数をカウント（鍋に矢印を出すときに使う）
    bool NabeArrow_flg = false; // true = 鍋に矢印表示中
    GameObject GM;
    GameManager GMscript;
    int Shrimp_order;
    int Fish_order;
    int Potato_order;
    bool Offer_flg = false;
    bool tekitou_flg = false;

    // 席について注文を記憶する=true, 席から離れる=false
    bool Audience0_flg = false; // 0番目の席用
    bool Audience1_flg = false; // 1番目の席用
    bool Audience2_flg = false; // 2番目の席用
    [SerializeField] List<GameObject> Arrow_List = new List<GameObject>(); // カメラの場所を入れるリスト


    //オーディオ
    AudioSource sounds;

    void Start() {
        

        ClickObj = GameObject.Find("ControllerObjClick");
        HoldingFlg = false;
        ColliderFlag = 0;

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

        GM = GameObject.Find("GameManager");
        GMscript = GM.GetComponent<GameManager>();

        //オーディオの情報取得
        sounds = GetComponent<AudioSource>();
        Move_arrow();

        // Arrow_Listの矢印を全て非表示にしている
        for (int i = 0; i < Arrow_List.Count; i++)
        {
            Arrow_List[i].SetActive(false);
        }
    }

    void Update() {
        if (script.PauseFlag) {
            return;
        }
        else {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            // 今選択しているカーソルの位置を代入している
            if (C3_script.pot_flg)
                direction = C3_script.PCS_List[C3_script.Pcursor].transform.position;
            else
                direction = C3_script.Cursor_List[C3_script.cursor].transform.position;

            // スペースを離したときにカーソル移動ができるようにしている
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("XBox_joystick_B")) MoveFlg = false;
            if (Physics.Linecast(Player_V, direction, out hit)) {
                Debug.DrawLine(Player_V, direction, Color.red);

                // 手に何か持っていたら矢印を出す
                if(0< ClickObj2.gameObject.transform.childCount && !tekitou_flg)
                    Arrow_Control();
                else if(0 == ClickObj2.gameObject.transform.childCount && tekitou_flg)
                    {

                        tekitou_flg = false;
                        for (int i = 0; i < Arrow_List.Count-3; i++)
                        {
                            Arrow_List[i].SetActive(false);
                        }
                }

                /*-----------------------------------------------------------------*/
                Debug.Log("AgeCount = " + AgeCount);
                //Debug.Log("[0,1] = " + GMscript.ItemName[0, 1]);
                //Debug.Log("[0,2] = " + GMscript.ItemName[0, 2]);

                if (AgeCount >= 1 && !Arrow_List[0].activeSelf) // 鍋にFrideが一つ以上あれば鍋上に矢印を出す
                {
                    Arrow_List[0].SetActive(true);
                    NabeArrow_flg = true;
                }
                //else if (AgeCount == 0 && NabeArrow_flg)
                //{
                //    NabeArrow_flg = false;
                //    Arrow_List[0].SetActive(false);
                //}

                //Debug.Log("エビ注文数："+ Shrimp_order);
                //Debug.Log("さかな注文数："+ Fish_order);
                //Debug.Log("イモ注文数："+ Potato_order);
                // 左席の注文を確認
                if (GMscript.ItemName[0, 0] != null && !Audience0_flg)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (GMscript.ItemName[0, i] == "Dish_T_Shrimp") Shrimp_order += 1;
                        else if (GMscript.ItemName[0, i] == "Dish_T_Fish") Fish_order += 1;
                        else if (GMscript.ItemName[0, i] == "Dish_T_Potato") Potato_order += 1;
                    }

                    Box_Arrow();
                    Audience0_flg = true; // 客が席についた時一度だけ注文の内容を記憶するため
                }
                else if (GMscript.ItemName[0, 0] == null) Audience0_flg = false;
                
                // 盛り付けたものを持った時客がその商品を注文してるか確認
                if(HoldingFlg && ClickObj2.GetChild(0).gameObject.name.Contains("Dish") && !Offer_flg)
                {
                    for (int i = 0; i < 3; i++){
                        for (int j = 0; j < 3; j++)
                        {
                            if (GMscript.ItemName[i, j] == ClickObj2.GetChild(0).gameObject.name)
                            {// 客が注文していたら客席向けの矢印をだして、注文している客席に矢印を出す
                                tmp = C3_script.Cursor_List[16].transform.position;
                                Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 1f, tmp.z), Quaternion.Euler(90, 0, 0));
                                tmp = C3_script.Cursor_List[21].transform.position;
                                Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 1f, tmp.z), Quaternion.Euler(90, 0, 0));
                                Offer_flg = true;
                                return;
                            }

                        }
                    }
                }
            /*-----------------------------------------------------------------*/

                TargetTag = hit.collider.gameObject.tag; // 今見ているOBJのタグを保存
                TargetObj = hit.collider.gameObject; // 今見ているOBJを保存(C3のアウトラインのオンオフで使う)

                // てんぷら粉、ウズラの液と粉、を選択中はフラグを立てる
                if (C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[2] ||
                    C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[11] ||
                    C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[12] ||
                    C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[14] ||
                    C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[15]) KonaFlag = true;
                else KonaFlag = false;

                // フラグがたっていないとボタンが効かない
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("XBox_joystick_B")) && C3_script.space_flg) {
                    MoveFlg = true;

                    // ストックにsaraを置いたときtrue(C3のストック自動選択で使う)
                    if (C3_script.stock_flg && ItemSara) C3_script.StockEX_flg = true;
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
                                    //サウンド再生
                                    sounds.Play();
                                    break;
                                case "ChickenBox":
                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Chicken");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ItemPowder = true;
                                    ColliderFlag = 1;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    //サウンド再生
                                    sounds.Play();
                                    break;
                                case "FishBox":
                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Fish");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ColliderFlag = 2;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    //サウンド再生
                                    sounds.Play();
                                    break;
                                case "PotatoBox":
                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Potato");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ColliderFlag = 3;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    //サウンド再生
                                    sounds.Play();
                                    break;
                                case "QuailBox":
                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Quail");   //Resourceフォルダのプレハブを読み込む
                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                    HoldingFlg = true;
                                    ColliderFlag = 4;
                                    //当たり判定をを外す
                                    ColliderOut();
                                    //サウンド再生
                                    sounds.Play();
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
                            //サウンド再生
                            sounds.Play();
                        }

                    }
                    else if ((ItemSara && (hit.collider.gameObject.tag == "Stock" || hit.collider.gameObject.tag == "Seki" || hit.collider.gameObject.tag == "Garbage can")) ||
                            (!ItemSara && hit.collider.gameObject.tag != "Item" && hit.collider.gameObject.tag != "Box" && hit.collider.gameObject.tag != "Stock"))
                    // 粉や鍋にすでに食材があるなら食材を置けないようにしている(唐揚げは何個でも置ける)
                    {
                        if (C3_script.pot_flg) C3_script.PotEX_flg = true;

                        ItemPowder = false; // 粉をつけたものを鍋に置いたときにFalse
                        //当たり判定を入れる
                        ColliderIn();
                        ClickObj2.GetChild(0).gameObject.transform.position = hit.point; // 見ているところに置く
                        clickedGameObject.transform.parent = null;              //手との親子付けを解除

                        clickedGameObject.GetComponent<Rigidbody>().isKinematic = false;    //重力を有効

                        clickedGameObject = null;   //対象を入れる箱を初期化
                        Resource = null;            //生成するプレハブの箱を初期化

                        HoldingFlg = false;
                        //サウンド再生
                        sounds.Play();
                    }
                    if (clickedGameObject != null)  //nullでないとき処理
                    {
                        clickedGameObject.transform.parent = ClickObj.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
                        clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
                    }

                    // 生の食材を持っている時、取った場所に戻せる処理
                    if(hit.collider.gameObject.tag == "Box"){
                        bool return_flg = false; // trueの時にBoxに戻す処理をする
                        switch (clickedGameObject.name)
                        {
                            case "Item_Shrimp":
                                if (hit.collider.gameObject.name.Contains("Ebi"))
                                    return_flg = true;
                                //サウンド再生
                                sounds.Play();
                                break;
                            case "Item_Fish_v2":
                                if (hit.collider.gameObject.name.Contains("Fish"))
                                    return_flg = true;
                                //サウンド再生
                                sounds.Play();
                                break;
                            case "Item_Potato":
                                if (hit.collider.gameObject.name.Contains("Potato"))
                                    return_flg = true;
                                //サウンド再生
                                sounds.Play();
                                break;
                            case "Item_Friedchicken":
                                if (hit.collider.gameObject.name.Contains("hicken"))
                                    return_flg = true;
                                //サウンド再生
                                sounds.Play();
                                break;
                            case "Item_Quail":
                                if (hit.collider.gameObject.name.Contains("Quail"))
                                    return_flg = true;
                                //サウンド再生
                                sounds.Play();
                                break;
                            default:
                                break;
                        }
                        if (return_flg)
                        {
                            Destroy(clickedGameObject); // 今持っているものを削除
                            clickedGameObject = null;   //対象を入れる箱を初期化
                            Resource = null;            //生成するプレハブの箱を初期化
                            HoldingFlg = false;         //物を持っているかどうかをFalse
                        }
                    }
                }

                // 粉系か皿に置くときに、ボタンを離すと手元に戻ってくるようにしている
                if (KonaFlag && (hit.collider.gameObject.name.Contains("Dish") || hit.collider.gameObject.tag == "Item") &&
                    (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("XBox_joystick_B"))) {
                    //サウンド再生
                    sounds.Play();
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

            if((Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("XBox_joystick_B")) && !HoldingFlg)
            {
                // 注文のBoxの上に矢印を出す
                //Debug.Log("↓↓↓");
                Offer_flg = false;
                Box_Arrow();
            }

        }
    }

    //当たり判定を切る関数
    void ColliderOut() {
        //clickedGameObject.GetComponent<Collider>().enabled = false;
    }

    //当たり判定を入れる関数
    void ColliderIn() {
        clickedGameObject.GetComponent<Collider>().enabled = true;
    }

    void Arrow_Control()
    {
        tekitou_flg = true;

        //天ぷら粉に矢印
        if (ClickObj2.GetChild(0).gameObject.name.Contains("Item"))
        {
            for (int i = 0; i < Arrow_List.Count-3; i++)
            {
                Arrow_List[i].SetActive(false);
            }
            Arrow_List[1].SetActive(true);
            //tekitou_flg = true;
        }
        //鍋に矢印
        if (ClickObj2.GetChild(0).gameObject.name.Contains("Powder"))
        {
            for (int i = 0; i < Arrow_List.Count - 3; i++)
            {
                Arrow_List[i].SetActive(false);
            }
            Arrow_List[0].SetActive(true);
            //tekitou_flg = true;
        }
        //皿に矢印
        if (ClickObj2.GetChild(0).gameObject.name.Contains("Fried"))
        {
            for (int i = 0; i < Arrow_List.Count - 3; i++)
            {
                Arrow_List[i].SetActive(false);
            }
            Arrow_List[2].SetActive(true);
            //tekitou_flg = true;
        }
        //// 天ぷら粉に矢印
        //if(!HoldingFlg && TargetTag == "Box")
        //{
        //    Destroy(GameObject.FindGameObjectWithTag("box_Arrow"));
        //    tmp = C3_script.Cursor_List[2].transform.position;
        //    ArrowObj.tag = "kona_Arrow";
        //    Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z), Quaternion.identity);
        //}
        //// 天ぷら鍋に矢印
        //else if (HoldingFlg && ClickObj2.GetChild(0).gameObject.name.Contains("Item")&& TargetTag == "kona")
        //{
        //    Destroy(GameObject.FindGameObjectWithTag("kona_Arrow"));
        //    tmp = C3_script.Cursor_List[1].transform.position;
        //    if (!NabeArrow_flg)
        //    {
        //        NabeArrow_flg = true;
        //        ArrowObj.tag = "tennabe_Arrow";
        //        Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z), Quaternion.identity);
        //    }
        //}
        //// Powderを鍋に入れると鍋の矢印を消す
        //else if(HoldingFlg && ClickObj2.GetChild(0).gameObject.name.Contains("Powder") && TargetTag == "tenpuranabe")
        //{
        //    if (AgeCount == 0)
        //    {
        //        Destroy(GameObject.FindGameObjectWithTag("tennabe_Arrow"));
        //        NabeArrow_flg = false;
        //    }
        //}
        //// 鍋からFriedを取るとSaraに矢印
        //else if(!HoldingFlg && TargetObj.name.Contains("Fried"))
        //{
        //    if (AgeCount == 1) DestroyFlg = true;
        //    tmp = C3_script.Cursor_List[15].transform.position;
        //    ArrowObj.tag = "sara_Arrow";
        //    Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z), Quaternion.identity);
        //}
        //// Friedを盛り付けると皿上の矢印を消す
        //else if (HoldingFlg && ClickObj2.GetChild(0).gameObject.name.Contains("Fried") && TargetTag == "Sara")
        //{
        //    if (AgeCount == 1) NabeArrow_flg = false;
        //    Destroy(GameObject.FindGameObjectWithTag("sara_Arrow"));
        //}


    }

    // Boxの上に矢印を出させる処理
    void Box_Arrow()
    {
        if (Shrimp_order != 0)
        {
            tmp = C3_script.Cursor_List[3].transform.position;
            ArrowObj.tag = "box_Arrow";
            Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z), Quaternion.identity);
        }
        if (Fish_order != 0)
        {
            tmp = C3_script.Cursor_List[4].transform.position;
            ArrowObj.tag = "box_Arrow";
            Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z), Quaternion.identity);
        }
        if (Potato_order != 0)
        {
            tmp = C3_script.Cursor_List[5].transform.position;
            ArrowObj.tag = "box_Arrow";
            Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z), Quaternion.identity);
        }
    }

    void Move_arrow()
    {
        //ArrowTurn += 1;
        //switch (ArrowTurn)
        //{
        //    case 1:
        //        tmp = C3_script.Cursor_List[3].transform.position;
        //        Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z), Quaternion.identity);
        //        tmp = C3_script.Cursor_List[4].transform.position;
        //        Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z), Quaternion.identity);
        //        tmp = C3_script.Cursor_List[5].transform.position;
        //        break;
        //    case 2:
        //        tmp = C3_script.Cursor_List[2].transform.position;
        //        Destroy_count = 0;
        //        break;
        //    case 3:
        //        Destroy(GameObject.Find("Yajirusi(Clone)"));
        //        tmp = C3_script.Cursor_List[1].transform.position;
        //        break;
        //    case 4:
        //        Destroy(GameObject.Find("Yajirusi(Clone)"));
        //        break;
        //    case 5:
        //        tmp = C3_script.Cursor_List[1].transform.position;
        //        break;
        //    case 6:
        //        Destroy(GameObject.Find("Yajirusi(Clone)"));
        //        tmp = C3_script.Cursor_List[1].transform.position;
        //        break;
        //    case 7:
        //        Destroy(GameObject.Find("Yajirusi(Clone)"));
        //        tmp = C3_script.Cursor_List[15].transform.position;
        //        break;
        //}
        //if(ArrowTurn != 4)
        //Instantiate(ArrowObj, tmp = new Vector3(tmp.x, tmp.y + 0.2f, tmp.z), Quaternion.identity);

        //tmp_Turn = ArrowTurn;



        //else if (DestroyFlg || ArrowTurn == 9)
        //{
        //    Destroy(GameObject.Find("Yajirusi(Clone)"));
        //    DestroyFlg = false;
        //    ArrowFlg = false;
        //}
    }
}
//using UnityEngine;

//public class HandControllerButton_S2 : MonoBehaviour {
//    //このスクリプトはControllerMouseClickと共存しない
//    //このスクリプトはhandにいれる

//    GameObject clickedGameObject;
//    GameObject Resource;
//    GameObject ClickObj;
//    float handspeed = 0.1f;

//    [System.NonSerialized]public bool HoldingFlg;
//    //ColliderFlagの説明
//    /* 0はEbiBox
//     * 1はChickenBox
//     * 2FishBox
//     * 3PotatoBox
//     * 4QuailBox*/
//    public int ColliderFlag;
//    public Transform ClickObj2;
//    private Transform TmpFood; // 手に持っている物を入れる
//    private GameObject TmpObj; // レイで当たっているObjを入れる

//    GameObject C2;    // Camera_2を入れる変数
//    Camera_2 C2_script; // Camera_2のscriptを入れる変数
//    Camera_3 C3_script; // Camera_3のscriptを入れる変数

//    [SerializeField] GameObject Player; // プレイヤーの位置を保存
//    Vector3 Player_V;                   // プレイヤーの座標を保存する用
//    [System.NonSerialized] public Vector3 direction; // Rayの終点座標

//    //ポーズ画面
//    GameObject Pause;
//    Pause_Botton_Script script;

//    bool ItemSara;  //アイテム名にSaraが含まれているか判定
//    bool KonaFlag = false; // 〇を押すと粉に漬け、離すと手元に戻るようにするフラグ
//    [System.NonSerialized] public string TargetTag;//今見ているOBJのタグを保存する 
//    [System.NonSerialized] public GameObject TargetObj;//今見ているOBJのタグを保存する 

//    [System.NonSerialized] public bool ItemPowder; // 粉系を持っているかの判定フラグ
//    [System.NonSerialized] public bool MoveFlg = false; // スペースを押している間は移動できないようにするフラグ

//    void Start() {
//        ClickObj = GameObject.Find("ControllerObjClick");
//        HoldingFlg = false;
//        ColliderFlag = 0;

//        C2 = GameObject.Find("Main Camera");
//        C2_script = C2.GetComponent<Camera_2>();
//        C3_script = C2.GetComponent<Camera_3>();

//        // プレイヤーの座標をVector3に変換
//        Player_V.x = Player.transform.position.x;
//        Player_V.y = Player.transform.position.y;
//        Player_V.z = Player.transform.position.z;
//        direction = C3_script.Cursor_List[C3_script.cursor].transform.position;

//        //ポーズ画面
//        Pause = GameObject.Find("Main Camera");
//        script = Pause.GetComponent<Pause_Botton_Script>();
//    }

//    void Update(){
//        //Debug.Log(KonaFlag);
//        if (script.PauseFlag) {
//            return;
//        }
//        else {
//            Ray ray = new Ray();
//            RaycastHit hit = new RaycastHit();
//            // 今選択しているカーソルの位置を代入している
//            if (C3_script.pot_flg) 
//                direction = C3_script.PCS_List[C3_script.Pcursor].transform.position;           
//            else 
//                direction = C3_script.Cursor_List[C3_script.cursor].transform.position;



//            // スペースを離したときにカーソル移動ができるようにしている
//            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("〇")) MoveFlg = false;
//            if (Physics.Linecast(Player_V, direction, out hit)) {
//                Debug.DrawLine(Player_V, direction, Color.red);

//                TargetTag = hit.collider.gameObject.tag; // 今見ているOBJのタグを保存
//                //TargetObj = hit.collider.gameObject; // 今見ているOBJを保存(C3のアウトラインのオンオフで使う)

//                // てんぷら粉、ウズラの液と粉、を選択中はフラグを立てる
//                if (C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[2]  ||
//                    C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[11] ||
//                    C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[12] ||
//                    C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[14] ||
//                    C3_script.Cursor_List[C3_script.cursor] == C3_script.Cursor_List[15] ) KonaFlag = true;
//                else KonaFlag = false;

//                // フラグがたっていないとボタンが効かない
//                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("〇")) && C3_script.space_flg ) {
//                    MoveFlg = true;
//                    if (!HoldingFlg) // 手に何も持っていない時に入る
//                    {
//                        if (hit.collider.gameObject.tag == "Box") {
//                            switch (hit.collider.gameObject.name) {
//                                case "EbiBox":
//                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Shrimp");   //Resourceフォルダのプレハブを読み込む
//                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
//                                    HoldingFlg = true;
//                                    ColliderFlag = 0;
//                                    //当たり判定をを外す
//                                    ColliderOut();
//                                    break;
//                                case "ChickenBox":
//                                    Resource = (GameObject)Resources.Load("S_Resources/ItemChicken");   //Resourceフォルダのプレハブを読み込む
//                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
//                                    HoldingFlg = true;
//                                    ItemPowder = true;
//                                    ColliderFlag = 1;
//                                    //当たり判定をを外す
//                                    ColliderOut();
//                                    break;
//                                case "FishBox":
//                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Fish");   //Resourceフォルダのプレハブを読み込む
//                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
//                                    HoldingFlg = true;
//                                    ColliderFlag = 2;
//                                    //当たり判定をを外す
//                                    ColliderOut();
//                                    break;
//                                case "PotatoBox":
//                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Potato");   //Resourceフォルダのプレハブを読み込む
//                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
//                                    HoldingFlg = true;
//                                    ColliderFlag = 3;
//                                    //当たり判定をを外す
//                                    ColliderOut();
//                                    break;
//                                case "QuailBox":
//                                    Resource = (GameObject)Resources.Load("R_Resources/Item_Quail");   //Resourceフォルダのプレハブを読み込む
//                                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
//                                    HoldingFlg = true;
//                                    ColliderFlag = 4;
//                                    //当たり判定をを外す
//                                    ColliderOut();
//                                    break;

//                            }
//                                ItemSara = false;                            
//                        }

//                        if (hit.collider.gameObject.tag == "Item") {
//                            clickedGameObject = hit.collider.gameObject;                              //タグがなければオブジェクトをclickedGameObjectにいれる
//                            clickedGameObject.transform.position = ClickObj.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
//                            HoldingFlg = true;

//                            if (hit.collider.gameObject.name.Contains("Dish"))
//                                ItemSara = hit.collider.gameObject.name.Contains("Dish");
//                            else if (hit.collider.gameObject.name.Contains("Sara"))
//                                ItemSara = hit.collider.gameObject.name.Contains("Sara"); // 後で消す
//                            else ItemSara = false;

//                            //当たり判定をを外す
//                            ColliderOut();
//                        }

//                        //ClickObj2.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = false;
//                    }
//                    else if ((ItemSara && (hit.collider.gameObject.tag == "Stock" || hit.collider.gameObject.tag == "Seki" || hit.collider.gameObject.tag == "Garbage can")) ||
//                            (!ItemSara &&  hit.collider.gameObject.tag != "Item"  && hit.collider.gameObject.tag != "Box"  && hit.collider.gameObject.tag != "Stock"))
//                    // 粉や鍋にすでに食材があるなら食材を置けないようにしている(唐揚げは何個でも置ける)
//                    {
//                        ItemPowder = false; // 粉をつけたものを鍋に置いたときにFalse
//                        //当たり判定を入れる
//                        ColliderIn();
//                        ClickObj2.GetChild(0).gameObject.transform.position = hit.point; // 見ているところに置く
//                        clickedGameObject.transform.parent = null;              //手との親子付けを解除

//                        clickedGameObject.GetComponent<Rigidbody>().isKinematic = false;    //重力を有効

//                        clickedGameObject = null;   //対象を入れる箱を初期化
//                        Resource = null;            //生成するプレハブの箱を初期化

//                        HoldingFlg = false;
//                    }
//                    if (clickedGameObject != null)  //nullでないとき処理
//                    {
//                        clickedGameObject.transform.parent = ClickObj.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
//                        clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
//                    }
//                }

//                // 粉系か皿に置くときに、ボタンを離すと手元に戻ってくるようにしている
//                if (KonaFlag && (hit.collider.gameObject.name.Contains("Dish") || hit.collider.gameObject.tag == "Item" ) && 
//                    (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("〇")))
//                {
//                    if (hit.collider.gameObject.name.Contains("Dish")) ItemSara = true;
//                    else if (hit.collider.gameObject.tag == "Item") ItemPowder = true;

//                    clickedGameObject = hit.collider.gameObject;                              //タグがなければオブジェクトをclickedGameObjectにいれる
//                    clickedGameObject.transform.position = ClickObj.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
//                    HoldingFlg = true;

//                    //当たり判定をを外す
//                    ColliderOut();
//                    clickedGameObject.transform.parent = ClickObj.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
//                    clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
//                }
//            }

//        }
//    }

//    //当たり判定を切る関数
//    void ColliderOut() {
//        clickedGameObject.GetComponent<Collider>().enabled = false;
//    }

//    //当たり判定を入れる関数
//    void ColliderIn() {
//        clickedGameObject.GetComponent<Collider>().enabled = true;
//    }
//}