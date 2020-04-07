using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skep_Botom_Script : MonoBehaviour {

    [SerializeField]
    //ポーズした時に表示するUI
    private GameObject PauseUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q")) {
            //　ポーズUIのアクティブ、非アクティブを切り替え
            PauseUI.SetActive(!PauseUI.activeSelf);

            //　ポーズUIが表示されてる時は停止
            if (PauseUI.activeSelf) {
                Time.timeScale = 0f;
                //　ポーズUIが表示されてなければ通常通り進行
            }
            else {
                Time.timeScale = 1f;
            }
        }   
    }
}
