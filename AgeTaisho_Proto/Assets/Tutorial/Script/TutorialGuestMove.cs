using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialGuestMove : MonoBehaviour
{

    Transform myTransform;
    public Vector3 pos;

    bool Collider;  //Colliderにあたっているかあたっていないか
    int flooredIntrandom;   //ランダムの変数を整数に変えて入れる箱
    int WaitCount;  //客が帰るまでの時間
    public string ItemString;   //Resourceのアイテム名の文字列を入れる箱
    private string OrderString; //表示するアイテム名の文字列をいれる箱
    public string NumberString;
    public int ItemScore;   //アイテムの「スコア

    int MyNumber;   //列番号
    public GameObject[] GuestNumber; //列番号を入れる箱
    public Vector3[] GuestPosition; //座標番号を入れる箱
    GameObject OrderObject;   //注文を表示するTextの箱

    public float GuestSpeed;   //客の移動速度をいれる箱
    public Vector3 GuestNowPosition;   //客の現在位置の仮決定をいれる箱
    bool Order = false; //注文したかどうか
    bool Retrun = false;    //帰る処理になったかどうか
    public float ReturnCount;   //客が帰るまでの時間をいれる箱
    public bool OneProces = false; //自分のいた箱を1回だけ初期化する

    void Start()
    {
        OrderObject = this.gameObject.transform.Find("Canvas/Text").gameObject; //子要素のtextを取得
        GuestSpeed = 0.05f; //客の移動速度
        GuestNowPosition = this.gameObject.transform.position;

        Collider = false;
        ItemString = null;

        myTransform = this.transform;           // transformを取得
        pos = myTransform.position;         // 座標を取得

        OrderObject.SetActive(false);   //席につくまではオーダーを表示しない
    }

    // Update is called once per frame
    void Update()
    {
        if (GuestNowPosition.z <= -2 && OneProces == false)
        {
            GuestNowPosition.z += GuestSpeed;   //-z方向に移動しつづける
        }

        this.gameObject.transform.position = GuestNowPosition;  //現在の位置を更新

        if (GuestNowPosition.z >= -2 && Order == false)  //席に着いたら処理
        {

            ReturnCount = 0;    //客が帰るまでの時間を初期化
            Order = true;
            OrderObject.SetActive(true);    //オーダーを表示する

            switch (SceneManager.GetActiveScene().name)
            {
                case "Ryusei_Scene":
                    ItemScore = 100;
                    ItemString = "ItemSara(Tenpura)"; //*(エビ、魚、ポテトの処理が同じなので) 後々エビフライを入れる
                    OrderString = "えびてん";
                    break;
                case "Tutorial_Scene":
                    ItemScore = 100;
                    ItemString = "Tutorial_ItemSara(Tenpura)"; //*(エビ、魚、ポテトの処理が同じなので) 後々エビフライを入れる
                    OrderString = "えびてん";
                    Debug.Log("tutorial");
                    break;
            }
            Text OrderText = OrderObject.GetComponent<Text>();            // オブジェクトからTextコンポーネントを取得
            OrderText.text = OrderString;    // テキストの表示を入れ替える
        }
        else if (Order == true)
        {
            ReturnCount += Time.deltaTime;
            if (ReturnCount >= 999) GuestReturn(); //席について999秒たつとGuestReturnが呼ばれる
        }

        if (GuestNowPosition.z <= -13) Destroy(gameObject);   //zが-13以下になったら消える
    }

    public void GuestReturn()  //客が帰る処理
    {
        GuestNowPosition.z -= GuestSpeed;   //-z方向に移動しつづける
        if (OneProces == false)
        {
            OrderObject.SetActive(false);    //オーダーを非表示にする
            OneProces = true;   //この処理が2回目以降通らないようにする
        }
    }
}
