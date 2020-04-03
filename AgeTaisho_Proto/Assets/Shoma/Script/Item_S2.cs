using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_S2 : MonoBehaviour
{

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
    float alpha_Sin;    //オブジェクト発光の間隔(Sin波)

    GameObject gameobject;
    GameObject Resource;
    GameObject objcolor;
    GameObject dummy;
    GameObject GM;              //GameMa agerがオブジェクト
    GameManager script;         //GameManagerが入る変数

    //Sliderを入れる
    public Slider slider;
    public Canvas canvas;

    // Use this for initialization
    void Start()
    {
        gameobject = this.gameObject;   //このオブジェクトの情報をいれる
        gameObject.name = gameObject.name.Replace("(Clone)", ""); //プレハブ生成時の(Clone)を消す
        Resource = null;            //生成するプレハブの箱を初期化
        AgeCount = 0f;               //カウント初期化
        StockCount = 0f;     //ストックのカウント
        KogeCountMax = 14f;        //焦げるスピード
        StockCountMax = 30f;       //ストックスピード
        kona = false;               //konaをfalseに
        BredPowder = false;
        liquid = false;
        QuailFry = false;
        Secondliquid = false;
        ThirdBreadPowder = false;

        //Sliderを満タンにする。
        slider.value = 1f;

        dummy = GameObject.Find("dummy");
        //りょうまが作ったやつ
        GM = GameObject.Find("GameManager");
        script = GM.GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;    //Sin波
        //EnemyCanvasをMain Cameraに向かせる
        canvas.transform.rotation = Camera.main.transform.rotation;
        //Debug.Log("液"+liquid);
        //Debug.Log("粉"+BredPowder);
        //Debug.Log("揚"+QuailFry);
    }

    private void OnTriggerStay(Collider other)
    {

        switch (gameobject.name)
        {
            case "ItemKoge":
                if (other.gameObject.tag == "Garbage can")
                {
                    GameManager.instance.score_num -= 100;
                    Destroy(gameObject);
                }
                break;
            case "Item_Potato":
                if (script.FiverFlag)
                {
                    AgeCountMax = 1;
                }
                else
                {
                    AgeCountMax = 5;
                }
                if (kona == false)
                {
                    objcolor = GameObject.Find("kona");
                }

                if (other.gameObject.tag == "kona")
                {
                    kona = true;
                    GetComponent<Renderer>().material.color = Color.white;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("tenpuranabe");
                }
                else if (kona == true && other.gameObject.tag == "tenpuranabe")
                {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;

                    if (AgeCount >= AgeCountMax)
                    {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("S_Resources/ItemTenpura");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click")
                {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);

                break;
            case "ItemFish":
            case "ItemEbi":
                if (script.FiverFlag)
                {
                    AgeCountMax = 1;
                }
                else
                {
                    AgeCountMax = 6;
                }
                if (kona == false)
                {
                    objcolor = GameObject.Find("kona");
                }


                if (other.gameObject.tag == "kona")
                {
                    kona = true;
                    GetComponent<Renderer>().material.color = Color.white;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("tenpuranabe");
                }
                else if (kona == true && other.gameObject.tag == "tenpuranabe")
                {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;

                    if (AgeCount >= AgeCountMax)
                    {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("S_Resources/ItemTenpura");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click")
                {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }


                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);

                break;

            case "ItemTenpura":

                AgeCount += Time.deltaTime;
                slider.value = AgeCount / KogeCountMax;

                if (other.gameObject.tag == "tenpuranabe")
                {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                    Debug.Log(slider.value);
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
                if (AgeCount >= KogeCountMax)
                {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                if (other.gameObject.tag == "Sara")
                {
                    print("ダイジョウブ");
                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                }

                //if (other.gameObject.tag == "tenpuranabe") {

                //    AgeCount += Time.deltaTime;
                //    slider.value = AgeCount / KogeCountMax;
                //    Debug.Log(slider.value);
                //    if (AgeCount >= KogeCountMax) {
                //        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                //    }
                //}
                //if (other.gameObject.tag == "Sara") {
                //    print("ダイジョウブ");
                //    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                //}

                break;

            case "ItemSara(Tenpura)":
                //ストックされたら腐る
                if (other.gameObject.tag == "Stock")
                {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax)
                    {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");

                break;

            case "ItemChicken":
                if (script.FiverFlag)
                {
                    AgeCountMax = 1;
                }
                else
                {
                    AgeCountMax = 5;
                }
                objcolor = GameObject.Find("karaagenabe");

                if (other.gameObject.tag == "karaagenabe")
                {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;
                    Debug.Log("slider.value : " + slider.value);

                    if (AgeCount >= AgeCountMax)
                    {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("S_Resources/ItemFriedchicken");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click")
                {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);
                break;


            case "ItemFriedchicken":

                AgeCount += Time.deltaTime;
                slider.value = AgeCount / KogeCountMax;

                if (other.gameObject.tag == "karaagenabe")
                {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                }
                if (AgeCount >= KogeCountMax)
                {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                if (other.gameObject.tag == "Sara")
                {
                    print("ダイジョウブ");
                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                }

                //    if (other.gameObject.tag == "karaagenabe") {
                //        AgeCount += Time.deltaTime;
                //        slider.value = AgeCount / KogeCountMax;
                //        if (AgeCount >= KogeCountMax) {
                //            Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                //        }
                //    }

                if (other.gameObject.tag == "Sara")
                {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Chicken)");   //Resourceフォルダのプレハブを読み込む
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
                break;

            case "ItemSara(Chicken)":

                //ストックされたら
                if (other.gameObject.tag == "Stock")
                {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax)
                    {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
                break;

            case "ItemQuail":
                //揚がるスピード
                if (script.FiverFlag)
                {
                    AgeCountMax = 1;
                }
                else
                {
                    AgeCountMax = 2;
                }
                if (BredPowder == false)
                {
                    objcolor = GameObject.Find("BreadPowder");
                }
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
                if (QuailFry == true && other.gameObject.tag == "karaagenabe")
                {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;
                    if (AgeCount >= AgeCountMax)
                    {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("S_Resources/ItemQuailFry");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag == "Bread powder" && ThirdBreadPowder == true)
                {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                else if (other.gameObject.tag == "liquid" && Secondliquid == true)  //液２回目
                {
                    ThirdBreadPowder = true;
                    QuailFry = true;
                    GetComponent<Renderer>().material.color = Color.blue;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("karaagenabe");
                }
                else if (other.gameObject.tag == "Bread powder" && BredPowder == true && liquid == true)//粉２回目
                {
                    Secondliquid = true;
                    GetComponent<Renderer>().material.color = Color.red;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("liquid");
                }
                else if (other.gameObject.tag == "Bread powder") //粉１回目
                {
                    BredPowder = true;
                    GetComponent<Renderer>().material.color = Color.red;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("liquid");
                }
                else if (BredPowder == true && other.gameObject.tag == "liquid") //液１回目
                {
                    //BredPowder = false;
                    liquid = true;
                    GetComponent<Renderer>().material.color = Color.blue;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("BreadPowder");
                }
                else if (other.gameObject.tag != "Click") //OnCollisionStayだと1液が処理された瞬間呼ばれてしまう
                {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);
                break;

            case "ItemQuailFry":

                AgeCount += Time.deltaTime;
                slider.value = AgeCount / KogeCountMax;

                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");

                if (other.gameObject.tag == "karaagenabe")
                {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / KogeCountMax;
                }

                if (AgeCount >= KogeCountMax)
                {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }

                if (other.gameObject.tag == "Sara")
                {
                    print("ダイジョウブ");
                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                }

                //if (other.gameObject.tag == "karaagenabe") {
                //    AgeCount += Time.deltaTime;
                //    slider.value = AgeCount / KogeCountMax;
                //    if (AgeCount >= KogeCountMax) {
                //        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                //    }
                //}

                if (other.gameObject.tag == "Sara")
                {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemSara(Quail)");   //Resourceフォルダのプレハブを読み込む
                }

                break;

            case "ItemSara(Quail)":

                //ストックされたら
                if (other.gameObject.tag == "Stock")
                {
                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / StockCountMax;
                    if (AgeCount >= StockCountMax)
                    {
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
        if (Resource != null)
        {
            Destroy(gameObject);    //今あるオブジェクトを消す
            gameobject = (GameObject)Instantiate(Resource, this.gameObject.transform.position, this.gameObject.transform.rotation); //焼きあがった(焦げた)オブジェクト生成
        }
    }
}