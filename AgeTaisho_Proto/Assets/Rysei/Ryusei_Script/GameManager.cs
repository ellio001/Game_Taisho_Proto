﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {


    public float GameTime;             //ゲーム開始の時間
    float GameFinishTime;       //ゲームのプレイ最大時間
    public float FiverTime;            //フィーバーの時間です
    float FiverEvacuation;      //フィーバーの退避エリア
    float FiverFinishTime;      //フィーバータイム最大時間
    int FiverNumber;            //スイッチ文で使うフィーバーフラグ
    int FiverCountFlag = 0;     //何回目のフィーバーか判定フラグ
    public bool FiverFlag;             //フィーバーかどうかの判定フラグ
    bool TestSceneFlag;         //シーンを飛ぶ用のフラグ
    public float Taihi;
    public bool TaihiFlag;


    public static GameManager instance = null;
    public int score_num = 0; // スコア変数
    public GameObject score_object = null; // Textオブジェクト
    public GameObject Pause_object = null;

    // プレファブ達をリスト化

    [SerializeField] List<GameObject> Item_Resources = new List<GameObject>();
    [SerializeField] List<GameObject> Powder_Resources = new List<GameObject>();
    [SerializeField] List<GameObject> Fried_Resources = new List<GameObject>();
    [SerializeField] List<GameObject> Dish_Resources = new List<GameObject>();

    private void Awake()    //スタートよりも最初に呼ばれる
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        GameFinishTime = 180f;
        FiverFinishTime = 30f;
        FiverNumber = 3;
        FiverFlag = false;
        TestSceneFlag = true;
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void Update() {

        GameTime += Time.deltaTime;

        if (TestSceneFlag) {
            // オブジェクトからTextコンポーネントを取得
            Text score_text = score_object.GetComponent<Text>();
            Text Pause_text = Pause_object.GetComponent<Text>();
            // テキストの表示を入れ替える
            score_text.text = "Score:" + score_num;
            Pause_text.text = "Pキー：ポーズ";

            //判定
            Judgment();
        }
    }

    public void Addition() {
        //ゲーム時間を加算
        //GameTime = Time.deltaTime;
        //Debug.Log("GameTime:" + GameTime);
    }

    public void Judgment() {
        //ゲーム時間を判定
        if (GameTime >= GameFinishTime) {
            Debug.Log("ゲーム終了！");
            //Scene読込
            ReadScene();
        }


        if (FiverFlag == false) {
            //何回目のフィーバーか判定
            switch (FiverCountFlag) {
                case 0:
                    //1回目のフィーバー
                    if (GameTime >= GameFinishTime - 120f) {
                        print("1回目のフィーバー");
                        //
                        FiverNumber = 0;
                        FiverFlag = true;
                        Initial();
                        FiverCountFlag++;
                    }
                    break;
                case 1:
                    //２回目のフィーバー
                    if (GameTime >= GameFinishTime - 60f) {
                        print("2回目のフィーバー");
                        FiverNumber = 1;
                        FiverFlag = true;
                        Initial();
                        FiverCountFlag++;
                    }
                    break;
            }

        }
        switch (FiverNumber) {
            //1回目のフィーバー
            case 0:
                if (FiverFlag == false) {
                    //変数初期化
                    Initial();
                }
                if (FiverFlag) {
                    FiverTime += Time.deltaTime;
                    //フィーバータイム終了
                    if (FiverTime >= FiverEvacuation) {
                        FiverFlag = false;
                        FiverNumber = 3;
                        Debug.Log("フィーバータイム終了！");
                    }
                }
                break;
            //２回目のフィーバー
            case 1:
                if (FiverFlag == false) {
                    //変数初期化
                    Initial();
                }
                if (FiverFlag) {
                    FiverTime += Time.deltaTime;
                    //フィーバータイム終了
                    if (FiverTime >= FiverEvacuation) {
                        FiverFlag = false;
                        FiverNumber = 3;
                        Debug.Log("フィーバータイム終了！");
                    }
                }
                break;
            //退避エリア(なにも入れない)
            case 3:
                break;
        }
    }

    //変数を初期化するための関数
    public void Initial() {
        FiverTime = Time.deltaTime;
        FiverEvacuation = FiverTime + FiverFinishTime;
        FiverFlag = true;
    }

    public void ReadScene() {
        //Endシーン読込
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
        //処理を二度としないようにフラグで管理
        TestSceneFlag = false;
    }

    void ActiveSceneChanged(Scene thisScene, Scene nextScene) {
        score_object = GameObject.Find("ScoreText"); // Textオブジェクト
        Pause_object = GameObject.Find("PouseUIText");
        if (score_object != null || Pause_object != null)
        {
            Text score_text = score_object.GetComponent<Text>();// オブジェクトからTextコンポーネントを取得
            Text Pause_text = Pause_object.GetComponent<Text>();
        }
    }
}