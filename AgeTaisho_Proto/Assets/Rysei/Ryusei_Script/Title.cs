using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    int SelectNum; // 0=スタートを選択中,1=おわるを選択中

    GameObject Start_Text = null;
    GameObject Start_UI = null;
    GameObject End_Text = null;
    GameObject End_UI = null;
    GameObject First_Text = null;

    [SerializeField]
    GameObject Update_object; // Textオブジェクトをいれる箱
    bool First_flg = true;
    bool Next_flg = false;

    bool Button_Flg = false;
    float Button_Time;

    void Start()
    {
        // ファイル名を取得
        string path1 = Application.dataPath;
        System.DateTime dt = System.IO.File.GetLastWriteTime(path1);
        // オブジェクトからTextコンポーネントを取得
        Text Update_text = Update_object.GetComponent<Text>();
        // テキストの表示を入れ替える
        Update_text.text = dt.ToString("yyyy/MM/dd hh:mm");

        SelectNum = 0;
        //オブジェクトの取得
        Start_Text = GameObject.Find("Start_Text");
        Start_UI   = GameObject.Find("Start_UI");
        End_Text   = GameObject.Find("End_Text");
        End_UI     = GameObject.Find("End_UI");
        First_Text = GameObject.Find("First_Text");

        //処理
        Start_Text.SetActive(false);
        Start_UI.SetActive(false);
        End_Text.SetActive(false);
        End_UI.SetActive(false);
    }

    void Update()
    {
        if (Next_flg) // 始めにBボタンを押したら文字を表示させる
            Next_Active();


        if (First_flg) 
        {
            if (Input.GetButtonUp("XBox_joystick_B"))
            {
                First_Text.SetActive(false); //[Bボタン～]を非表示
                First_flg = false;
                Next_flg = true;
                // 「はじめる」「おわる」を表示
                Start_Text.SetActive(true); 
                Start_UI.SetActive(true);
                End_Text.SetActive(true);
            }
        }
    }

    void Next_Active()
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

        if ((Input.GetKeyDown(KeyCode.UpArrow) || 0 < Input.GetAxisRaw("XBox_Pad_V")) && SelectNum == 0 && !Button_Flg)
        {
            Button_Flg = true;
            SelectNum = 1;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || 0 < Input.GetAxisRaw("XBox_Pad_V")) && SelectNum == 1 && !Button_Flg)
        {
            Button_Flg = true;
            SelectNum = 0;
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("XBox_Pad_V")) && SelectNum == 0 && !Button_Flg)
        {
            Button_Flg = true;
            SelectNum = 1;
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("XBox_Pad_V")) && SelectNum == 1 && !Button_Flg)
        {
            Button_Flg = true;
            SelectNum = 0;
        }

        switch (SelectNum)
        {
            case 0:
                Start_Active();
                if (Input.GetButtonUp("XBox_joystick_B"))
                    SceneManager.LoadScene("Difficulty_Scene");
                break;
            case 1:
                End_Active();
                if (Input.GetButtonUp("XBox_joystick_B"))
                    UnityEditor.EditorApplication.isPlaying = false;
                break;
        }
    }

    void Start_Active()
    {
        Start_UI.SetActive(true);
        End_UI.SetActive(false);

    }
    void End_Active()
    {
        Start_UI.SetActive(false);
        End_UI.SetActive(true);

    }
}
