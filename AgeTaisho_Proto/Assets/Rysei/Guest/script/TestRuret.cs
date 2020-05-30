using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestRuret : MonoBehaviour
{

    int RondemNumber;       //本日のおすすめ品番
    public static int NumberTaihi;        //確定した品番
    int NumberFlag;         //確定した品番をとる回数
    int Flame;              //フレーム変数
    int FlameMin;           //フレームの最大回数
    int FlameMax;           //フレームの初期値
    int Count;              //処理のカウント
    int CountMin;           //処理のカウント初期値
    int CountMax;           //処理の最大値
    int RandomMin;          //ランダムの最小値
    int RandomMax;          //ランダムの最大値
    //GameObject Recommended_object = null;

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
        Debug.Log(NumberTaihi);
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
            if (NumberFlag++ < 1) NumberTaihi = RondemNumber;
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
    public static int getNumberTaihi()
    {
        return NumberTaihi;
    }
}
