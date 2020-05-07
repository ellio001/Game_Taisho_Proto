using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestRuret : MonoBehaviour
{

    public int RondemNumber;
    int Flame;
    int FlameMin;
    int FlameMax;
    int Count;
    int CountMin;
    int CountMax;
    int RandomMin;
    int RandomMax;
    GameObject Recommended_object = null;

    // Start is called before the first frame update
    void Start()
    {
        //Recommended_object = GameObject.Find("Recommended_text"); // Textオブジェクト
        RondemNumber = 0;
        Flame = 0;
        FlameMin = 0;
        FlameMax = 200;
        RandomMin = 100;
        RandomMax = 200;
        CountMax = Random.Range(RandomMin, RandomMax);
        Count = 0;
        CountMin = 0;
    }



    // Update is called once per frame
    void Update()
    {

        // オブジェクトからTextコンポーネントを取得
        //Text Recommended_text = Recommended_object.GetComponent<Text>();

        RondemNumber++;

        if (Count++ < CountMax)
        {
            if (Flame++ <= FlameMax)
            {
                if (RondemNumber == 1)
                {
                    // テキストの表示を入れ替える
                    //Recommended_text.text = "えび";
                    print("えび");
                }
                else if (RondemNumber == 2)
                {
                    // テキストの表示を入れ替える
                    //Recommended_text.text = "さかな";
                    print("さかな");
                }
                else if (RondemNumber == 3)
                {
                    // テキストの表示を入れ替える
                    //Recommended_text.text = "いも";
                    print("いも");
                    RondemNumber = 0;
                }
                Flame = FlameMin;
            }
        }
        else if (Count++ >= CountMax)
        {

        }

        //if (script.Number == 1) {
        //    // テキストの表示を入れ替える
        //    Recommended_text.text = "えび";
        //    print("えび");
        //}
        //else if (script.Number == 2) {
        //    // テキストの表示を入れ替える
        //    Recommended_text.text = "さかな";
        //    print("さかな");
        //}
        //else if (script.Number == 3) {
        //    // テキストの表示を入れ替える
        //    Recommended_text.text = "いも";
        //    print("いも");
        //}
    }
}
