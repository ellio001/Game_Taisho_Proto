using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_S : MonoBehaviour {

    private float AgeCount;
    private float StockCount;
    private float AgeCountMax;
    private float KogeCountMax;
    private float StockCountMax;
    private bool kona;
    private bool BredPowder;
    private bool liquid;
    private bool QuailFry;
    private bool Secondliquid;
    private bool ThirdBreadPowder;
    private bool TaihiFlag;
    float alpha_Sin;    //オブジェクト発光の間隔(Sin波)
    bool Burnflag;      //焦げるフラグ
    bool Burnflag2;     //焦げてるか判定
    bool FastOneflag;   //天ぷら二度付け

    GameObject gameobject;
    GameObject Resource;
    GameObject objcolor;
    GameObject dummy;
    GameObject GM;              //GameMa agerがオブジェクト
    GameManager script;         //GameManagerが入る変数

    //Sliderを入れる
    public Slider slider;
    public Canvas canvas;

    //オーディオ
    AudioSource[] sounds;

    /*　パーティクル変数　*/
    bool effectflag = false;    //エフェクト始めるフラグ

    /*　パーティクル情報　*/

    // プレファブを入れる
    GameObject obj_Burn;
    // 鍋に入った物の座標を入れる
    Vector3 eff_pos;
    // エフェクトの回転を入れる
    Quaternion eff_rot;
    // 表示したエフェクトを入れる
    GameObject eff_Burn;

    // Use this for initialization
    void Start() {
        gameobject = this.gameObject;   //このオブジェクトの情報をいれる
        gameObject.name = gameObject.name.Replace("(Clone)", ""); //プレハブ生成時の(Clone)を消す
        Resource = null;                //生成するプレハブの箱を初期化
        //AgeCount = 0f;                  //カウント初期化
        StockCount = 0f;                //ストックのカウント
        KogeCountMax = 14f;             //焦げるスピード
        StockCountMax = 45f;            //ストックスピード
        kona = false;                   //konaをfalseに
        BredPowder = false;
        liquid = false;
        QuailFry = false;
        Secondliquid = false;
        ThirdBreadPowder = false;
        TaihiFlag = false;
        Burnflag = false;
        Burnflag2 = true;
        FastOneflag = false;

        //Sliderを満タンにする。
        slider.value = 1f;

        dummy = GameObject.Find("dummy");
        //りょうまが作ったやつ
        GM = GameObject.Find("GameManager");
        script = GM.GetComponent<GameManager>();

        //オーディオの情報取得
        sounds = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;    //Sin波
        //EnemyCanvasをMain Cameraに向かせる
        canvas.transform.rotation = Camera.main.transform.rotation;
        //Debug.Log("液"+liquid);
        //Debug.Log("粉"+BredPowder);
        //Debug.Log("揚"+QuailFry);
    }

    private void OnTriggerEnter(Collider other) {
        switch (gameobject.name) {
            case "Powder_Potato":
                //二度付け判定
                if (FastOneflag) {
                    if (other.gameObject.tag == "kona") {
                        Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");//Resourceフォルダのプレハブを読み込む
                    }
                }
                break;
            case "Powder_Fish":
                //二度付け判定
                if (FastOneflag) {
                    if (other.gameObject.tag == "kona") {
                        Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");//Resourceフォルダのプレハブを読み込む
                    }
                }
                break;
            case "Powder_Shrimp":
                //二度付け判定
                if (FastOneflag) {
                    if (other.gameObject.tag == "kona") {
                        Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");//Resourceフォルダのプレハブを読み込む
                    }
                }
                break;
            case "Powder_Quail1":
            case "Powder_Quail3":
                //二度付け判定
                if (FastOneflag) {
                    if (other.gameObject.tag == "Bread powder") {
                        Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                break;
            case "Powder_Quail2":
            case "Powder_Quail4":
                //二度付け判定
                if (FastOneflag) {
                    if (other.gameObject.tag == "liquid") {
                        Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                break;
        }
    }

    private void OnTriggerStay(Collider other) {
        switch (gameobject.name) {
            case "ItemKoge":
                if (other.gameObject.tag == "Garbage can") {
                    GameManager.instance.score_num -= 100;
                    Destroy(gameObject);
                }
                break;

            case "Item_Potato":
                if (other.gameObject.tag == "kona") {
                    Resource = (GameObject)Resources.Load("R_Resources/Powder_Potato");   //Resourceフォルダのプレハブを読み込む
                }
                //触れると焦げる
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");//皿
                }
                if (other.gameObject.tag == "karaagenabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");//鍋
                }
                if (other.gameObject.tag == "tenpuranabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");//鍋
                }
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");//ゴミ場
                break;

            case "Powder_Potato":
                if (script.FiverFlag) {
                    AgeCountMax = 1;
                }
                else {
                    AgeCountMax = 5;
                }
                if (other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;

                    if (AgeCount >= AgeCountMax) {
                        Resource = (GameObject)Resources.Load("R_Resources/Fried_K_Potato");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                //触れると焦げる
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");//皿
                }
                if (other.gameObject.tag == "karaagenabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");//唐揚げ鍋
                }
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");//ゴミ場

                FastOneflag = true;
                break;

            case "Fried_K_Potato":
                if (other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");
                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");   //Resourceフォルダのプレハブを読み込む
                }
                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("R_Resources/Dish_T_Potato");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "Dish_T_Potato":
                if (TaihiFlag == false) {
                    AgeCount = script.Taihi;
                    TaihiFlag = true;
                }

                //ストックされたら腐る
                if (other.gameObject.tag == "Stock") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax) {
                        Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Potato");

                break;

            case "Burn_Potato":
                Burnflag = true;
                if (other.gameObject.tag == "Garbage can") {
                    GameManager.instance.score_num -= 100;
                    Destroy(gameObject);
                }
                if (Burnflag && Burnflag2) {
                    //サウンド再生
                    sounds[0].Play();
                    sounds[1].Play();
                    Burnflag = false;
                    Burnflag2 = false;
                    if (!effectflag) {
                        //エフェクトスタート
                        Start_Effect();
                    }
                }
                break;

            case "Item_Fish":
                if (other.gameObject.tag == "kona") {
                    Resource = (GameObject)Resources.Load("R_Resources/Powder_Fish");   //Resourceフォルダのプレハブを読み込む
                }
                //触れると焦げる
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");//皿
                }
                if (other.gameObject.tag == "karaagenabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");//鍋
                }
                if (other.gameObject.tag == "tenpuranabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");//鍋
                }
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");//ゴミ場
                break;

            case "Powder_Fish":
                if (script.FiverFlag) {
                    AgeCountMax = 1;
                }
                else {
                    AgeCountMax = 5;
                }
                if (other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;

                    if (AgeCount >= AgeCountMax) {
                        Resource = (GameObject)Resources.Load("R_Resources/Fried_T_Fish");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                //皿に触れると焦げる
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");
                }
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");//ゴミ場

                FastOneflag = true;
                break;

            case "Fried_T_Fish":
                if (other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;

                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");
                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");   //Resourceフォルダのプレハブを読み込む
                }
                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("R_Resources/Dish_T_Fish");   //Resourceフォルダのプレハブを読み込む
                                                                                        //Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                }
                break;


            case "Dish_T_Fish":
                if (TaihiFlag == false) {
                    AgeCount = script.Taihi;
                    TaihiFlag = true;
                }

                //ストックされたら腐る
                if (other.gameObject.tag == "Stock") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax) {
                        Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Fish");
                break;


            case "Burn_Fish":
                Burnflag = true;
                if (other.gameObject.tag == "Garbage can") {
                    GameManager.instance.score_num -= 100;
                    Destroy(gameObject);
                }
                if (Burnflag && Burnflag2) {
                    //サウンド再生
                    sounds[0].Play();
                    sounds[1].Play();
                    Burnflag = false;
                    Burnflag2 = false;
                    if (!effectflag) {
                        //エフェクトスタート
                        Start_Effect();
                    }
                }
                break;

            case "Item_Shrimp":
                if (other.gameObject.tag == "kona") {
                    Resource = (GameObject)Resources.Load("R_Resources/Powder_Shrimp");   //Resourceフォルダのプレハブを読み込む
                }
                //触れると焦げる
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");//皿
                }
                if (other.gameObject.tag == "karaagenabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");//鍋
                }
                if (other.gameObject.tag == "tenpuranabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");//鍋
                }
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");//ゴミ場
                break;

            case "Powder_Shrimp":
                if (script.FiverFlag) {
                    AgeCountMax = 1;
                }
                else {
                    AgeCountMax = 5;
                }
                if (other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;

                    if (AgeCount >= AgeCountMax) {
                        Resource = (GameObject)Resources.Load("R_Resources/Fried_T_Shrimp");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                //皿に触れると焦げる
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");
                }
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");//ゴミ場

                FastOneflag = true;
                break;

            case "Fried_T_Shrimp":
                if (other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;

                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");
                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");   //Resourceフォルダのプレハブを読み込む
                }
                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("R_Resources/Dish_T_Shrimp");   //Resourceフォルダのプレハブを読み込む
                                                                                          //Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "Dish_T_Shrimp":
                if (TaihiFlag == false) {
                    AgeCount = script.Taihi;
                    TaihiFlag = true;
                }

                //ストックされたら腐る
                if (other.gameObject.tag == "Stock") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax) {
                        Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Shrimp");
                break;

            case "Burn_Shrimp":
                Burnflag = true;
                if (other.gameObject.tag == "Garbage can") {
                    GameManager.instance.score_num -= 100;
                    Destroy(gameObject);
                }
                if (Burnflag && Burnflag2) {
                    //サウンド再生
                    sounds[0].Play();
                    sounds[1].Play();
                    Burnflag = false;
                    Burnflag2 = false;
                    if (!effectflag) {
                        //エフェクトスタート
                        Start_Effect();
                    }
                }
                break;

            case "Item_Chicken":
                if (script.FiverFlag) {
                    AgeCountMax = 1;
                }
                else {
                    AgeCountMax = 5;
                }

                if (other.gameObject.tag == "karaagenabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;

                    if (AgeCount >= AgeCountMax) {
                        Resource = (GameObject)Resources.Load("R_Resources/Fried_K_Chicken");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                //触れると焦げる
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_chicken");//皿
                }
                if (other.gameObject.tag == "tenpuranabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_chicken");//鍋
                }
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_chicken");//ゴミ場
                break;


            case "Fried_K_Chicken":

                if (other.gameObject.tag == "karaagenabe") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                }
                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_chicken");   //Resourceフォルダのプレハブを読み込む
                }

                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("R_Resources/Dish_K_Chicken");   //Resourceフォルダのプレハブを読み込む
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_chicken");
                break;

            case "Dish_K_Chicken":

                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_chicken");

                if (other.gameObject.tag == "karaagenabe") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                }

                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_chicken");   //Resourceフォルダのプレハブを読み込む
                }

                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("R_Resources/Dish_K_Chicken");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "Burn_chicken":
                Burnflag = true;
                if (other.gameObject.tag == "Garbage can") {
                    GameManager.instance.score_num -= 100;
                    Destroy(gameObject);
                }
                if (Burnflag && Burnflag2) {
                    //サウンド再生
                    sounds[0].Play();
                    sounds[1].Play();
                    Burnflag = false;
                    Burnflag2 = false;
                    if (!effectflag) {
                        //エフェクトスタート
                        Start_Effect();
                    }
                }
                break;
            case "Item_Quail":
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }
                if (other.gameObject.tag == "liquid") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }
                if (other.gameObject.tag == "karaagenabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }

                if (other.gameObject.tag == "Bread powder") { //粉１回目
                    Resource = (GameObject)Resources.Load("R_Resources/Powder_Quail1");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "Powder_Quail1":
                if (other.gameObject.tag == "liquid") { //液1回目
                    Resource = (GameObject)Resources.Load("R_Resources/Powder_Quail2");   //Resourceフォルダのプレハブを読み込む
                }
                //ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }
                if (other.gameObject.tag == "karaagenabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }
                FastOneflag = true;
                break;

            case "Powder_Quail2":
                if (other.gameObject.tag == "Bread powder") {//粉２回目
                    Resource = (GameObject)Resources.Load("R_Resources/Powder_Quail3");   //Resourceフォルダのプレハブを読み込む
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }
                if (other.gameObject.tag == "karaagenabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }
                FastOneflag = true;
                break;

            case "Powder_Quail3":
                if (other.gameObject.tag == "liquid") { //液２回目
                    Resource = (GameObject)Resources.Load("R_Resources/Powder_Quail4");   //Resourceフォルダのプレハブを読み込む
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }
                if (other.gameObject.tag == "karaagenabe") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                }
                FastOneflag = true;
                break;

            case "Powder_Quail4":
                //揚がるスピード
                if (script.FiverFlag) {
                    AgeCountMax = 1;
                }
                else {
                    AgeCountMax = 2;
                }
                if (other.gameObject.tag == "karaagenabe") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;
                    if (AgeCount >= AgeCountMax) {
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("R_Resources/Fried_K_Quail");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                if (other.gameObject.tag == "Bread powder") {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");   //Resourceフォルダのプレハブを読み込む
                }
                FastOneflag = true;
                break;

            case "Fried_K_Quail":

                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");

                if (other.gameObject.tag == "karaagenabe") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                }

                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");   //Resourceフォルダのプレハブを読み込む
                }

                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("R_Resources/Dish_K_Quail");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "Dish_K_Quail":
                if (TaihiFlag == false) {
                    AgeCount = script.Taihi;
                    TaihiFlag = true;
                }

                //ストックされたら腐る
                if (other.gameObject.tag == "Stock") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax) {
                        Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("R_Resources/Burn_Quail");
                break;

            case "Burn_Quail":
                Burnflag = true;
                if (other.gameObject.tag == "Garbage can") {
                    GameManager.instance.score_num -= 100;
                    Destroy(gameObject);
                }
                if (Burnflag && Burnflag2) {
                    //サウンド再生
                    sounds[0].Play();
                    sounds[1].Play();
                    Burnflag = false;
                    Burnflag2 = false;
                    if (!effectflag) {
                        //エフェクトスタート
                        Start_Effect();
                    }
                }
                break;

            default:
                //case文にあるもの以外の場合
                break;


        }   //switc文終了
        if (Resource != null) {
            Destroy(gameObject);    //今あるオブジェクトを消す
            gameobject = (GameObject)Instantiate(Resource, this.gameObject.transform.position, this.gameObject.transform.rotation); //焼きあがった(焦げた)オブジェクト生成
        }
    }

    //エフェクトが生成、スタート
    void Start_Effect() {
        //Resourceフォルダのプレハブを読み込む
        obj_Burn = (GameObject)Resources.Load("Effects/E_Burn");
        //座標
        eff_pos = gameObject.transform.position;
        //角度
        eff_rot = Quaternion.identity;
        //生成
        eff_Burn = Instantiate(obj_Burn, new Vector3(eff_pos.x, eff_pos.y, eff_pos.z), eff_rot);

        //二度読み防止
        effectflag = true;
        End_Effect();
    }

    /// <summary>
    /// ///エフェクトエンド
    /// </summary>
    //エフェクトを停止消去
    void End_Effect() {
        Destroy(eff_Burn, 3f);
        //二度読み防止
        effectflag = false;
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Item_S : MonoBehaviour {

//    private float AgeCount;
//    private float StockCount;
//    private float AgeCountMax;
//    private float KogeCountMax;
//    private float StockCountMax;
//    private bool kona;
//    private bool BredPowder;
//    private bool liquid;
//    private bool QuailFry;
//    private bool Secondliquid;
//    private bool ThirdBreadPowder;
//    private bool TaihiFlag;
//    float alpha_Sin;    //オブジェクト発光の間隔(Sin波)

//    GameObject gameobject;
//    GameObject Resource;
//    GameObject objcolor;
//    GameObject dummy;
//    GameObject GM;              //GameMa agerがオブジェクト
//    GameManager script;         //GameManagerが入る変数

//    //Sliderを入れる
//    public Slider slider;
//    public Canvas canvas;

//    // Use this for initialization
//    void Start() {
//        gameobject = this.gameObject;   //このオブジェクトの情報をいれる
//        gameObject.name = gameObject.name.Replace("(Clone)", ""); //プレハブ生成時の(Clone)を消す
//        Resource = null;                //生成するプレハブの箱を初期化
//        //AgeCount = 0f;                  //カウント初期化
//        StockCount = 0f;                //ストックのカウント
//        KogeCountMax = 14f;             //焦げるスピード
//        StockCountMax = 45f;            //ストックスピード
//        kona = false;                   //konaをfalseに
//        BredPowder = false;
//        liquid = false;
//        QuailFry = false;
//        Secondliquid = false;
//        ThirdBreadPowder = false;
//        TaihiFlag = false;

//        //Sliderを満タンにする。
//        slider.value = 1f;

//        dummy = GameObject.Find("dummy");
//        //りょうまが作ったやつ
//        GM = GameObject.Find("GameManager");
//        script = GM.GetComponent<GameManager>();
//    }

//    // Update is called once per frame
//    void Update() {
//        alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;    //Sin波
//        //EnemyCanvasをMain Cameraに向かせる
//        canvas.transform.rotation = Camera.main.transform.rotation;
//        //Debug.Log("液"+liquid);
//        //Debug.Log("粉"+BredPowder);
//        //Debug.Log("揚"+QuailFry);
//    }


//    private void OnTriggerStay(Collider other) {

//        switch (gameobject.name) {
//            case "ItemKoge":
//                if (other.gameObject.tag == "Garbage can") {
//                    GameManager.instance.score_num -= 100;
//                    Destroy(gameObject);
//                }
//                break;
//            case "ItemPotato":
//                if (script.FiverFlag) {
//                    AgeCountMax = 1;
//                }
//                else {
//                    AgeCountMax = 5;
//                }
//                if (kona == false) {
//                    objcolor = GameObject.Find("kona");
//                }

//                if (other.gameObject.tag == "kona") {
//                    kona = true;
//                    objcolor = GameObject.Find("tenpuranabe");
//                }
//                else if (kona == true && other.gameObject.tag == "tenpuranabe") {

//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / AgeCountMax;

//                    if (AgeCount >= AgeCountMax) {
//                        objcolor = dummy;
//                        Resource = (GameObject)Resources.Load("S_Resources/ItemTenpura");   //Resourceフォルダのプレハブを読み込む
//                    }
//                }
//                else if (other.gameObject.tag != "Click") {
//                    objcolor = dummy;
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                }

//                break;
//            case "ItemFish":
//            case "ItemEbi":
//                if (script.FiverFlag) {
//                    AgeCountMax = 1;
//                }
//                else {
//                    AgeCountMax = 6;
//                }
//                if (kona == false) {
//                    objcolor = GameObject.Find("kona");
//                }


//                if (other.gameObject.tag == "kona") {
//                    kona = true;
//                    objcolor = GameObject.Find("tenpuranabe");
//                }
//                else if (kona == true && other.gameObject.tag == "tenpuranabe") {

//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / AgeCountMax;

//                    if (AgeCount >= AgeCountMax) {
//                        objcolor = dummy;
//                        Resource = (GameObject)Resources.Load("S_Resources/ItemTenpura");   //Resourceフォルダのプレハブを読み込む
//                    }
//                }
//                else if (other.gameObject.tag != "Click") {
//                    objcolor = dummy;
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                }



