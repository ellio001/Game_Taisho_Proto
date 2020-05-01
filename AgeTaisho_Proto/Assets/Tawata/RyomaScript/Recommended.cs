using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recommended : MonoBehaviour {
    
    public float GameTime;             //ゲーム開始の時間
    public GameObject Recommended_object = null;

    // Start is called before the first frame update
    void Start() {
        
        // ランダムな値を取得する
        float random = Random.value;
    }

    
    // Update is called once per frame
    void Update() {
        // オブジェクトからTextコンポーネントを取得
        Text Recommended_text = Recommended_object.GetComponent<Text>();
        // テキストの表示を入れ替える
        Recommended_text.text = "ルーレット";
    }
}
