﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Item : MonoBehaviour
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
    private bool TaihiFlag;
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

        //Sliderを満タンにする。
        slider.value = 1f;

        dummy = GameObject.Find("dummy");
        //りょうまが作ったやつ
        GM = GameObject.Find("GameManager");
        script = GM.GetComponent<GameManager>();
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


    private void OnTriggerStay(Collider other) {

        switch (gameobject.name) {
            case "ItemKoge":
                if (other.gameObject.tag == "Garbage can") {
                    GameManager.instance.score_num -= 100;
                    Destroy(gameObject);
                }
                break;
            case "Tutorial_ItemEbi":
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
                    GetComponent<Renderer>().material.color = Color.white;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("tenpuranabe");
                }
                else if (kona == true && other.gameObject.tag == "tenpuranabe") {

                    AgeCount += Time.deltaTime;
                    slider.value = AgeCount / AgeCountMax;

                    if (AgeCount >= AgeCountMax) {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("S_Resources/Tutorial_ItemTenpura");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click") {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }


                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);

                break;

            case "Tutorial_ItemTenpura":

                //if (other.gameObject.tag == "tenpuranabe") {
                //    AgeCount += Time.deltaTime;
                //    slider.value = AgeCount / KogeCountMax;
                //    Debug.Log(slider.value);
                //}
                // ゴミ箱に当たると焦げになる
                if (other.gameObject.tag == "Garbage can") Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");
                if (AgeCount >= KogeCountMax) {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                if (other.gameObject.tag == "Sara") {
                    script.Taihi = AgeCount;
                    script.TaihiFlag = TaihiFlag;
                    Resource = (GameObject)Resources.Load("S_Resources/Tutorial_ItemSara(Tenpura)");   //Resourceフォルダのプレハブを読み込む
                }
                break;

            case "Tutorial_ItemSara(Tenpura)":
                if (TaihiFlag == false) {
                    AgeCount = script.Taihi;
                    TaihiFlag = true;
                }

                //ストックされたら腐る
                if (other.gameObject.tag == "Stock") {
                    //AgeCount += Time.deltaTime;
                    //slider.value = AgeCount / StockCountMax;
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
