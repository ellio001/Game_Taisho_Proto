using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Ryoma: MonoBehaviour {

    private float AgeCount;         //現在の揚げる時間
    private float AgeCountMax;      //揚げ揚がる時間
    private float KogeCountMax;     //揚げ焦げる時間
    private float StockCountMax;    //置き腐れる時間
    private bool kona;              //粉判定
    private bool BredPowder;        //うずら判定
    private bool liquid;            //うずら判定
    private bool QuailFry;          //うずら判定
    private bool Secondliquid;      //うずら判定
    private bool ThirdBreadPowder;  //うずら判定
    private bool TaihiFlag;         //AgeCountを維持する判定
    float alpha_Sin;                //オブジェクト発光の間隔(Sin波)

    GameObject gameobject;
    GameObject Resource;
    GameObject objcolor;
    GameObject GM;              //GameMa agerがオブジェクト
    GameManager script;         //GameManagerが入る変数

    //Sliderを入れる
    public Slider slider;
    public Canvas canvas;

    // Use this for initialization
    void Start() {
        gameobject = this.gameObject;   //このオブジェクトの情報をいれる
        gameObject.name = gameObject.name.Replace("(Clone)", ""); //プレハブ生成時の(Clone)を消す
        Resource = null;                //生成するプレハブの箱を初期化
        //AgeCount = 0f;                  //カウント初期化
        KogeCountMax = 14f;             //焦げるスピード
        StockCountMax = 45f;            //ストックスピード
        kona = false;                   //konaをfalseに
        BredPowder = false;
        liquid = false;
        QuailFry = false;
        Secondliquid = false;
        ThirdBreadPowder = false;
        TaihiFlag = false;

        //Sliderを満タンにする。
        slider.value = 1f;
        
        //りょうまが作ったやつ
        GM = GameObject.Find("GameManager");
        script = GM.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;    //Sin波
        //EnemyCanvasをMain Cameraに向かせる
        canvas.transform.rotation = Camera.main.transform.rotation;
    }


    private void OnTriggerStay(Collider other) {

        switch (gameobject.name) {
            case "ItemKoge":
                if (other.gameObject.tag == "Garbage can") {
                    GameManager.instance.score_num -= 100;
                    Destroy(gameObject);
                }
                break;
            case "ItemPotato":
                if (script.FiverFlag) {
                    AgeCountMax = 1;
                }
                else {
                    AgeCountMax = 5;
                }
                if (kona == false) {
                    objcolor = GameObject.Find("kona");
                }

                if (other.gameObject.tag == "kona") {
                    kona = true;
                    objcolor = GameObject.Find("tenpuranabe");
                }
                else if (kona == true && other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;

                    if (AgeCount >= AgeCountMax) {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemTenpura");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click") {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }

                break;
            case "ItemFish":
            case "ItemEbi":
                if (script.FiverFlag) {
                    AgeCountMax = 1;
                }
                else {
                    AgeCountMax = 6;
                }
                if (kona == false) {
                    objcolor = GameObject.Find("kona");
                }


                if (other.gameObject.tag == "kona") {
                    kona = true;
                    objcolor = GameObject.Find("tenpuranabe");
                }
                else if (kona == true && other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;

                    if (AgeCount >= AgeCountMax) {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemTenpura");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click") {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }



                break;

            case "ItemTenpura":

                if (other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                    Debug.Log(slider.value);

                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "ItemSara(Tenpura)":
                if (TaihiFlag == false) {
                    print("通った");
                    AgeCount = script.Taihi;
                    TaihiFlag = true;
                }

                //ストックされたら腐る
                if (other.gameObject.tag == "Stock") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax) {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");

                break;

            case "ItemChicken":
                if (script.FiverFlag) {
                    AgeCountMax = 1;
                }
                else {
                    AgeCountMax = 5;
                }
                objcolor = GameObject.Find("karaagenabe");

                if (other.gameObject.tag == "karaagenabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;
                    Debug.Log("slider.value : " + slider.value);

                    if (AgeCount >= AgeCountMax) {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemFriedchicken");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click") {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                break;


            case "ItemFriedchicken":

                AgeCount += Time.deltaTime;
                slider.value = AgeCount / KogeCountMax;

                if (other.gameObject.tag == "karaagenabe") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                }
                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                }
                
                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Chicken)");   //Resourceフォルダのプレハブを読み込む
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
                break;

            case "ItemSara(Chicken)":

                if (TaihiFlag == false) {
                    AgeCount = script.Taihi;
                    TaihiFlag = true;
                }

                //ストックされたら
                if (other.gameObject.tag == "Stock") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax) {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
                break;

            case "ItemQuail":
                //揚がるスピード
                if (script.FiverFlag) {
                    AgeCountMax = 1;
                }
                else {
                    AgeCountMax = 2;
                }
                if (BredPowder == false) {
                    objcolor = GameObject.Find("BreadPowder");
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
                if (QuailFry == true && other.gameObject.tag == "karaagenabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;
                    if (AgeCount >= AgeCountMax) {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemQuailFry");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag == "Bread powder" && ThirdBreadPowder == true) {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                else if (other.gameObject.tag == "liquid" && Secondliquid == true)  //液２回目
                {
                    ThirdBreadPowder = true;
                    QuailFry = true;
                    objcolor = GameObject.Find("karaagenabe");
                }
                else if (other.gameObject.tag == "Bread powder" && BredPowder == true && liquid == true)//粉２回目
                {
                    Secondliquid = true;
                    objcolor = GameObject.Find("liquid");
                }
                else if (other.gameObject.tag == "Bread powder") //粉１回目
                {
                    BredPowder = true;
                    objcolor = GameObject.Find("liquid");
                }
                else if (BredPowder == true && other.gameObject.tag == "liquid") //液１回目
                {
                    liquid = true;
                    objcolor = GameObject.Find("BreadPowder");
                }
                else if (other.gameObject.tag != "Click") //OnCollisionStayだと1液が処理された瞬間呼ばれてしまう
                {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "ItemQuailFry":

                AgeCount += Time.deltaTime;
                slider.value = AgeCount / KogeCountMax;

                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");

                if (other.gameObject.tag == "karaagenabe") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                }

                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }

                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Quail)");   //Resourceフォルダのプレハブを読み込む
                }

                break;

            case "ItemSara(Quail)":

                if (TaihiFlag == false) {
                    AgeCount = script.Taihi;
                    TaihiFlag = true;
                }

                //ストックされたら
                if (other.gameObject.tag == "Stock") {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax) {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
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
}