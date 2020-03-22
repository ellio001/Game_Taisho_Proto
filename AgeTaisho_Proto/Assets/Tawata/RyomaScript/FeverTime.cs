using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FeverTime : MonoBehaviour {
    float GameTime;             //ゲームのプレイ時間
    float GameFinishTime;       //ゲームのプレイ最大時間
    float FiverTime;            //フィーバータイム
    float FiverFinishTime;      //フィーバータイム最大時間


    // Start is called before the first frame update
    void Start() {
        GameTime = 0f;
        GameFinishTime = 200f;
        FiverTime = 0f;
        FiverFinishTime = 200f;
    }

    // Update is called once per frame
    void Update() {
        Addition();
        Judgment();
    }

    public void Addition() {
        //ゲーム時間を加算
        GameTime++;
        Debug.Log(GameTime);
    }

    public void Judgment() {
        //ゲーム時間を判定
        if (GameTime == GameFinishTime) {
            Debug.Log("ゲーム終了！");
            ReadScene();
        }
        //Fiverかどうか判定
        //試しに100フレームたったらフィーバー
        if (GameTime >= GameTime - 100f) {
            Debug.Log("フィーバータイム開始！");
            FiverTime++;
            Debug.Log(FiverTime);
        }
    }

    public void ReadScene() {
        //Endシーン読込
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
    }

}
