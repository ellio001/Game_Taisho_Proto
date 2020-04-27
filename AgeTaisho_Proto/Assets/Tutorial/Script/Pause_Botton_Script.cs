using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Botton_Script : MonoBehaviour {

    public bool PauseFlag;

    [SerializeField]
    //ポーズした時に表示するUI
    private GameObject PauseUI;

    // Start is called before the first frame update
    void Start()
    {
        PauseFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PS4_joystick_button_9")) {
            //　ポーズUIのアクティブ、非アクティブを切り替え
            PauseUI.SetActive(!PauseUI.activeSelf);

            //　ポーズUIが表示されてる時は停止
            if (PauseUI.activeSelf) {
                Time.timeScale = 0f;
                PauseFlag = true;
                //　ポーズUIが表示されてなければ通常通り進行
            }
            else {
                Time.timeScale = 1f;
                PauseFlag = false;
            }
        }   
    }
}
