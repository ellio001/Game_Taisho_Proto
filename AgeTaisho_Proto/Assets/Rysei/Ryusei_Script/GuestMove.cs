using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestMove : MonoBehaviour
{

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
    int MyNumber;   //列番号
    public GameObject[] GuestNumber; //列番号を入れる箱
    public Vector3[] GuestPosition; //座標番号を入れる箱

    public float GuestSpeed;   //客の移動速度をいれる箱
    public Vector3 GuestNowPosition;   //客の現在位置の仮決定をいれる箱
    bool Order = false; //注文したかどうか
    bool Retrun = false;    //帰る処理になったかどうか
    public float ReturnCount;   //客が帰るまでの時間をいれる箱
    public bool OneProces = false; //自分のいた箱を1回だけ初期化する

    void Start()
    {

        GuestGenerator = GameObject.Find("GuestGenerator"); //GuestGeneratorがはいったgameobject
        Number = GuestGenerator.GetComponent<GuestGenerator>();
        MyNumber = Number.Guest.Length - 1;   //自分の席番号を記憶する
        GuestNumber = Number.Guest; //GeneratorのGuestを獲得
        GuestPosition = Number.Position; //GeneratorのPositionを獲得
        GuestSpeed = 0.05f; //客の移動速度
        GuestNowPosition = this.gameObject.transform.position;

        //Plate1 = GameObject.Find("Plate1");
        Collider = false;
        ItemString = null;

        myTransform = this.transform;           // transformを取得
        pos = myTransform.position;         // 座標を取得

        random = Random.value * 5;          // ランダムな値を取得し5倍する(0~5の値をとるため)

        while (random >= 5)
        {    //後のswith文で5以上の値は使わないのでそれが入ったら値を取得しなおす
            random = Random.value * 5;          // ランダムな値を取得し10倍する
        }

        flooredIntrandom = (int)Mathf.Floor(random);        //5倍したランダムな値の小数点を切り捨てる(random自体の範囲0f~1.0f)
    }

    // Update is called once per frame
    void Update()
    {

        if (Order == false && OneProces == false) //席についていない間実行
        {
            while (MyNumber >= 3 && GuestNumber[MyNumber - 1] == null) //3番地以上 かつ 1こ前の番地が空いていたら処理
            {
                MyNumber -= 1;  //番地を-1する
                GuestNumber[MyNumber] = this.gameObject;    //1つ前の番地にコピー
                GuestNumber[MyNumber + 1] = null;   //前いた番地に残ったコピーを初期化
                //Debug.Log(MyNumber);
            }

            if (MyNumber == 3)
            {
                if (GuestNumber[0] == null) MyNumber = 0; //0の席が空いていたら移動
                else if (GuestNumber[1] == null) MyNumber = 1; //1の席が空いていたら移動
                else if (GuestNumber[2] == null) MyNumber = 2; //2の席が空いていたら移動

                //Debug.Log(MyNumber);

                switch (MyNumber)   //列から席へ移る処理
                {
         
                    case 0:
                        GuestNumber[MyNumber] = this.gameObject;    //0の番地にコピー
                        GuestNumber[3] = null;   //前いた番地に残ったコピーを初期化
                        break;
                    case 1:
                        GuestNumber[MyNumber] = this.gameObject;    //1の番地にコピー
                        GuestNumber[3] = null;   //前いた番地に残ったコピーを初期化
                        break;
                    case 2:
                        GuestNumber[MyNumber] = this.gameObject;    //2の番地にコピー
                        GuestNumber[3] = null;   //前いた番地に残ったコピーを初期化
                        break;
                }
            }

            if (GuestNowPosition.x < GuestPosition[MyNumber].x) GuestNowPosition.x += GuestSpeed;   //目的地よりz座標が小さければ-
            else if (GuestNowPosition.x > GuestPosition[MyNumber].x) GuestNowPosition.x -= GuestSpeed; //目的地よりz座標が大きければ+
            if (GuestNowPosition.z < GuestPosition[MyNumber].z) GuestNowPosition.z += GuestSpeed;   //目的地よりx座標が小さければ-
            else if (GuestNowPosition.z > GuestPosition[MyNumber].z) GuestNowPosition.z -= GuestSpeed; //目的地よりx座標が大きければ+
        }

        this.gameObject.transform.position = GuestNowPosition;  //現在の位置を更新

        if (MyNumber >= 3)
        {
            ReturnCount += Time.deltaTime;
            if (ReturnCount >= 8) GuestReturn(); //席につかず8秒たつとGuestReturnが呼ばれる
            //Debug.Log(LineReturn);
        }
        if (GuestNowPosition.z >= -2 && Order == false)  //席に着いたら処理
        {

            //ReturnCount = 14;    //客が席に着いてから帰るまでの時間
            ReturnCount = 0;
            Order = true;

            switch (flooredIntrandom)
            {
                case 0:
                    ItemScore = 100;
                    ItemString = "ItemSara(Tenpura)"; //*(エビ、魚、ポテトの処理が同じなので) 後々エビフライを入れる
                    break;
                case 1:
                    ItemScore = 100;
                    ItemString = "ItemSara(Tenpura)"; //*(エビ、魚、ポテトの処理が同じなので) 後々魚フライを入れる
                    break;
                case 2:
                    ItemScore = 100;
                    ItemString = "ItemSara(Tenpura)"; //*(エビ、魚、ポテトの処理が同じなので) 後々ポテトフライを入れる
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
            switch (MyNumber)
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
            Debug.Log(NumberString + "の席に" + ItemString + "の注文が入った");
        }
        else if (Order == true)
        {
            ReturnCount += Time.deltaTime;
            if (ReturnCount >= 18) GuestReturn(); //席につかず8秒たつとGuestReturnが呼ばれる
        }

        if (GuestNowPosition.z <= -13) Destroy(gameObject);   //zが-13以下になったら消える
    }

    public void GuestReturn()  //客が帰る処理
    {
        GuestNowPosition.z -= GuestSpeed;   //-z方向に移動しつづける
        if(OneProces == false) Number.Guest[MyNumber] = null;  //さっきまでいた席をnull
        OneProces = true;   //上の処理が2回目以降通らないようにする
    }
}
