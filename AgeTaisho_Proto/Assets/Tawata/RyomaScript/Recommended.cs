using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Recommended : MonoBehaviour {

    private float GameTime;             //ゲーム開始の時間
    GameObject Recommended_object = null;
    GameObject Random_obj;
    Random_Number script;

    // Start is called before the first frame update
    void Start() {
        Recommended_object = GameObject.Find("Recommended_text"); // Textオブジェクト
        Random_obj = GameObject.Find("Random_obj");
        script = Random_obj.GetComponent<Random_Number>();
    }


    // Update is called once per frame
    void Update() {

        // オブジェクトからTextコンポーネントを取得
        Text Recommended_text = Recommended_object.GetComponent<Text>();

        if (script.Number == 1) {
            // テキストの表示を入れ替える
            Recommended_text.text = "えび";
            print("えび");
        }
        else if (script.Number == 2) {
            // テキストの表示を入れ替える
            Recommended_text.text = "さかな";
            print("さかな");
        }
        else if (script.Number == 3) {
            // テキストの表示を入れ替える
            Recommended_text.text = "いも";
            print("いも");
        }
    }
}