//                break;

//            case "ItemTenpura":

//                if (other.gameObject.tag == "tenpuranabe") {

//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / KogeCountMax;
//                    Debug.Log(slider.value);

//                }
//                // ゴミ箱に当たると焦げになる
//                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
//                if (AgeCount >= KogeCountMax) {
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                }
//                if (other.gameObject.tag == "Sara") {
//                    script.Taihi = AgeCount;
//                    script.TaihiFlag = TaihiFlag;
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
//                }

//                //if (other.gameObject.tag == "tenpuranabe") {

//                //    AgeCount += Time.deltaTime;
//                //    slider.value = AgeCount / KogeCountMax;
//                //    Debug.Log(slider.value);
//                //    if (AgeCount >= KogeCountMax) {
//                //        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                //    }
//                //}
//                //if (other.gameObject.tag == "Sara") {
//                //    print("ダイジョウブ");
//                //    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
//                //}

//                break;

//            case "ItemSara(Tenpura)":
//                if (TaihiFlag == false) {
//                    print("通った");
//                    AgeCount = script.Taihi;
//                    TaihiFlag = true;
//                }

//                //ストックされたら腐る
//                if (other.gameObject.tag == "Stock") {
//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / StockCountMax;
//                    if (AgeCount >= StockCountMax) {
//                        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                    }
//                }
//                // ゴミ箱に当たると焦げになる
//                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");

