using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Display : MonoBehaviour
{
    bool flg = false;
    [SerializeField] GameObject TimeBar; // UIの時間バーを入れる

    void Start()
    {
        // ザ・ワールド‼（時間停止）
        Time.timeScale = 0f;
        // ゲーム開始時にチュートリアル表示させる
        this.gameObject.SetActive(true);
        TimeBar.gameObject.SetActive(false); // 始めのチュートリアル表示中はバーを消す
    }

    void Update()
    {
        if (Input.GetButtonDown("XBox_joystick_B") && !flg)
        { // 始めの一度だけ読む
            flg = true;
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
            TimeBar.gameObject.SetActive(true);

        }
    }
}
