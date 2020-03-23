using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    private int AgeCount;
    private int AgeCountMax;
    private int KogeCountMax;
    private int StockCountMax;
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

    // Use this for initialization
    void Start() {
        gameobject = this.gameObject;   //このオブジェクトの情報をいれる
        gameObject.name = gameObject.name.Replace("(Clone)", ""); //プレハブ生成時の(Clone)を消す
        Resource = null;            //生成するプレハブの箱を初期化
        AgeCount = 0;               //カウント初期化
        AgeCountMax = 120;          //揚がるスピード
        KogeCountMax = 1200;        //焦げるスピード
        StockCountMax = 3000;       //ストックスピード
        kona = false;               //konaをfalseに
        BredPowder = false;
        liquid = false;
        QuailFry = false;
        Secondliquid = false;
        ThirdBreadPowder = false;
        dummy = GameObject.Find("dummy");

    }

    // Update is called once per frame
    void Update() {
        alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;    //Sin波
        //Debug.Log("液"+liquid);
        //Debug.Log("粉"+BredPowder);
        //Debug.Log("揚"+QuailFry);
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
            case "ItemFish":
            case "ItemEbi":
                if (kona == false) {
                    objcolor = GameObject.Find("kona");
                }

                if (other.gameObject.tag == "kona") {
                    kona = true;
                    GetComponent<Renderer>().material.color = Color.white;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("tenpuranabe");
                }
                else if (kona == true && other.gameObject.tag == "tenpuranabe") {

                    AgeCount++;

                    if (AgeCount >= AgeCountMax) {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("ItemTenpura");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click") {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);

                break;

            case "ItemTenpura":
                if (other.gameObject.tag == "tenpuranabe") AgeCount++;

                //ストックされたら腐る
                //if (other.gameObject.tag == "Stock") AgeCount++;

                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                }

                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "ItemSara(Tenpura)":
                //ストックされたら腐る
                if (other.gameObject.tag == "Stock") AgeCount++;

                if (AgeCount >= StockCountMax) {
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "ItemChicken":

                objcolor = GameObject.Find("karaagenabe");

                if (other.gameObject.tag == "karaagenabe") {
                    AgeCount++;

                    if (AgeCount >= AgeCountMax) {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("ItemFriedchicken");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click") {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);
                break;


            case "ItemFriedchicken":
                if (other.gameObject.tag == "karaagenabe") AgeCount++;

                //ストックされたら
                //if (other.gameObject.tag == "Stock") AgeCount++;

                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("ItemSara(Chicken)");   //Resourceフォルダのプレハブを読み込む
                }

                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "ItemSara(Chicken)":

                //ストックされたら
                if (other.gameObject.tag == "Stock") AgeCount++;

                if (AgeCount >= StockCountMax) {
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "ItemQuail":
                if (BredPowder == false) {
                    objcolor = GameObject.Find("BreadPowder");
                }
                if (QuailFry == true && other.gameObject.tag == "karaagenabe") {
                    AgeCount++;

                    if (AgeCount >= AgeCountMax) {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("ItemQuailFry");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag == "Bread powder" && ThirdBreadPowder == true) {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
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
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);
                break;

            case "ItemQuailFry":

                if (other.gameObject.tag == "karaagenabe") AgeCount++;

                //ストックされたら
                //if (other.gameObject.tag == "Stock") AgeCount++;

                if (other.gameObject.tag == "Sara") {
                    Resource = (GameObject)Resources.Load("ItemSara(Quail)");   //Resourceフォルダのプレハブを読み込む
                }

                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "ItemSara(Quail)":

                //ストックされたら
                if (other.gameObject.tag == "Stock") AgeCount++;

                if (AgeCount >= StockCountMax) {
                    Resource = (GameObject)Resources.Load("ItemKoge");   //Resourceフォルダのプレハブを読み込む
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
}