//                break;

//            case "ItemChicken":
//                if (script.FiverFlag) {
//                    AgeCountMax = 1;
//                }
//                else {
//                    AgeCountMax = 5;
//                }
//                objcolor = GameObject.Find("karaagenabe");

//                if (other.gameObject.tag == "karaagenabe") {

//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / AgeCountMax;
//                    Debug.Log("slider.value : " + slider.value);

//                    if (AgeCount >= AgeCountMax) {
//                        objcolor = dummy;
//                        Resource = (GameObject)Resources.Load("S_Resources/ItemFriedchicken");   //Resourceフォルダのプレハブを読み込む
//                    }
//                }
//                else if (other.gameObject.tag != "Click") {
//                    objcolor = dummy;
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                }
//                break;


//            case "ItemFriedchicken":

//                AgeCount += Time.deltaTime;
//                slider.value = AgeCount / KogeCountMax;

//                if (other.gameObject.tag == "karaagenabe") {
//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / KogeCountMax;
//                }
//                if (AgeCount >= KogeCountMax) {
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                }
//                if (other.gameObject.tag == "Sara") {
//                    script.Taihi = AgeCount;
//                    script.TaihiFlag = TaihiFlag;
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
//                }

//                //    if (other.gameObject.tag == "karaagenabe") {
//                //        AgeCount += Time.deltaTime;
//                //        slider.value = AgeCount / KogeCountMax;
//                //        if (AgeCount >= KogeCountMax) {
//                //            Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                //        }
//                //    }

