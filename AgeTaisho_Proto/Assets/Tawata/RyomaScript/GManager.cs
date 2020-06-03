using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GManager : MonoBehaviour {

    //ゲームメイン
    float GameTime;             //ゲーム開始の時間
    float GameFinishTime;       //ゲームのプレイ最大時間
    float FiverTime;            //フィーバーの時間です
    float FiverEvacuation;      //フィーバーの退避エリア
    float FiverFinishTime;      //フィーバータイム最大時間
    int FiverNumber;            //スイッチ文で使うフィーバーフラグ
    int FiverCountFlag = 0;     //何回目のフィーバーか判定フラグ
    bool FiverFlag;             //フィーバーかどうかの判定フラグ
    bool TestSceneFlag;         //シーンを飛ぶ用のフラグ
    float Taihi;                //退避エリア
    bool TaihiFlag;             //退避フラグ
    public int score_num = 0; // スコア変数
    public GameObject score_object = null; // Textオブジェクト
    public GameObject Pause_object = null;

    //シーン
    int GameStatus;
    bool sceneflag;

    //リザルト
    bool Bad_Score;     //Bad_Scoreをいれる箱
    bool Normal_Score;  //Nomal_Score1をいれる箱
    bool Good_Score;    //Good_Scoreをいれる箱
                        //public Slider slider;    //Sliderを入れる


    // プレファブ達をリスト化
    [SerializeField] List<GameObject> Item_Resources = new List<GameObject>();
    [SerializeField] List<GameObject> Powder_Resources = new List<GameObject>();
    [SerializeField] List<GameObject> Fried_Resources = new List<GameObject>();
    [SerializeField] List<GameObject> Dish_Resources = new List<GameObject>();

    public static GManager instance = null;

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
        sceneflag = false;
        print("スタート");
    }


    private void Update() {

        print("update");

        switch (GameStatus) {
            //タイトル
            case 0:
                if (!sceneflag) LoadScene();
                TitleScene();
                break;
            //難易度選択
            case 1:
                DifficultyScene();
                break;
            //チュートリアル
            case 2:
                TutorialScene();
                break;
            //本日のおすすめ
            case 3:
                TodayScene();
                break;
            //メインシーン
            case 4:
                if (!sceneflag) LoadScene();
                MainScene();
                break;
            //リザルト
            case 5:
                ResultScene();
                break;
            //エンディング
            case 6:
                EndScene();
                break;
        }

    }

    //各シーンの初期化ロード
    public void LoadScene() {

        switch (GameStatus) {
            //タイトル
            case 0:
                break;
            //難易度選択
            case 1:
                break;
            //チュートリアル
            case 2:
                break;
            //本日のおすすめ
            case 3:
                break;
            //メインシーン
            case 4:
                //ゲームの最大時間
                GameFinishTime = 180f;

                //フィーバータイムの時間
                FiverFinishTime = 30f;

                //フィーバーの回数
                FiverNumber = 3;

                //フィーバー判定
                FiverFlag = false;

                //シーンを飛ぶ用のフラグ
                TestSceneFlag = true;

                //シーンマネージャー
                SceneManager.activeSceneChanged += ActiveSceneChanged;
                GameTime = 0f;

                Bad_Score = score_num < 1000;                           //スコア1000未満でBad_Score
                Normal_Score = score_num >= 1000 && score_num < 2000;   //スコア1000以上2000未満でNormal_Score
                Good_Score = score_num >= 2000;                         //スコア2000以上でGood_Score
                break;
            //リザルト
            case 5:
                break;
            //エンディング
            case 6:
                break;
        }
    }
    //タイトル
    public void TitleScene() {
        SceneManager.LoadScene("Easy_Tutorial_Scene");
    }
    //難易度選択
    public void DifficultyScene() {
    }
    //チュートリアル
    public void TutorialScene() {
    }
    //本日のおすすめ
    public void TodayScene() {
    }
    //メイン
    public void MainScene() {
        //現在までのフレーム
        GameTime += Time.deltaTime;
        ////ゲージを動かす
        //slider.value = GameTime / GameFinishTime;

        if (TestSceneFlag) {
            // オブジェクトからTextコンポーネントを取得
            Text score_text = score_object.GetComponent<Text>();
            Text Pause_text = Pause_object.GetComponent<Text>();
            // テキストの表示を入れ替える
            score_text.text = "Score:" + score_num;
            Pause_text.text = "Optionボタン：ポーズ";

            //判定
            Judgment();
        }
    }
    //リザルト
    public void ResultScene() {
    }
    //エンド
    public void EndScene() {
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

        if (Bad_Score) SceneManager.LoadScene("Score_Bad_Scene");
        else if (Normal_Score) SceneManager.LoadScene("Score_Normal_Scene");
        else if (Good_Score) SceneManager.LoadScene("Score_Good_Scene");

        //処理を二度としないようにフラグで管理
        TestSceneFlag = false;
    }

    void ActiveSceneChanged(Scene thisScene, Scene nextScene) {
        score_object = GameObject.Find("ScoreText"); // Textオブジェクト
        Pause_object = GameObject.Find("PouseUIText");
        if (score_object != null || Pause_object != null) {
            Text score_text = score_object.GetComponent<Text>();// オブジェクトからTextコンポーネントを取得
            Text Pause_text = Pause_object.GetComponent<Text>();
        }
    }

    void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
          UnityEngine.Application.Quit();
#endif
    }
}