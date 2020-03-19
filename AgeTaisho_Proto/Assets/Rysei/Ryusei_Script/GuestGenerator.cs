using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestGenerator : MonoBehaviour {

    public int Trainnumber;
    public int Positionnumber;
    private int GuestSpawn;

    public GameObject[] Train;  //客の種類
    public Vector3[] Position;  //席の番号(座標)
    public bool []counter;  //席が埋まっているか判定
    public GameObject[] Guest;

    float time = 0f;//時間を記録する小数も入る変数.

    void Start () {
        GuestSpawn = 12;
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;//毎フレームの時間を加算.
        if(time >= GuestSpawn)   //数値秒に１回処理
        {
            Trainnumber = Random.Range(0, Train.Length);    //客の種類をランダムにとる
            Positionnumber = Random.Range(0, Position.Length);  //席番号をランダムにとる
            if (counter[Positionnumber] == false)
            {
                Guest[Positionnumber] = Instantiate(Train[Trainnumber], Position[Positionnumber], transform.rotation);  //客生成
                counter[Positionnumber] = true; //席が埋まってるときはtrue
            }
            time = 0;
        }
    }
}
