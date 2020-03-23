using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    float time;                 //ゲーム開始の時間
    float GameFinishTime;       //ゲームのプレイ最大時間
    float FiverTime;            //フィーバーの時間です
    float FiverEvacuation;      //フィーバーの退避エリア
    float FiverFinishTime;      //フィーバータイム最大時間
    public int FiverFlag;       //フィーバーのフラグ
    bool TestSceneFlag;         //シーンを飛ぶ用のフラグ

    public static GameManager instance = null;
    public int score_num = 0; // スコア変数
    public GameObject score_object = null; // Textオブジェクト

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
        //GameTime = 0f;
        GameFinishTime = 180f;
        //FiverTime = 0f;
        FiverFlag = 0;
        TestSceneFlag = true;
        FiverFinishTime = 30f;
    }

    private void Update() {

        time += Time.deltaTime;

        if (TestSceneFlag) {
            // オブジェクトからTextコンポーネントを取得
            Text score_text = score_object.GetComponent<Text>();
            // テキストの表示を入れ替える
            score_text.text = "Score:" + score_num;

            Addition();
            Judgment();
        }
        //Debug.Log("経過時間(秒)" + Time.time);
        //print("経過時間(秒)" + Time.time);
    }

    public void Addition() {
        //ゲーム時間を加算
        //time = Time.deltaTime;
        //Debug.Log("GameTime:" + GameTime);
    }

    public void Judgment() { 
        //ゲーム時間を判定
        else if (time >= GameFinishTime) {
            Debug.Log("ゲーム終了！");
            //Scene読込
            ReadScene();
        }
        switch (FiverFlag) {
            case 0:
                break;
        }
        ////Fiverかどうか判定
        //if (FiverFlag == false) {
        //    //FiverTime初期化
        //    Initial();
        //    //FiverFlag = true;
        //}
        //else if (FiverFlag) {
        //    FiverTime += Time.deltaTime;
        //    print(FiverTime);
        //    print(FiverEvacuation);
        //    //FiverTime++;
        //    //フィーバータイム終了
        //    if (FiverTime >= FiverEvacuation) {
        //        FiverFlag = false;
        //        Debug.Log("フィーバータイム終了！");
        //    }
        //}
    }

    //変数を初期化するための関数
    public void Initial() {
        FiverTime = Time.deltaTime;
        FiverEvacuation = FiverTime + FiverFinishTime;
    }

    public void ReadScene() {
        //Endシーン読込
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
        //処理を二度としないようにフラグで管理
        TestSceneFlag = false;
    }
}