//                if (other.gameObject.tag == "Sara") {
//                    script.Taihi = AgeCount;
//                    script.TaihiFlag = TaihiFlag;
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Chicken)");   //Resourceフォルダのプレハブを読み込む
//                }
//                // ゴミ箱に当たると焦げになる
//                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
//                break;

//            case "ItemSara(Chicken)":

//                if (TaihiFlag == false) {
//                    AgeCount = script.Taihi;
//                    TaihiFlag = true;
//                }

//                //ストックされたら
//                if (other.gameObject.tag == "Stock") {
//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / StockCountMax;
//                    if (AgeCount >= StockCountMax) {
//                        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                    }
//                }
//                // ゴミ箱に当たると焦げになる
//                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
//                break;

//            case "ItemQuail":
//                //揚がるスピード
//                if (script.FiverFlag) {
//                    AgeCountMax = 1;
//                }
//                else {
//                    AgeCountMax = 2;
//                }
//                if (BredPowder == false) {
//                    objcolor = GameObject.Find("BreadPowder");
//                }
//                // ゴミ箱に当たると焦げになる
//                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
//                if (QuailFry == true && other.gameObject.tag == "karaagenabe") {

//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / AgeCountMax;
//                    if (AgeCount >= AgeCountMax) {
//                        objcolor = dummy;
//                        Resource = (GameObject)Resources.Load("S_Resources/ItemQuailFry");   //Resourceフォルダのプレハブを読み込む
//                    }
//                }
//                else if (other.gameObject.tag == "Bread powder" && ThirdBreadPowder == true) {
//                    objcolor = dummy;
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                }
//                else if (other.gameObject.tag == "liquid" && Secondliquid == true)  //液２回目
//                {
//                    ThirdBreadPowder = true;
//                    QuailFry = true;
//                    objcolor = GameObject.Find("karaagenabe");
//                }
//                else if (other.gameObject.tag == "Bread powder" && BredPowder == true && liquid == true)//粉２回目
//                {
//                    Secondliquid = true;
//                    objcolor = GameObject.Find("liquid");
//                }
//                else if (other.gameObject.tag == "Bread powder") //粉１回目
//                {
//                    BredPowder = true;
//                    objcolor = GameObject.Find("liquid");
//                }
//                else if (BredPowder == true && other.gameObject.tag == "liquid") //液１回目
//                {
//                    //BredPowder = false;
//                    liquid = true;
//                }
//                else if (other.gameObject.tag != "Click") //OnCollisionStayだと1液が処理された瞬間呼ばれてしまう
//                {
//                    objcolor = dummy;
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                }
//                break;

