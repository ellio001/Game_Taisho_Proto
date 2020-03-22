using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Camera_2 : MonoBehaviour {


    public int cursor = 1; // カーソル用
    int old_cursor = 0;
    private GameObject cs_target_M;
    private GameObject cs_target_1;
    private GameObject cs_target_2;
    private GameObject cs_target_3;
    private GameObject cs_target_4;
    private GameObject cs_target_5;
    private GameObject cs_target_6;
    private GameObject cs_target_7;
    private GameObject cs_target_8;
    private GameObject cs_target_9;
    private GameObject cs_target_10;
    private GameObject cs_target_11;
    private GameObject cs_target_99; // ゴミ箱用番号

    private GameObject Garbage_can;

    void Start () {
        cs_target_1 = GameObject.Find("CS_1");
        cs_target_2 = GameObject.Find("CS_2");
        cs_target_3 = GameObject.Find("CS_3");
        cs_target_4 = GameObject.Find("CS_4");
        cs_target_5 = GameObject.Find("CS_5");
        cs_target_6 = GameObject.Find("CS_6");
        cs_target_7 = GameObject.Find("CS_7");
        cs_target_8 = GameObject.Find("CS_8");
        cs_target_9 = GameObject.Find("CS_9");
        cs_target_10 = GameObject.Find("CS_10");
        cs_target_11 = GameObject.Find("CS_11");
        cs_target_99 = GameObject.Find("CS_gomi");

        Garbage_can = GameObject.Find("Garbage can");
        Garbage_can.gameObject.SetActiveRecursively(false);
    }
	
	void Update () {
        DownKeyCheck();
        

	}

    void DownKeyCheck()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            { // 左押したとき
                Garbage_can.gameObject.SetActiveRecursively(false); // ゴミ箱非表示
                cursor += 1;
                if (cursor > 11) cursor = 1;
                CameraCursor();
            }
            if (Input.GetKey(KeyCode.RightArrow))
            { // 右押したとき
                Garbage_can.gameObject.SetActiveRecursively(false);
                cursor -= 1;
                if (cursor < 1) cursor = 11;
                CameraCursor();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            { // 下押したときゴミ箱を見る
                if (cs_target_M != cs_target_99)
                {
                    Garbage_can.gameObject.SetActiveRecursively(true); // ゴミ箱表示
                    cs_target_M = cs_target_99;
                    var aim = this.cs_target_M.transform.position - this.transform.position;
                    var look = Quaternion.LookRotation(aim);
                    this.transform.localRotation = look;
                }
            }
            if (Input.GetKey(KeyCode.UpArrow))
            { // 上押したときゴミ箱の選択前に戻る
                Garbage_can.gameObject.SetActiveRecursively(false);
                CameraCursor();
            }

        }
    }

    void CameraCursor()
    {

        switch (cursor)
        {
            case 1:
                cs_target_M = cs_target_1;
                break;
            case 2:
                cs_target_M = cs_target_2;
                break;
            case 3:
                cs_target_M = cs_target_3;
                break;
            case 4:
                cs_target_M = cs_target_4;
                break;
            case 5:
                cs_target_M = cs_target_5;
                break;
            case 6:
                cs_target_M = cs_target_6;
                break;
            case 7:
                cs_target_M = cs_target_7;
                break;
            case 8:
                cs_target_M = cs_target_8;
                break;
            case 9:
                cs_target_M = cs_target_9;
                break;
            case 10:
                cs_target_M = cs_target_10;
                break;
            case 11:
                cs_target_M = cs_target_11;
                break;
        }
        var aim = this.cs_target_M.transform.position - this.transform.position;
        var look = Quaternion.LookRotation(aim);
        this.transform.localRotation = look;
    }
}
