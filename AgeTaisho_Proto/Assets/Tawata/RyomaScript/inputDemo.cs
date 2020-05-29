using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class inputDemo : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        //float dph = Input.GetAxis("XBox_Pad_H");
        //float dpv = Input.GetAxis("XBox_Pad_V");
        float menu = Input.GetAxis("XBox_joystick_Menu");
        //if ((dph != 0) || (dpv != 0)) {
        //    Debug.Log("D Pad:" + dph + "," + dpv);
        //}
        if(menu != 0) {
            Debug.Log("Menu:" + menu);
        }
    }
}