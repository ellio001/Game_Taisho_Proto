using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_limit : MonoBehaviour {

    public float time;         //時間
    float gaugeMax;       //ゲージの最大

    //Sliderを入れる
    Slider slider;

    void Start() {

        slider = GetComponent<Slider>();

        //Sliderを満タンにする。
        time = 0f;
        gaugeMax = 180f;

        //スライダーの最大値の設定
        slider.maxValue = gaugeMax;
    }

    //ColliderオブジェクトのIsTriggerにチェック入れること。
    void Update() {

        time += Time.deltaTime;//毎フレームの時間を加算.

        slider.value = time;
        


        //print(time);
        //print(slider.value);
    }
}