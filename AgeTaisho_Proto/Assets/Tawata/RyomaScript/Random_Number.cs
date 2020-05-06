using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Random_Number : MonoBehaviour {

    float Random_1;         //ランダム変数
    float RandomMax;        //ランダムの最大値
    float RandomMin;        //ランダムの最小値
    public int Number;             //本日のおすすめの品番
    int Flame;              //何フレームか計測
    int FlameMax;           //フレームの最大数
    int Flameinitial;       //フレームの初期値
    int Count;              //処理のカウント
    int CountMax;           //処理の最大カウント

    //Start is called before the first frame update
    void Start() {
        RandomMax = 100.0f;
        RandomMin = 0f;
        Flameinitial = 0;
        Flame = Flameinitial;
        FlameMax = 5;
        Count = 0;
        CountMax = 100;
    }

    //Update is called once per frame
    void Update() {
        Flame++;
        if (Count < CountMax) {
            if (Flame > FlameMax) {
                //ランダム取得
                Random_1 = Random.Range(RandomMin, RandomMax);

                //品番を取得
                if (RandomMin <= Random_1 && (RandomMax / 3f) > Random_1) {
                    Number = 1;
                }
                else if ((RandomMax / 3f) <= Random_1 && ((RandomMax / 3f) * 2f) > Random_1) {
                    Number = 2;
                }
                else if (((RandomMax / 3f) * 2f) <= Random_1 && RandomMax >= Random_1) {
                    Number = 3;
                }

                //フレーム初期化
                Flame = Flameinitial;

                Count++;
            }
        }
        else if (Count == CountMax) {
            Road_Scene();
            Count++;
        }

    }

    void Road_Scene() {
        print("読んでる？");
    }
}
