using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score_Scene_Transition : MonoBehaviour
{
    [SerializeField] GameObject score_object;    // Textオブジェクトをいれる箱
    [SerializeField] GameObject StartText;

    float Button_Time;          //ボタンが押せるようになるまでのインターバル

    // Start is called before the first frame update
    void Start()
    {
        Text score_text = score_object.GetComponent<Text>();         // オブジェクトからTextコンポーネントを取得
        score_text.text = GameManager.instance.score_num + "円";     // テキストの表示を入れ替える
        StartText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Button_Time += Time.deltaTime;
        if(Button_Time >= 1)  //このシーンにはいって0.5秒たったらボタンが押せるようになる
        {
            StartText.SetActive(true);

            if (Input.GetButtonUp("XBox_joystick_B"))
                SceneManager.LoadScene("Title_Scene");
        }

    }
}
