using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class inputDemo : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        ////L Stick
        //float lsh = Input.GetAxis("L_Stick_H");
        //float lsv = Input.GetAxis("L_Stick_V");
        //if ((lsh != 0) || (lsv != 0)) {
        //    Debug.Log("L stick:" + lsh + "," + lsv);
        //}
        ////R Stick
        //float rsh = Input.GetAxis("R_Stick_H");
        //float rsv = Input.GetAxis("R_Stick_V");
        //if ((rsh != 0) || (rsv != 0)) {
        //    Debug.Log("R stick:" + rsh + "," + rsv);
        //}
        ////Trigger
        //float tri = Input.GetAxis("L_R_Trigger");
        //if (tri > 0) {
        //    Debug.Log("L trigger:" + tri);
        //}
        //else if (tri < 0) {
        //    Debug.Log("R trigger:" + tri);
        //}
        //else {
        //    Debug.Log("  trigger:none");
        //}
        //D-Pad
        float dph = Input.GetAxis("XBox_Pad_H");
        float dpv = Input.GetAxis("XBox_Pad_V");
        if ((dph != 0) || (dpv != 0)) {
            Debug.Log("D Pad:" + dph + "," + dpv);
        }
    }
}