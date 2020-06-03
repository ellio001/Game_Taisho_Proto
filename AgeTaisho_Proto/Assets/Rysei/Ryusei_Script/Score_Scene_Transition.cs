using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score_Scene_Transition : MonoBehaviour
{
    [SerializeField] GameObject score_object;    // Textオブジェクトをいれる箱
    [SerializeField] GameObject NextText;        //Bボタンで移動　のテキスト
    [SerializeField] GameObject Panel;           //パネルをいれる箱
    Vector3 PanelPos;   //パネルの位置

    int Difficulty = 0;     //選択の変数
    int TransitionMax = 1;  //移動先の最大数-1

    bool Button_Flg = false;    //ボタンを押したとき1回とまらせるためのbool
    float Button_Time;          //次にボタンが押せるようになるまでのインターバル


    // Start is called before the first frame update
    void Start()
    {
        PanelPos = Panel.transform.position;    //パネルの座標をいれる
        // オブジェクトからTextコンポーネントを取得
        Text score_text = score_object.GetComponent<Text>();
        // テキストの表示を入れ替える
        score_text.text = GameManager.instance.score_num + "円";
    }

    // Update is called once per frame
    void Update()
    {
        if (Button_Flg) // 次にボタンが押せるようになるまでのインターバル
        {
            Button_Time += Time.deltaTime;
            if (Button_Time >= 0.15)
            {
                Button_Flg = false;
                Button_Time = 0;
            }
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || 0 < Input.GetAxisRaw("XBox_Pad_V")) && Difficulty == 0 && !Button_Flg)  //上押したときに一番上だったら一番下に行く
        {
            Button_Flg = true;
            Difficulty = TransitionMax;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || 0 < Input.GetAxisRaw("XBox_Pad_V")) && Difficulty > 0 && !Button_Flg)    //上押したときに1以上なら-1する
        {
            Button_Flg = true;
            Difficulty -= 1;
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("XBox_Pad_V")) && Difficulty == TransitionMax && !Button_Flg)  //下押したときに一番下だったら一番上に行く
        {
            Button_Flg = true;
            Difficulty = 0;
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("XBox_Pad_V")) && Difficulty < TransitionMax && !Button_Flg)  //下押したときに最大値以下なら+1する
        {
            Button_Flg = true;
            Difficulty += 1;
        }

        switch (Difficulty)
        {
            case 0:
                PanelPos.y = 156.5f;
                if (Input.GetButtonUp("XBox_joystick_B"))
                    SceneManager.LoadScene("Title_Scene");
                break;
            case 1:
                PanelPos.y = 56.5f;
                if (Input.GetButtonUp("XBox_joystick_B"))
                    SceneManager.LoadScene("Difficulty_Scene");
                break;
        }

        Panel.transform.position = PanelPos;    //パネルの座標をいれる
    }
}
