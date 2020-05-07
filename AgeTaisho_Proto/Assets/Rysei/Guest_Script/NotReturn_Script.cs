using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NotReturn_Script : MonoBehaviour
{
    //金持ちステータス---------------------------------
    float EatTime = 1;          //食べ終わるまでの時間
    float RowTime = 180;         //列に並んでいる時間
    float SitTime = 180;         //席に座っている時間
    //-------------------------------------------------------
    Transform myTransform;
    public Vector3 pos;
    //public GameObject Plate1;
    bool Collider;  //Colliderにあたっているかあたっていないか
    float random;   //注文のランダム変数
    int flooredIntrandom;   //ランダムの変数を整数に変えて入れる箱
    float RandomMax = 10;    //ランダムの最大値を決める変数
    int WaitCount;  //客が帰るまでの時間
    [SerializeField] string []ItemString;   //Resourceのアイテム名の文字列を入れる箱
    [SerializeField] string []OrderString; //表示するアイテム名の文字列をいれる箱
    public string NumberString;
    public int ItemScore;   //アイテムの「スコア

    GameObject GuestGenerator;
    GuestGenerator Number;
    int MyNumber;   //列番号
    public GameObject[] GuestNumber; //列番号を入れる箱
    public Vector3[] GuestPosition; //座標番号を入れる箱
    [SerializeField] GameObject []OrderObject;   //注文を表示するTextの箱
    [SerializeField] Text [] OrderText;              //Textをいれる箱

    public float GuestSpeed;   //客の移動速度をいれる箱
    public Vector3 GuestNowPosition;   //客の現在位置の仮決定をいれる箱
    bool Order = false; //注文したかどうか
    bool Retrun = false;    //帰る処理になったかどうか
    public float ReturnCount;   //客が帰るまでの時間をいれる箱
    public float EatCount;      //客が食べている間の時間をいれる
    public bool OneProces = false; //自分のいた箱を1回だけ初期化する
    public bool Eat;            //提供された時trueにする

    string SceneName; // sceneの名前を記憶する変数

    void Start()
    {

        GuestGenerator = GameObject.Find("GuestGenerator"); //GuestGeneratorがはいったgameobject
        OrderObject[0] = this.gameObject.transform.Find("Canvas/Text1").gameObject; //子要素のtextを取得
        OrderObject[1] = this.gameObject.transform.Find("Canvas/Text2").gameObject; //子要素のtextを取得
        OrderObject[2] = this.gameObject.transform.Find("Canvas/Text3").gameObject; //子要素のtextを取得

        Number = GuestGenerator.GetComponent<GuestGenerator>();
        MyNumber = Number.Guest.Length - 1;   //自分の席番号を記憶する
        GuestNumber = Number.Guest; //GeneratorのGuestを獲得
        GuestPosition = Number.Position; //GeneratorのPositionを獲得
        GuestSpeed = 0.05f; //客の移動速度
        GuestNowPosition = this.gameObject.transform.position;

        //Plate1 = GameObject.Find("Plate1");
        Collider = false;
        //ItemString = null;

        myTransform = this.transform;           // transformを取得
        pos = myTransform.position;         // 座標を取得

        random = Random.value * RandomMax;          // ランダムな値を取得し5倍する(0~5の値をとるため)   0~1 0.2*5=1 0.2未満は0.99以下=小数点切り捨て

        while (random >= RandomMax)
        {    //後のswith文でRandomMax以上の値は使わないのでそれが入ったら値を取得しなおす
            random = Random.value * RandomMax;          // ランダムな値を取得しRandomMax倍する   3の場合0.33 0.66 0.99を3倍することで0.99 1.98 2.98小数点切り捨てで0~3となる
        }

        flooredIntrandom = (int)Mathf.Floor(random);        //5倍したランダムな値の小数点を切り捨てる(random自体の範囲0f~1.0f)

        OrderObject[0].SetActive(false);   //席につくまではオーダーを表示しない
        OrderObject[1].SetActive(false);   //席につくまではオーダーを表示しない
        OrderObject[2].SetActive(false);   //席につくまではオーダーを表示しない

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
            if (ReturnCount >= RowTime) GuestReturn(); //席につかず20秒たつとGuestReturnが呼ばれる
            //Debug.Log(LineReturn);
        }
        if (Eat)
        {
            EatCount += Time.deltaTime;
            if (EatCount >= EatTime)
            {
                EatCount = 0;
                Eat = false;
            }
            //if (EatCount >= EatTime) GuestReturn();   //5秒たったら食べ終わり帰る
        }
        if (GuestNowPosition.z >= -2 && Order == false)  //席に着いたら処理
        {

            ReturnCount = 0;    //客が帰るまでの時間を初期化
            Order = true;
            OrderObject[0].SetActive(true);    //オーダーを表示する
            OrderObject[1].SetActive(true);    //オーダーを表示する
            OrderObject[2].SetActive(true);    //オーダーを表示する


            ItemString[0] = "Dish_T_Shrimp";
            OrderString[0] = "えびてん";

            ItemString[1] = "Dish_T_Fish";
            OrderString[1] = "魚てん";

            ItemString[2] = "Dish_T_Potato";
            OrderString[2] = "芋てん";


            OrderText[0] = OrderObject[0].GetComponent<Text>();            // オブジェクトからTextコンポーネントを取得
            OrderText[1] = OrderObject[1].GetComponent<Text>();            // オブジェクトからTextコンポーネントを取得
            OrderText[2] = OrderObject[2].GetComponent<Text>();            // オブジェクトからTextコンポーネントを取得

            OrderText[0].text = OrderString[0];    // テキストの表示を入れ替える
            OrderText[1].text = OrderString[1];    // テキストの表示を入れ替える
            OrderText[2].text = OrderString[2];    // テキストの表示を入れ替える
        }
        else if (Order == true)
        {
            ReturnCount += Time.deltaTime;
            if (ReturnCount >= SitTime) GuestReturn(); //席について25秒たつとGuestReturnが呼ばれる
            if (ItemString[0] == null && ItemString[1] == null && ItemString[2] == null) GuestReturn();
        }

        if (GuestNowPosition.z <= -13) Destroy(gameObject);   //zが-13以下になったら消える
    }

    public void GuestReturn()  //客が帰る処理
    {
        GuestNowPosition.z -= GuestSpeed;   //-z方向に移動しつづける
        if (OneProces == false)
        {
            Number.Guest[MyNumber] = null;  //さっきまでいた席をnull
            OneProces = true;   //この処理が2回目以降通らないようにする
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == ItemString[0])
        {
            Eat = true;      //客が商品を食べ始める
            GameManager.instance.score_num += 100; //点数を加算する
            Destroy(other.gameObject);  //客が商品を食べる
            ItemString[0] = null;
            OrderObject[0].SetActive(false);   //席につくまではオーダーを表示しない
        }
        else if (other.gameObject.name == ItemString[1])
        {
            Eat = true;      //客が商品を食べ始める
            GameManager.instance.score_num += 100; //点数を加算する
            Destroy(other.gameObject);  //客が商品を食べる
            ItemString[1] = null;
            OrderObject[1].SetActive(false);   //席につくまではオーダーを表示しない
        }
        else if (other.gameObject.name == ItemString[2])
        {
            Eat = true;      //客が商品を食べ始める
            GameManager.instance.score_num += 100; //点数を加算する
            Destroy(other.gameObject);  //客が商品を食べる
            ItemString[2] = null;
            OrderObject[2].SetActive(false);   //席につくまではオーダーを表示しない
        }
    }
}
