using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty_Manager : MonoBehaviour
{
    [System.NonSerialized] public int Difficulty; //難易度選択の変数

    GameObject easy_back_image = null;
    GameObject easy_back_ui = null;

    GameObject normal_back_image = null;
    GameObject normal_back_ui = null;

    GameObject hard_back_image = null;
    GameObject hard_back_ui = null;

    bool Button_Flg = false;
    float Button_Time;
    

    void Start()
    {
        Difficulty = 0;
        //オブジェクトの取得
        easy_back_image = GameObject.Find("Easy_Back_Image");
        easy_back_ui = GameObject.Find("Easy_Back_UI");
        normal_back_image = GameObject.Find("Normal_Back_Image");
        normal_back_ui = GameObject.Find("Normal_Back_UI");
        hard_back_image = GameObject.Find("Hard_Back_Image");
        hard_back_ui = GameObject.Find("Hard_Back_UI");

        //処理
        normal_back_image.SetActive(false);
        normal_back_ui.SetActive(false);
        hard_back_image.SetActive(false);
        hard_back_ui.SetActive(false);
    }

    public void Update()
    {
        if (Button_Flg) // 次のボタンが押せるまでのインターバル
        {
            Button_Time += Time.deltaTime;
            if (Button_Time >= 0.15)
            {
                Button_Flg = false;
                Button_Time = 0;
            }
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || 0 < Input.GetAxisRaw("XBox_Pad_V")) && Difficulty == 0 && !Button_Flg)
        {
            Button_Flg = true;
            Difficulty = 2;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || 0 < Input.GetAxisRaw("XBox_Pad_V")) && Difficulty > 0 && !Button_Flg)
        {
            Button_Flg = true;
            Difficulty -= 1;
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("XBox_Pad_V")) && Difficulty == 2 && !Button_Flg)
        {
            Button_Flg = true;
            Difficulty = 0;
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("XBox_Pad_V")) && Difficulty < 2 && !Button_Flg)
        {
            Button_Flg = true;
            Difficulty += 1;
        }

        switch (Difficulty)
        {
            case 0:
                Easy_Active();
                if (Input.GetButtonUp("XBox_joystick_B"))
                    SceneManager.LoadScene("Easy_Scene");
                break;
            case 1:
                Normal_Active();
                if (Input.GetButtonUp("XBox_joystick_B"))
                    SceneManager.LoadScene("Normal_Scene");
                break;
            case 2:
                Hard_Active();
                if (Input.GetButtonUp("XBox_joystick_B"))
                    SceneManager.LoadScene("Hard_Scene");
                break;
        }
    }


    void Easy_Active()
    {
        easy_back_image.SetActive(true);
        easy_back_ui.SetActive(true);
        normal_back_image.SetActive(false);
        normal_back_ui.SetActive(false);
        hard_back_image.SetActive(false);
        hard_back_ui.SetActive(false);
    }
    void Normal_Active()
    {
        easy_back_image.SetActive(false);
        easy_back_ui.SetActive(false);
        normal_back_image.SetActive(true);
        normal_back_ui.SetActive(true);
        hard_back_image.SetActive(false);
        hard_back_ui.SetActive(false);
    }
    void Hard_Active()
    {
        easy_back_image.SetActive(false);
        easy_back_ui.SetActive(false);
        normal_back_image.SetActive(false);
        normal_back_ui.SetActive(false);
        hard_back_image.SetActive(true);
        hard_back_ui.SetActive(true);
    }
}
