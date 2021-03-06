﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Recommended_Easy : MonoBehaviour {

    public static int NumberTaihi;  //確定した品番
    int RondemNumber;               //本日のおすすめ品番
    bool NumberFlag;                //確定した品番取った判定
    int Flame;                      //フレーム変数
    int FlameMin;                   //フレームの最大回数
    int FlameMax;                   //フレームの初期値
    int FlameMax2;                  //NumberFlagがたった後のフレーム
    bool FlashingFlag;              //文字の点滅に使う
    int Count;                      //処理のカウント
    int CountMin;                   //処理のカウント初期値
    int CountMax;                   //処理の最大値
    int RandomMin;                  //ランダムの最小値
    int RandomMax;                  //ランダムの最大値
    bool soundflag;                 //サウンドフラグ
    bool soundflag2;                 //サウンドフラグ
    GameObject Recommended_object = null;
    GameObject ebi_image_object = null;
    GameObject imo_image_object = null;
    GameObject sakana_image_object = null;
    GameObject panel_object = null;
    GameObject Decision_Text = null;

    //オーディオ
    AudioSource[] sounds;

    // Start is called before the first frame update
    void Start() {

        // オブジェクトの取得
        ebi_image_object = GameObject.Find("EbiImage");      // Imageオブジェクト
        imo_image_object = GameObject.Find("ImoImage");      // Imageオブジェクト
        sakana_image_object = GameObject.Find("SakanaImage");// Imageオブジェクト
        panel_object = GameObject.Find("PanelImage");        // Panelオブジェクト
        Decision_Text = GameObject.Find("DecisionText");     // Textオブジェクト

        //変数
        RondemNumber = 0;
        Flame = 0;
        FlameMin = 0;
        FlameMax = 3;
        FlameMax2 = 60;
        RandomMin = 30;
        RandomMax = 50;
        CountMax = Random.Range(RandomMin, RandomMax);
        Count = 0;
        CountMin = 0;
        NumberFlag = false;
        FlashingFlag = false;
        soundflag = false;
        soundflag2 = false;

        //処理
        ebi_image_object.SetActive(false);
        imo_image_object.SetActive(false);
        sakana_image_object.SetActive(false);
        panel_object.SetActive(true);
        Decision_Text.SetActive(false);

        //オーディオの情報取得
        sounds = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        //描画
        Jugment();
        //ルーレット処理
        if (!NumberFlag) {
            if (!soundflag && !soundflag2) {
                //ロールスタート
                Start_Sound();
            }
            if (Flame++ >= FlameMax) {
                if (Count++ < CountMax) {
                    RondemNumber++;
                    //描画
                    Jugment();
                }
                else if (Count++ >= CountMax) {
                    NumberFlag = true;
                    FlashingFlag = true;
                    if (NumberFlag) NumberTaihi = RondemNumber;
                }
                Flame = FlameMin;
            }
        }

        //ルーレット後の処理
        else{
            panel_object.SetActive(false);
            if (soundflag && !soundflag2) {
                //ロールストップ
                Stop_Sound();
            }
            else if (soundflag && soundflag2) {
                //ドドンスタート
                Start_Sound_Ddon();
            }
            Flame++;
            //文字点滅処理
            if (Flame > FlameMax2) {
                if (FlashingFlag) {
                    Decision_Text.SetActive(true);
                    FlashingFlag = false;
                }
                else {
                    Decision_Text.SetActive(false);
                    FlashingFlag = true;
                }
                Flame = FlameMin;
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("XBox_joystick_B")) {
                //シーン遷移
                SceneManager.LoadScene("Easy_Scene");
            }
        }

        //テスト用においている
        //もし見つけたら消してくれ
        if (Input.GetKey(KeyCode.Tab)) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
    public static int getNumberTaihi() {
        return GameManager.instance.Osusume = NumberTaihi;
    }

    void Jugment() {

        //エビ
        if (RondemNumber == 1) {
            ebi_image_object.SetActive(true);
            imo_image_object.SetActive(false);
            sakana_image_object.SetActive(false);
        }
        //サカナ
        else if (RondemNumber == 2) {
            ebi_image_object.SetActive(false);
            imo_image_object.SetActive(false);
            sakana_image_object.SetActive(true);
        }
        //イモ
        else if (RondemNumber == 3) {
            ebi_image_object.SetActive(false);
            imo_image_object.SetActive(true);
            sakana_image_object.SetActive(false);
            RondemNumber = 0;
        }
    }

    //ロールスタート
    void Start_Sound() {
        //サウンド再生
        sounds[0].Play();
        soundflag = true;
    }
    
    //ロール終了
    void Stop_Sound() {
        //サウンド再生
        sounds[0].Stop();
        soundflag2 = true;
    }

    //ドドンスタート
    void Start_Sound_Ddon() {
        //サウンド再生
        sounds[1].Play();
        soundflag = false;
        soundflag2 = false;
    }
}