//            case "ItemQuailFry":

//                AgeCount += Time.deltaTime;
//                slider.value = AgeCount / KogeCountMax;

//                // ゴミ箱に当たると焦げになる
//                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");

//                if (other.gameObject.tag == "karaagenabe") {
//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / KogeCountMax;
//                }

//                if (AgeCount >= KogeCountMax) {
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                }

//                //if (other.gameObject.tag == "karaagenabe") {
//                //    AgeCount += Time.deltaTime;
//                //    slider.value = AgeCount / KogeCountMax;
//                //    if (AgeCount >= KogeCountMax) {
//                //        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                //    }
//                //}

//                if (other.gameObject.tag == "Sara") {
//                    script.Taihi = AgeCount;
//                    script.TaihiFlag = TaihiFlag;
//                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Quail)");   //Resourceフォルダのプレハブを読み込む
//                }

//                break;

//            case "ItemSara(Quail)":

//                if (TaihiFlag == false) {
//                    AgeCount = script.Taihi;
//                    TaihiFlag = true;
//                }

//                //ストックされたら
//                if (other.gameObject.tag == "Stock") {
//                    AgeCount += Time.deltaTime;
//                    slider.value = AgeCount / StockCountMax;
//                    if (AgeCount >= StockCountMax) {
//                        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
//                    }
//                }
//                // ゴミ箱に当たると焦げになる
//                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
//                break;

//            default:
//                //case文にあるもの以外の場合
//                break;


//        }   //switc文終了
//        if (Resource != null) {
//            Destroy(gameObject);    //今あるオブジェクトを消す
//            gameobject = (GameObject)Instantiate(Resource, this.gameObject.transform.position, this.gameObject.transform.rotation); //焼きあがった(焦げた)オブジェクト生成
//        }
//    }
//}