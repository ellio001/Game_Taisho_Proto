using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Recommended : MonoBehaviour {

    public static int NumberTaihi;  //確定した品番
    int RondemNumber;               //本日のおすすめ品番
    bool NumberFlag;                //確定した品番取った判定
    int Flame;                      //フレーム変数
    int FlameMin;                   //フレームの最大回数
    int FlameMax;                   //フレームの初期値
    int Count;                      //処理のカウント
    int CountMin;                   //処理のカウント初期値
    int CountMax;                   //処理の最大値
    int RandomMin;                  //ランダムの最小値
    int RandomMax;                  //ランダムの最大値
    GameObject Recommended_object = null;
    GameObject ebi_image_object = null;
    GameObject imo_image_object = null;
    GameObject sakana_image_object = null;
    GameObject panel_object = null;
    GameObject Decision_Text = null;

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
        FlameMax = 200;
        RandomMin = 100;
        RandomMax = 200;
        CountMax = Random.Range(RandomMin, RandomMax);
        Count = 0;
        CountMin = 0;
        NumberFlag = false;

        //処理
        ebi_image_object.SetActive(false);
        imo_image_object.SetActive(false);
        sakana_image_object.SetActive(false);
        panel_object.SetActive(false);
        Decision_Text.SetActive(false);
    }
    
    // Update is called once per frame
    void Update() {

        if (NumberFlag == false) {
            RondemNumber++;
            if (Count++ < CountMax) {
                if (Flame++ <= FlameMax) {
                    if (RondemNumber == 1) {
                        ebi_image_object.SetActive(true);
                        imo_image_object.SetActive(false);
                        sakana_image_object.SetActive(false);
                    }
                    else if (RondemNumber == 2) {
                        ebi_image_object.SetActive(false);
                        imo_image_object.SetActive(false);
                        sakana_image_object.SetActive(true);
                    }
                    else if (RondemNumber == 3) {
                        ebi_image_object.SetActive(false);
                        imo_image_object.SetActive(true);
                        sakana_image_object.SetActive(false);
                        RondemNumber = 0;
                    }
                    Flame = FlameMin;
                }
            }
            else if (Count++ >= CountMax) {
                NumberFlag = true;
                if (NumberFlag) NumberTaihi = RondemNumber;
            }
        }
        else if (NumberFlag) {
            panel_object.SetActive(true);
            Decision_Text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("〇")) {
                SceneManager.LoadScene("Easy_Scene");
            }
        }

        //テスト用においている
        //もし見つけたら消してくれ
        if (Input.GetKey(KeyCode.Escape)) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
