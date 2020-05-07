using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuestGenerator : MonoBehaviour
{

    public int Trainnumber;         //客の種類
    public int Positionnumber;
    public int GuestSpawn;         //客生成の間隔
    int GuestSpawnCount;            //客が生成された回数

    public GameObject[] GuestType;  //客の種類
    public Vector3[] Position;      //席の番号(座標)
    //public bool []counter;        //席が埋まっているか判定
    public GameObject[] Guest;      //生成した客をいれる箱
    bool FirstGuest;    //最初の客は12秒

    float time = 0f;                //時間を記録する小数も入る変数

    GameObject GM;
    GameManager script; //UnityChanScriptが入る変数


    void Start()
    {
        GuestSpawn = 15; //最初の客生成の間隔　数値/秒
        FirstGuest = false;
        GM = GameObject.Find("GameManager");
        script = GM.GetComponent<GameManager>();
        GuestSpawnCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;//毎フレームの時間を加算.

        if (script.FiverFlag)
        {
            GuestSpawn = 5;     //フィーバータイム時は5秒
        }
        else if (FirstGuest == true)
        {
            GuestSpawn = 8;    //非フィーバー状態時は8秒
        }

        if (time >= GuestSpawn && Guest[Guest.Length - 1] == null)   //列の最後尾が埋まってなければGuestSpawn秒に１回処理
        {

            GuestSpawnCount += 1;

            if (GuestSpawnCount % 7 == 0) Guest[Guest.Length - 1] = Instantiate(GuestType[5], Position[Position.Length - 1], transform.rotation);  //客生成(客番号,座標,回転)
            else if (GuestSpawnCount % 10 == 0) Guest[Guest.Length - 1] = Instantiate(GuestType[4], Position[Position.Length - 1], transform.rotation);  //客生成(客番号,座標,回転)
            else if (GuestSpawnCount % 6 == 0) Guest[Guest.Length - 1] = Instantiate(GuestType[3], Position[Position.Length - 1], transform.rotation);  //客生成(客番号,座標,回転)
            else if (SceneManager.GetActiveScene().name == "Ryusei_Scene" || SceneManager.GetActiveScene().name == "Easy_Scene")
            {
                Trainnumber = Random.Range(0, 2);    //客の種類をランダムにとる
                Guest[Guest.Length - 1] = Instantiate(GuestType[Trainnumber], Position[Position.Length - 1], transform.rotation);  //客生成(客番号,座標,回転)
            }
            else
            {
                Trainnumber = Random.Range(0, 3);    //客の種類をランダムにとる
                Guest[Guest.Length - 1] = Instantiate(GuestType[Trainnumber], Position[Position.Length - 1], transform.rotation);  //客生成(客番号,座標,回転)
            }

            FirstGuest = true;
            //Trainnumber = Random.Range(0, GuestType.Length);    //客の種類をランダムにとる
            //Guest[Guest.Length - 1] = Instantiate(GuestType[Trainnumber], Position[Position.Length - 1], transform.rotation);  //客生成(客番号,座標,回転)

            time = 0;

            Debug.Log("Spawn"+GuestSpawnCount);
            Debug.Log(Trainnumber);
        }
    }
}
