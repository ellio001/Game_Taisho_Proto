using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public float GameTime;             //ゲーム開始の時間
    float GameFinishTime;       //ゲームのプレイ最大時間
    float FiverTime;            //フィーバーの時間です
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

    bool Bad_Score;     //Bad_Scoreをいれる箱
    bool Normal_Score;  //Nomal_Score1をいれる箱
    bool Good_Score;    //Good_Scoreをいれる箱
                        //public Slider slider;    //Sliderを入れる


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
        GameTime = 0f;

        Bad_Score = score_num < 1000;                           //スコア1000未満でBad_Score
        Normal_Score = score_num >= 1000 && score_num < 2000;   //スコア1000以上2000未満でNormal_Score
        Good_Score = score_num >= 2000;                         //スコア2000以上でGood_Score

    }

    private void Update() {

        if (SceneManager.GetActiveScene().name == "Easy_Scene" ||
            SceneManager.GetActiveScene().name == "Normal_Scene" ||
            SceneManager.GetActiveScene().name == "Hard_Scene") { // hogehogeシーンでのみやりたい処理
                                                                  //現在までのフレーム
            GameTime += Time.deltaTime;

            // オブジェクトからTextコンポーネントを取得
            Text score_text = score_object.GetComponent<Text>();
            Text Pause_text = Pause_object.GetComponent<Text>();
            // テキストの表示を入れ替える
            score_text.text = "得点:" + score_num;
            Pause_text.text = "STARTボタン：ポーズ";

            //判定
            Judgment();
        }
        else if (SceneManager.GetActiveScene().name != "Easy_Scene" ||
            SceneManager.GetActiveScene().name != "Normal_Scene" ||
            SceneManager.GetActiveScene().name != "Hard_Scene") {
            print("メイン以外"+TestSceneFlag);
            if (TestSceneFlag) {
                print("初期化");
                Initial_Variable();
            }
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
            //Scene読込
            ReadScene();
        }


        if (FiverFlag == false) {
            //何回目のフィーバーか判定
            switch (FiverCountFlag) {
                case 0:
                    //1回目のフィーバー
                    if (GameTime >= GameFinishTime - 120f) {
                        FiverNumber = 0;
                        FiverFlag = true;
                        Initial();
                        FiverCountFlag++;
                    }
                    break;
                case 1:
                    //２回目のフィーバー
                    if (GameTime >= GameFinishTime - 60f) {
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
                    }
                }
                break;
            //退避エリア(なにも入れない)
            case 3:
                break;
        }
        //ESCで終了
        if (Input.GetKey(KeyCode.Tab)) Quit();
    }

    //変数を初期化するための関数
    public void Initial() {
        FiverTime = Time.deltaTime;
        FiverEvacuation = FiverTime + FiverFinishTime;
        FiverFlag = true;
        print(FiverEvacuation);
    }

    public void ReadScene() {

        if (Bad_Score) {
            //テキストデータ初期化
            Initial_Scene();
            SceneManager.LoadScene("Score_Bad_Scene");
        }
        else if (Normal_Score) {
            //テキストデータ初期化
            Initial_Scene();
            SceneManager.LoadScene("Score_Normal_Scene");
        }
        else if (Good_Score) {
            //テキストデータ初期化
            Initial_Scene();
            SceneManager.LoadScene("Score_Good_Scene");
        }

    }

    void ActiveSceneChanged(Scene thisScene, Scene nextScene) {
        score_object = GameObject.Find("ScoreText"); // Textオブジェクト
        Pause_object = GameObject.Find("PouseUIText");
        if (score_object != null || Pause_object != null) {
            Text score_text = score_object.GetComponent<Text>();// オブジェクトからTextコンポーネントを取得
            Text Pause_text = Pause_object.GetComponent<Text>();
        }
    }

    //初期化シーン
    void Initial_Scene() {
        print("来てる？");
        score_object = null;
        Pause_object = null;
        TestSceneFlag = true;
    }

    //初期化変数
    void Initial_Variable() {
        GameFinishTime = 180f;
        FiverFinishTime = 30f;
        FiverNumber = 3;
        FiverFlag = false;
        TestSceneFlag = false;
        GameTime = 0f;
        FiverTime = 0f;

         Bad_Score = score_num < 1000;                           //スコア1000未満でBad_Score
        Normal_Score = score_num >= 1000 && score_num < 2000;   //スコア1000以上2000未満でNormal_Score
        Good_Score = score_num >= 2000;                         //スコア2000以上でGood_Score
    }

    void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
          UnityEngine.Application.Quit();
#endif
    }
}