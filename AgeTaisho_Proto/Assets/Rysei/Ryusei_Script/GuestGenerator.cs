using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestGenerator : MonoBehaviour {

    public int Trainnumber;         //客の種類
    public int Positionnumber;
    public int GuestSpawn;         //客生成の間隔

    public GameObject[] GuestType;  //客の種類
    public Vector3[] Position;      //席の番号(座標)
    //public bool []counter;        //席が埋まっているか判定
    public GameObject[] Guest;      //生成した客をいれる箱

    float time = 0f;                //時間を記録する小数も入る変数

    GameObject GM;
    GameManager script; //UnityChanScriptが入る変数


    void Start() {
        GuestSpawn = 12; //客生成の間隔　数値/秒
        GM = GameObject.Find("GameManager");
        script = GM.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;//毎フレームの時間を加算.

        if (script.FiverFlag) {
            GuestSpawn = 6;     //フィーバータイム時は6秒
        }
        else {
            GuestSpawn = 12;    //非フィーバー状態時は12秒
        }

        if (time >= GuestSpawn && Guest[Guest.Length - 1] == null)   //列の最後尾が埋まってなければGuestSpawn秒に１回処理
        {
            Trainnumber = Random.Range(0, GuestType.Length);    //客の種類をランダムにとる
            Guest[Guest.Length - 1] = Instantiate(GuestType[Trainnumber], Position[Position.Length - 1], transform.rotation);  //客生成(客番号,座標,回転)

            time = 0;
        }
    }
}
