using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    [System.NonSerialized] public string TmpFood; // 手に持っている物の名前を記憶
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

    [System.NonSerialized] public bool ItemSara;  //アイテム名にSaraが含まれているか判定
    bool KonaFlag = false; // 〇を押すと粉に漬け、離すと手元に戻るようにするフラグ
    [System.NonSerialized] public string TargetTag;//今見ているOBJのタグを保存する 
    [System.NonSerialized] public GameObject TargetObj;//今見ているOBJを保存する 

    [System.NonSerialized] public bool ItemPowder; // 粉系を持っているかの判定フラグ
    [System.NonSerialized] public bool MoveFlg = false; // スペースを押している間は移動できないようにするフラグ

    /*------- 矢印関連 -------*/
    [SerializeField] GameObject ArrowObj; // 矢印のObjを入れる変数
    [System.NonSerialized] public int AgeCount; // 鍋でFrideになっている数をカウント（天ぷら鍋に矢印を出すときに使う）
    [System.NonSerialized] public int AgeCount2;// 　　〃　（揚げ物鍋に矢印を出すときに使う）
    bool NabeArrow_flg = false; // true = 鍋に矢印表示中
    GameObject GM;
    GameManager GMscript;
    [System.NonSerialized] public int Shrimp_order;  // 各注文数を記憶させる
    [System.NonSerialized] public int Fish_order;
    [System.NonSerialized] public int Potato_order;
    [System.NonSerialized] public int Chicken_order;
    [System.NonSerialized] public int Quail_order;
    bool tekitou_flg = false;
    bool DEBU_flg = false; // デブが席に着くとtrue

    // 席について注文を記憶する=true, 席から離れる=false
    bool Audience0_flg = false; // 0番目の席用
    bool Audience1_flg = false; // 1番目の席用
    bool Audience2_flg = false; // 2番目の席用
    bool[,] OrderCount= new bool[3, 5]; // [席番号, 0=エビ・1=サカナ・2=イモ・3=唐揚げ・4=うずら]
    [SerializeField] List<GameObject> Arrow_List = new List<GameObject>(); // 矢印の場所を入れるリスト

    string SceneName; // 現在のscene名を入れる

    //オーディオ
    AudioSource sounds;

    void Start() {
        // 現在のscene名を入れている
        SceneName = SceneManager.GetActiveScene().name;

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

        // Arrow_Listの矢印を全て非表示にしている
        for (int i = 0; i < Arrow_List.Count; i++)
        {
            Arrow_List[i].SetActive(false);
        }
        for (int i=0; i < 3; i++){
            for(int j = 0; j < 5; j++)
            {
                OrderCount[i, j] = false;
                if(j < 3) GMscript.ItemName[i, j] = null;
            }
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

                /*----------------------------------------------<矢印の処理>------------------------------------------------------------------------------------*/
                
                // 手に何か持っていたら矢印を出す
                if (0 < ClickObj2.gameObject.transform.childCount && !tekitou_flg)
                    Arrow_Control();
                else if (0 == ClickObj2.gameObject.transform.childCount && tekitou_flg && TmpFood != null)
                {
                    if (TmpFood.Contains("Shrimp")) Shrimp_order += 1;
                    if (TmpFood.Contains("Fish")) Fish_order += 1;
                    if (TmpFood.Contains("Potato")) Potato_order += 1;
                    if (TmpFood.Contains("Chicken")) Chicken_order += 1;
                    if (TmpFood.Contains("Quail")) Quail_order += 1;
                    tekitou_flg = false;
                    TmpFood = null;
                    for (int i = 0; i < Arrow_List.Count - 5; i++)
                    {
                        Arrow_List[i].SetActive(false);
                    }
                }
                //Debug.Log("今持っているものの名前 = " + TmpFood);
                //Debug.Log("ポテト = " + Potato_order);
                //Debug.Log("サカナ = " + Fish_order);
                //Debug.Log("エビ = " + Shrimp_order);

                if (SceneName == "Easy_Scene" && AgeCount >= 1 && !Arrow_List[0].activeSelf) // 天ぷら鍋にFrideが一つ以上あれば鍋上に矢印を出す
                    Arrow_List[0].SetActive(true);
                if (AgeCount2 >= 1 && !Arrow_List[7].activeSelf) // 揚げ物鍋に　〃
                    Arrow_List[7].SetActive(true);
                

                // 左席の注文を確認---------------------------------------------------------------------
                if (GMscript.ItemName[0, 0] != null && !Audience0_flg)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (SceneName == "Easy_Scene")
                        {
                            switch(GMscript.ItemName[0, i])
                            {
                                case "Dish_T_Shrimp":
                                    Shrimp_order += 1;
                                    OrderCount[0, 0] = true;
                                    break;
                                case "Dish_T_Fish":
                                    Fish_order += 1;
                                    OrderCount[0, 1] = true;
                                    break;
                                case "Dish_T_Potato":
                                    Potato_order += 1;
                                    OrderCount[0, 2] = true;
                                    break;
                            }
                        }
                        else // NormalかHardだった場合
                        {
                            if (GMscript.ItemName[0, i] == "Dish_K_Chicken")
                            {
                                Chicken_order += 1;
                                OrderCount[0, 3] = true;
                            }
                            else if (GMscript.ItemName[0, i] == "Dish_K_Quail")
                            {
                                Quail_order += 1;
                                OrderCount[0, 4] = true;
                            }
                        }
                    }
                    // 席に着いた客がデブかどうか調べる
                    if (GMscript.ItemName[0, 2] != null) DEBU_flg = true;
                    Box_Arrow();
                    Audience0_flg = true; // 客が席についた時一度だけ注文の内容を記憶するため
                }
                else if (GMscript.ItemName[0, 0] == null) Audience0_flg = false;


                //　中央席の注文を確認---------------------------------------------------------------------
                if (GMscript.ItemName[1, 0] != null && !Audience1_flg)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (SceneName == "Easy_Scene")
                        {
                            switch (GMscript.ItemName[1, i])
                            {
                                case "Dish_T_Shrimp":
                                    Shrimp_order += 1;
                                    OrderCount[1, 0] = true;
                                    break;
                                case "Dish_T_Fish":
                                    Fish_order += 1;
                                    OrderCount[1, 1] = true;
                                    break;
                                case "Dish_T_Potato":
                                    Potato_order += 1;
                                    OrderCount[1, 2] = true;
                                    break;
                            }
                        }
                        else // NormalかHardだった場合
                        {
                            if (GMscript.ItemName[1, i] == "Dish_K_Chicken")
                            {
                                Chicken_order += 1;
                                OrderCount[1, 3] = true;
                            }
                            else if (GMscript.ItemName[1, i] == "Dish_K_Quail")
                            {
                                Quail_order += 1;
                                OrderCount[1, 4] = true;
                            }
                        }
                    }

                    Box_Arrow();
                    Audience1_flg = true; // 客が席についた時一度だけ注文の内容を記憶するため
                }
                else if (GMscript.ItemName[1, 0] == null) Audience1_flg = false;

                // 右席の注文を確認---------------------------------------------------------------------
                if (GMscript.ItemName[2, 0] != null && !Audience2_flg)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (SceneName == "Easy_Scene")
                        {
                            switch (GMscript.ItemName[2, i])
                            {
                                case "Dish_T_Shrimp":
                                    Shrimp_order += 1;
                                    OrderCount[2, 0] = true;
                                    break;
                                case "Dish_T_Fish":
                                    Fish_order += 1;
                                    OrderCount[2, 1] = true;
                                    break;
                                case "Dish_T_Potato":
                                    Potato_order += 1;
                                    OrderCount[2, 2] = true;
                                    break;
                            }
                        }
                        else // NormalかHardだった場合
                        {
                            if (GMscript.ItemName[2, i] == "Dish_K_Chicken")
                            {
                                Chicken_order += 1;
                                OrderCount[2, 3] = true;
                            }
                            else if (GMscript.ItemName[2, i] == "Dish_K_Quail")
                            {
                                Quail_order += 1;
                                OrderCount[2, 4] = true;
                            }
                        }
                    }

                    Box_Arrow();
                    Audience2_flg = true; // 客が席についた時一度だけ注文の内容を記憶するため
                }
                else if (GMscript.ItemName[2, 0] == null) Audience2_flg = false;
                

            /*---------------------------------------------------------------------------------------------------------------------------------------*/

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
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("XBox_joystick_B")) && C3_script.space_flg && TargetTag != "BATU") {
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
                            else ItemSara = false;


                            //当たり判定をを外す
                            ColliderOut();
                            //サウンド再生
                            sounds.Play();
                        }

                    }

                    /* ・焦げを持っていてゴミ箱でスペースを押したら
                       ・Dishを持ているとき、ストック・席・ゴミ箱以外に置けないようにしている
                       ・Dishと焦げ以外を持っているとき、アイテム・Box・ストック・席に置けないようにしている
                        */
                    else if ((ClickObj2.GetChild(0).gameObject.name.Contains("Burn") && hit.collider.gameObject.tag == "Garbage can") ||
                            (ItemSara && (hit.collider.gameObject.tag == "Stock" || hit.collider.gameObject.tag == "Seki" || hit.collider.gameObject.tag == "Garbage can")) ||
                            ((!ItemSara && !ClickObj2.GetChild(0).gameObject.name.Contains("Burn")) && hit.collider.gameObject.tag != "Item" && hit.collider.gameObject.tag != "Box" && hit.collider.gameObject.tag != "Stock"  && hit.collider.gameObject.tag != "Seki")) 
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
                            case "Item_Fish":
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
                            case "Item_Chicken":
                                if (hit.collider.gameObject.name.Contains("Chicken"))
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
        TmpFood = ClickObj2.GetChild(0).gameObject.name;
        tekitou_flg = true;
        // 今持っているもののオーダーを一時的に-1している
        if(ClickObj2.GetChild(0).gameObject.name.Contains("Shrimp"))Shrimp_order -= 1;
        if(ClickObj2.GetChild(0).gameObject.name.Contains("Fish")) Fish_order -= 1;
        if(ClickObj2.GetChild(0).gameObject.name.Contains("Potato")) Potato_order -= 1;
        if(ClickObj2.GetChild(0).gameObject.name.Contains("Chicken")) Chicken_order -= 1;
        if(ClickObj2.GetChild(0).gameObject.name.Contains("Quail")) Quail_order -= 1;

        // 各オーダーが0以下になったら各Boxの矢印を非表示にする
        if(Shrimp_order <= 0)  Arrow_List[12].SetActive(false);
        if(Fish_order <= 0)    Arrow_List[13].SetActive(false);
        if(Potato_order <= 0)  Arrow_List[14].SetActive(false);
        if(Chicken_order <= 0) Arrow_List[15].SetActive(false);
        if(Quail_order <= 0)   Arrow_List[16].SetActive(false);

        for (int i = 0; i < Arrow_List.Count - 5; i++)
        {
            Arrow_List[i].SetActive(false);
        }

        // Easyで天ぷら関連だけの矢印操作
        if (SceneName == "Easy_Scene")
        {
            switch (ClickObj2.GetChild(0).gameObject.name)
            {
                // 天ぷら粉に矢印
                case "Item_Shrimp":
                case "Item_Potato":
                case "Item_Fish":
                    Arrow_List[1].SetActive(true);
                    break;
                //天ぷら側 鍋に矢印
                case "Powder_Shrimp":
                case "Powder_Potato":
                case "Powder_Fish":
                    Arrow_List[0].SetActive(true);
                    break;
                // 皿に矢印
                case "Fried_T_Shrimp":
                case "Fried_K_Potato":
                case "Fried_T_Fish":
                    Arrow_List[2].SetActive(true); // 天ぷら側の皿
                    break;
                // 正面を向かせる用の矢印
                case "Dish_T_Shrimp":
                case "Dish_T_Potato":
                case "Dish_T_Fish":
                    for (int i = 0; i < 3; i++){
                        for (int j = 0; j < 3; j++)
                        {
                            if (ClickObj2.GetChild(0).gameObject.name == GMscript.ItemName[i, j])
                            {
                                Arrow_List[3].SetActive(true);//正面を向かせる用の矢印
                                Arrow_List[i + 4].SetActive(true); // 注文している客に矢印
                            }
                        }
                    }
                    break;
            }
        }
        // Normalで唐揚げ関連だけの矢印操作
        else if (SceneName == "Normal_Scene")
        {
            switch (ClickObj2.GetChild(0).gameObject.name)
            {
                // 揚げ物側 鍋に矢印
                case "Item_Chicken":
                    Arrow_List[7].SetActive(true);
                    break;
                // 皿に矢印
                case "Fried_K_Chicken":
                    Arrow_List[2].SetActive(true); // 天ぷら側の皿
                    Arrow_List[10].SetActive(true); // 揚げ物側の皿
                    break;
                // 正面を向かせる用の矢印
                case "Dish_K_Chicken":
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (ClickObj2.GetChild(0).gameObject.name == GMscript.ItemName[i, j])
                            {
                                Arrow_List[3].SetActive(true);//正面を向かせる用の矢印
                                Arrow_List[11].SetActive(true);
                                Arrow_List[i + 4].SetActive(true); // 注文している客に矢印
                            }
                        }
                    }
                    break;
            }
        }
        // Hardでうずら関連だけの矢印操作
        else if (SceneName == "Hard_Scene")
        {
            switch (ClickObj2.GetChild(0).gameObject.name)
            {
                // うずらの粉
                case "Item_Quail":
                case "Powder_Quail2":
                    Arrow_List[9].SetActive(true);
                    break;
                // うずらの液
                case "Powder_Quail1":
                case "Powder_Quail3":
                    Arrow_List[8].SetActive(true);
                    break;
                // 揚げ物側 鍋に矢印
                case "Powder_Quail4":
                    Arrow_List[7].SetActive(true);
                    break;
                // 皿に矢印
                case "Fried_K_Quail":
                    Arrow_List[2].SetActive(true); // 天ぷら側の皿
                    Arrow_List[10].SetActive(true); // 揚げ物側の皿
                    break;

                // 正面を向かせる用の矢印
                case "Dish_K_Quail":
                    for (int i = 0; i < 3; i++){
                        for (int j = 0; j < 3; j++)
                        {
                            if (ClickObj2.GetChild(0).gameObject.name == GMscript.ItemName[i, j])
                            {
                                Arrow_List[3].SetActive(true);//正面を向かせる用の矢印
                                Arrow_List[11].SetActive(true);
                                Arrow_List[i + 4].SetActive(true); // 注文している客に矢印
                            }
                        }
                    }
                    break;
            }
        }
            //switch (ClickObj2.GetChild(0).gameObject.name) {
            //    // 天ぷら粉に矢印
            //    case "Item_Shrimp":
            //    case "Item_Potato":
            //    case "Item_Fish":
            //        Arrow_List[1].SetActive(true);
            //        break;
            //    // うずらの粉
            //    case "Item_Quail":
            //    case "Powder_Quail2":
            //        Arrow_List[9].SetActive(true);
            //        break;
            //    // うずらの液
            //    case "Powder_Quail1":
            //    case "Powder_Quail3":
            //        Arrow_List[8].SetActive(true);
            //        break;

            //    //天ぷら側 鍋に矢印
            //    case "Powder_Shrimp":
            //    case "Powder_Potato":
            //    case "Powder_Fish":
            //        Arrow_List[0].SetActive(true);
            //        break;
            //    // 揚げ物側 鍋に矢印
            //    case "Item_Chicken":
            //    case "Powder_Quail4":
            //        Arrow_List[7].SetActive(true);
            //        break;
            //    // 皿に矢印
            //    case "Fried_T_Shrimp":
            //    case "Fried_K_Potato":
            //    case "Fried_T_Fish":
            //    case "Fried_K_Chicken":
            //    case "Fried_K_Quail":
            //        Arrow_List[2].SetActive(true); // 天ぷら側の皿
            //        Arrow_List[10].SetActive(true); // 揚げ物側の皿
            //        break;

            //    // 焦げを持っているときは矢印を全部消す（Box以外）
            //    case "Burn_Shrimp":
            //    case "Burn_Potato":
            //    case "Burn_Fish":
            //    case "Burn_Chicken":
            //    case "Burn_Quail":
            //        break;
            //    // 正面を向かせる用の矢印
            //    case "Dish_T_Shrimp":
            //    case "Dish_T_Potato":
            //    case "Dish_T_Fish":
            //    case "Dish_K_Chicken":
            //    case "Dish_K_Quail":
            //        for (int i = 0; i < 3; i++)
            //        {
            //            for (int j = 0; j < 3; j++)
            //            {
            //                if (ClickObj2.GetChild(0).gameObject.name == GMscript.ItemName[i, j])
            //                {
            //                    Arrow_List[3].SetActive(true);//正面を向かせる用の矢印
            //                    Arrow_List[11].SetActive(true);
            //                    Arrow_List[i + 4].SetActive(true); // 注文している客に矢印
            //                }
            //            }
            //        }
            //        break;

            //}


            ////天ぷら粉に矢印
            //if (ClickObj2.GetChild(0).gameObject.name.Contains("Item"))
            //{
            //    for (int i = 0; i < Arrow_List.Count-5; i++)
            //    {
            //        Arrow_List[i].SetActive(false);
            //    }
            //    Arrow_List[1].SetActive(true);
            //    //tekitou_flg = true;
            //}


            //天ぷら側 鍋に矢印
            //if (ClickObj2.GetChild(0).gameObject.name.Contains("Powder"))
            //{
            //    for (int i = 0; i < Arrow_List.Count - 5; i++)
            //    {
            //        Arrow_List[i].SetActive(false);
            //    }
            //    Arrow_List[0].SetActive(true);
            //}

            // 揚げ物側 鍋に矢印
            //if (ClickObj2.GetChild(0).gameObject.name.Contains("Chicken"))
            //{
            //    for (int i = 0; i < Arrow_List.Count - 5; i++)
            //    {
            //        Arrow_List[i].SetActive(false);
            //    }
            //    Arrow_List[7].SetActive(true);
            //}

            //皿に矢印
            //if (ClickObj2.GetChild(0).gameObject.name.Contains("Fried"))
            //{
            //    for (int i = 0; i < Arrow_List.Count - 5; i++)
            //    {
            //        Arrow_List[i].SetActive(false);
            //    }
            //    Arrow_List[2].SetActive(true); // 天ぷら側の皿
            //    Arrow_List[10].SetActive(true); // 揚げ物側の皿
            //}

            ////正面を向かせる用の矢印
            //if (ClickObj2.GetChild(0).gameObject.name == GMscript.ItemName[0, 0])
            //{
            //    for (int i = 0; i < Arrow_List.Count - 3; i++)
            //    {
            //        Arrow_List[i].SetActive(false);
            //    }
            //    Arrow_List[3].SetActive(true);
            //}

            // 焦げを持っているときは矢印を全部消す（Box以外）
            //if (ClickObj2.GetChild(0).gameObject.name.Contains("Burn") )
            //{
            //    for (int i = 0; i < Arrow_List.Count - 3; i++)
            //    {
            //        Arrow_List[i].SetActive(false);
            //    }
            //}



            // 今持っているものを注文している客席に矢印を出す
            //if (ClickObj2.GetChild(0).gameObject.name.Contains("Dish"))
            //{
            //    for(int i = 0; i < 3; i++){
            //        for (int j = 0; j < 3; j++)
            //        {
            //            if (ClickObj2.GetChild(0).gameObject.name == GMscript.ItemName[i, j])
            //            {
            //                Arrow_List[3].SetActive(true);//正面を向かせる用の矢印
            //                Arrow_List[11].SetActive(true);
            //                Arrow_List[i + 4].SetActive(true); // 注文している客に矢印
            //            }
            //        }
            //    }          
            //}

        }

    // Boxの上に矢印を出させる処理
    void Box_Arrow()
    {
        // Easyでは天ぷら系のBoxにだけ矢印を出す
         if (SceneName == "Easy_Scene")
         {
            if (Shrimp_order >= 1 && !Arrow_List[12].activeSelf) // エビが一つ以上注文されているとBox上に矢印を出す
                Arrow_List[12].SetActive(true);

            if (Fish_order >= 1 && !Arrow_List[13].activeSelf) // 魚が一つ以上注文されているとBox上に矢印を出す
                Arrow_List[13].SetActive(true);

            if (Potato_order >= 1 && !Arrow_List[14].activeSelf) // イモが一つ以上注文されているとBox上に矢印を出す
                Arrow_List[14].SetActive(true);

         }

         // Normalでは唐揚げBoxだけに矢印を出す
         else if (SceneName == "Normal_Scene")
            if (Chicken_order >= 1 && !Arrow_List[15].activeSelf) // 唐揚げが一つ以上注文されているとBox上に矢印を出す
            Arrow_List[15].SetActive(true);

         // HardではうずらBoxだけに矢印を出す
         else if (SceneName == "Hard_Scene")
            if (Quail_order >= 1 && !Arrow_List[16].activeSelf) // うずらが一つ以上注文されているとBox上に矢印を出す
            Arrow_List[16].SetActive(true);
        
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