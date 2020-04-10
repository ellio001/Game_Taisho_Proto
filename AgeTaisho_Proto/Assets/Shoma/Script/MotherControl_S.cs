using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherControl_S : MonoBehaviour {

    float y = 2.0f;

    GameObject Pause;
    Pause_Botton_Script script;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //ポーズ画面
        Pause = GameObject.Find("Main Camera");
        script = Pause.GetComponent<Pause_Botton_Script>();
    }

    // Update is called once per frame
    void Update() {
        if (script.PauseFlag) {
            return;
        }
        else {
            if (Input.GetAxisRaw("L_Horizontal") < 0/* && -0.4 < transform.rotation.y*/) {
                Debug.Log("左に傾いている");
                transform.Rotate(new Vector3(0, -2.0f, 0));

                //y -= Time.deltaTime * 100;
                //transform.rotation = Quaternion.Euler(0, x, 0);
            }
            else if (0 < Input.GetAxisRaw("L_Horizontal")/* && transform.rotation.x > 0.4*/) {
                Debug.Log("右に傾いている");
                transform.Rotate(new Vector3(0, 2.0f, 0));

                //y += Time.deltaTime * 100;
                //transform.rotation = Quaternion.Euler(0, x, 0);
            }
            else {
                //左右方向には傾いていない
            }

            //if (Input.GetMouseButton(0))
            //{
            //    Debug.Log("下に傾いている");
            //    transform.Rotate(new Vector3(0, 2, 0));
            //}

            // 今だけコメント
            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    transform.Rotate(new Vector3(0, 2.0f, 0));
            //}
            //else if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    transform.Rotate(new Vector3(0, -2.0f, 0));
            //}
        }
    }
}
