using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_limit : MonoBehaviour {

    float time;         //時間
    int gaugeMax;     //ゲージの最大

    //Sliderを入れる
    public Slider slider;

    void Start() {
        //Sliderを満タンにする。
        slider.value = 1;
        time = 0f;
        gaugeMax = 180;
    }

    //ColliderオブジェクトのIsTriggerにチェック入れること。
    void Update() {

        time += Time.deltaTime;//毎フレームの時間を加算.

        slider.value = time / gaugeMax;
        
        print(time);
        print(slider.value);
    }
}