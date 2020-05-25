using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty_Script : MonoBehaviour {
    [System.NonSerialized] public int Difficulty; //難易度選択の変数

    [SerializeField]
    GameObject Choice_Image;    //Chois_Imageをいれる箱
    Transform Choice_Image_Transform;   //Chois_ImageのTransformをいれる箱
    Vector3 Choice_Image_Vector;    //Chois_Imageの

    GameObject easy_back_image = null;
    GameObject easy_back_ui = null;
    GameObject normal_back_ui = null;
    GameObject hard_back_ui = null;

    bool Button_Flg;
    float Button_Time;

    // Start is called before the first frame update
    void Start() {
        Difficulty = 0;
        Button_Flg = false;
        //オブジェクトの取得
        easy_back_image = GameObject.Find("Easy_Back_Image");
        easy_back_ui = GameObject.Find("Easy_Back_UI");
        normal_back_ui = GameObject.Find("Normal_Back_UI");
        hard_back_ui = GameObject.Find("Hard_Back_UI");
        //処理
        easy_back_image.SetActive(false);
        easy_back_ui.SetActive(false);
        normal_back_ui.SetActive(false);
        hard_back_ui.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

        Choice_Image_Transform = Choice_Image.transform;    //Choice_Image.transform(本体)の位置をChoice_Image_Transform(仮)にいれる
        Choice_Image_Vector = Choice_Image_Transform.position;  //仮にはいっている位置のVector3をChoice_Image_Vectorにいれる

        if (Button_Flg) {
            Button_Time += Time.deltaTime;
            if (Button_Time >= 0.2) {
                Button_Flg = false;
                Button_Time = 0;
            }
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || 0 < Input.GetAxisRaw("XBox_Pad_V")) && Difficulty == 0 && !Button_Flg) {
            Button_Flg = true;
            Difficulty = 2;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || 0 < Input.GetAxisRaw("XBox_Pad_V")) && Difficulty > 0 && !Button_Flg) {
            Button_Flg = true;
            Difficulty -= 1;
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("XBox_Pad_V")) && Difficulty == 2 && !Button_Flg) {
            Button_Flg = true;
            Difficulty = 0;
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("XBox_Pad_V")) && Difficulty < 2 && !Button_Flg) {
            Button_Flg = true;
            Difficulty += 1;
        }

        switch (Difficulty) {
            case 0:
                Easy_Active();
                break;
            case 1:
                Normal_Active();
                break;
            case 2:
                Hard_Active();
                break;
        }

        ////Choice_Image_Vector.y = -150 * (Difficulty + 1) + 650;
        //Choice_Image_Vector.y = -150 * (Difficulty + 1) + 650;  //仮の座標を決定
        //Choice_Image_Transform.position = Choice_Image_Vector;  //仮の座標を本決定

        print(Choice_Image_Vector.y);

        if (Input.GetButtonUp("XBox_joystick_B")) {
            switch (Difficulty) {
                case 0:
                    SceneManager.LoadScene("Easy_Tutorial_Scene");
                    break;
                case 1:
                    SceneManager.LoadScene("Normal_Tutorial_Scene");
                    break;
                case 2:
                    SceneManager.LoadScene("Hard_Tutorial_Scene");
                    break;
            }
        }

    }
    void Easy_Active() {
        easy_back_image.SetActive(true);
        easy_back_ui.SetActive(true);
        normal_back_ui.SetActive(false);
        hard_back_ui.SetActive(false);
    }
    void Normal_Active() {
        easy_back_image.SetActive(false);
        easy_back_ui.SetActive(false);
        normal_back_ui.SetActive(true);
        hard_back_ui.SetActive(false);
    }
    void Hard_Active() {
        easy_back_image.SetActive(false);
        easy_back_ui.SetActive(false);
        normal_back_ui.SetActive(false);
        hard_back_ui.SetActive(true);
    }
}
