using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestMove : MonoBehaviour {

    Transform myTransform;
    public Vector3 pos;
    //public GameObject Plate1;
    bool Collider;  //Colliderにあたっているかあたっていないか
    float random;   //注文のランダム変数
    int flooredIntrandom;   //ランダムの変数を整数に変えて入れる箱
    int WaitCount;  //客が帰るまでの時間
    public string ItemString;   //アイテム名の文字列を入れる箱
    public string NumberString;
    public int ItemScore;   //アイテムの「スコア

    GameObject GuestGenerator;
    GuestGenerator Number;
    int MyNumber;

    // Use this for initialization
    void Start () {

        GuestGenerator = GameObject.Find("GuestGenerator"); //GuestGeneratorがはいったgameobject
        Number = GuestGenerator.GetComponent<GuestGenerator>();
        MyNumber = Number.Positionnumber;   //自分の席番号を記憶する

        //Plate1 = GameObject.Find("Plate1");
        Collider = false;
        ItemString = null;

        myTransform = this.transform;           // transformを取得
        pos = myTransform.position;         // 座標を取得

        random = Random.value * 5;          // ランダムな値を取得し5倍する(0~5の値をとるため)

        while(random >= 5) {    //後のswith文で5以上の値は使わないのでそれが入ったら値を取得しなおす
            random = Random.value * 5;          // ランダムな値を取得し10倍する
        }

        flooredIntrandom = (int)Mathf.Floor(random);        //5倍したランダムな値の小数点を切り捨てる(random自体の範囲0f~1.0f)
    }
	
	// Update is called once per frame
	void Update () {

        if (WaitCount / 2.0f >= 1000) {
            pos.z -= 0.02f;    // z座標へ0.01減算
        }
        else if(Collider == true)
        {
            WaitCount++;
        }
        else if (Collider == false)
        {
            pos.z += 0.02f;    // z座標へ0.01加算
        }

        myTransform.position = pos;  // 座標を設定
        if (gameObject.transform.position.z <= -13)
        {
            Number.Guest[MyNumber] = null;
            Number.counter[MyNumber] = false; //席が埋まってるときはtrue
            Destroy(gameObject);    //z座標が-13以下になったら(客が帰ったら)消滅する
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Collider = true;

        switch (flooredIntrandom)
        {
            case 0:
                ItemScore = 100;
                ItemString = "ItemSara(Tenpura)"; //*(エビ、魚、ポテトの処理が同じなので) 後々エビフライを入れる
                break;
            case 1:
                ItemScore = 100;
                ItemString = "ItemSara(Tenpura)"; // * 後々魚フライをいれる
                break;
            case 2:
                ItemScore = 100;
                ItemString = "ItemSara(Tenpura)"; // * 後々ポテトフライをいれる
                break;
            case 3:
                ItemScore = 100;
                ItemString = "ItemSara(Chicken)";
                break;
            case 4:
                ItemScore = 100;
                ItemString = "ItemSara(Quail)";
                break;
        }
        switch(MyNumber)
        {
            case 0:
                NumberString = "左";
                break;
            case 1:
                NumberString = "真ん中";
                break;
            case 2:
                NumberString = "右";
                break;
        }
        Debug.Log(NumberString + "の席に" +ItemString + "の注文が入った");
    }

    private void OnTriggerExit(Collider other)
    {
        Collider = false;
    }
}
