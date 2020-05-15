using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score_Scene_Transition : MonoBehaviour
{
    [SerializeField]
    GameObject score_object; // Textオブジェクトをいれる箱

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトからTextコンポーネントを取得
        Text score_text = score_object.GetComponent<Text>();
        // テキストの表示を入れ替える
        score_text.text = "TotalScore:" + GameManager.instance.score_num;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("XBox_joystick_B"))
        {
            SceneManager.LoadScene("Difficulty_Scene");
        }
    }
}
