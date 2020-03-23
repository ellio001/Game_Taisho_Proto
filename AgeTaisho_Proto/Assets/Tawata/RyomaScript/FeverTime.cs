using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FeverTime : MonoBehaviour {

    float GameTime;             //ゲームのプレイ時間
    float GameFinishTime;       //ゲームのプレイ最大時間3分は10,800フレーム
    float FiverTime;            //フィーバータイム
    float FiverFinishTime;      //フィーバータイム最大時間
    public bool FiverFlag;      //フィーバーのフラグ
    bool TestSceneFlag;         //デバッグ用のフラグ
    
    private void Start() {
        GameTime = 0f;
        GameFinishTime = 1000f;
        FiverTime = 0f;
        FiverFinishTime = 50f;
        FiverFlag = false;
        TestSceneFlag = true;
    }

    private void Update() {
        
    }

    public void Addition() {
        //ゲーム時間を加算
        GameTime++;
        //Debug.Log("GameTime:" + GameTime);
    }

    public void Judgment() {
        //ゲーム時間を判定
        if (GameTime == GameFinishTime) {
            Debug.Log("ゲーム終了！");
            //Scene読込
            ReadScene();
        }
        //Fiverかどうか判定
        if (FiverFlag == false) {
            //FiverTime初期化
            Initial();
            //FiverFlag = true;
            //Debug.Log("フィーバータイム開始！");
        }
        else if (FiverFlag) {
            FiverTime++;
            Debug.Log("FiverTime:" + FiverTime);
            //フィーバータイム終了
            if (FiverTime >= FiverFinishTime) {
                FiverFlag = false;
            }
        }
    }

    //変数を初期化するための関数
    public void Initial() {
        FiverTime = 0f;
    }

    public void ReadScene() {
        //Endシーン読込
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
        //処理を二度としないようにフラグで管理
        TestSceneFlag = false;
    }
}
