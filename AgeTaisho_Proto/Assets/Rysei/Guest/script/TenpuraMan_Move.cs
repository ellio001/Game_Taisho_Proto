using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TenpuraMan_Move : MonoBehaviour {
    //天ぷらマンステータス---------------------------------
    float EatTime = 2;          //食べ終わるまでの時間
    float RowTime = 20;         //列に並んでいる時間
    float SitTime = 30;         //席に座っている時間
    int Mistake = 5;            //間違えた時の時間の減量

    float RandomMax = 3;    //ランダムの最大値を決める変数
    //-------------------------------------------------------
    Transform myTransform;
    public Vector3 pos;
    //public GameObject Plate1;
    bool Collider;  //Colliderにあたっているかあたっていないか
    float random;   //注文のランダム変数
    int flooredIntrandom;   //ランダムの変数を整数に変えて入れる箱
    int WaitCount;  //客が帰るまでの時間
    public string ItemString;   //Resourceのアイテム名の文字列を入れる箱
    private string OrderString; //表示するアイテム名の文字列をいれる箱
    public string NumberString;
    public int ItemScore;   //アイテムの「スコア

    GameObject GuestGenerator;
    GuestGenerator Number;
    int MyNumber;   //列番号
    public GameObject[] GuestNumber; //列番号を入れる箱
    public Vector3[] GuestPosition; //座標番号を入れる箱
    GameObject Panel;         //客についてるパネル
    [SerializeField] GameObject[] OrderItems;   //席についたとき注文

    public float GuestSpeed;   //客の移動速度をいれる箱
    public Vector3 GuestNowPosition;   //客の現在位置の仮決定をいれる箱
    bool Order = false; //注文したかどうか
    bool Retrun = false;    //帰る処理になったかどうか
    public float ReturnCount;   //客が帰るまでの時間をいれる箱
    public float EatCount;      //客が食べている間の時間をいれる
    public bool OneProces = false; //自分のいFた箱を1回だけ初期化する
    public bool Eat;            //提供された時trueにする
    bool OneDelete;             //注文を１回だけ消す

    string SceneName; // sceneの名前を記憶する変数

    //GameObject Display;
    //SideDisplay S_Display;
    [SerializeField] GameObject[] SideOrder;        //プレハブをいれる
    [SerializeField] Vector3[] DisplayPosition;     //位置をいれる
    [SerializeField] GameObject[] SideItems;        //シーン上に置くアイテムをいれる
    [SerializeField] Image ReturnImage;           //客が帰るまでのゲージ
    [SerializeField] Text ReturnText;               //客が帰るまでの秒数を表示するテキスト
    int ReturnTime;                                 //客が帰るまでの秒数

    /*　パーティクル変数　*/
    bool effectflag = false;    //エフェクト始めるフラグ
    bool effectflag_angry = false;    //エフェクト始めるフラグ
    bool angryflag = false;             //怒っているか判定フラグ
    
    /*　パーティクル情報　*/
    //親子付けオブジェ
    private GameObject Child;
    // プレファブを入れる
    GameObject obj_Tave;
    GameObject obj_Heart;
    GameObject obj_Angry;
    // 鍋に入った物の座標を入れる
    Vector3 eff_pos;
    // エフェクトの回転を入れる
    Quaternion eff_rot;
    // 表示したエフェクトを入れる
    GameObject eff_Tabe;
    GameObject eff_Heart;
    GameObject eff_Angry;

    //オーディオ
    AudioSource[] sounds;

    void Start() {
        this.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        GuestGenerator = GameObject.Find("GuestGenerator"); //GuestGeneratorがはいったgameobject
        //Display = GameObject.Find("SideDisplay"); //ディスプレイの追加
        Panel = this.gameObject.transform.Find("Canvas/Panel").gameObject; //子要素のPanelを取得
        Number = GuestGenerator.GetComponent<GuestGenerator>();
        //S_Display = Display.GetComponent<SideDisplay>();    //SideDysplayスクリプトの追加
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

        random = Random.value * RandomMax;          // ランダムな値を取得し5倍する(0~5の値をとるため)   0~1 0.2*5=1 0.2未満は0.99以下=小数点切り捨て

        while (random >= RandomMax) {    //後のswith文でRandomMax以上の値は使わないのでそれが入ったら値を取得しなおす
            random = Random.value * RandomMax;          // ランダムな値を取得しRandomMax倍する   3の場合0.33 0.66 0.99を3倍することで0.99 1.98 2.98小数点切り捨てで0~3となる
        }

        flooredIntrandom = (int)Mathf.Floor(random);        //5倍したランダムな値の小数点を切り捨てる(random自体の範囲0f~1.0f)

        Panel.SetActive(false);   //席につくまではパネルを表示しない
        OrderItems[0].SetActive(false);   //席につくまではパネルを表示しない
        OrderItems[1].SetActive(false);   //席につくまではパネルを表示しない
        OrderItems[2].SetActive(false);   //席につくまではパネルを表示しない
        ReturnImage.enabled = false;      //帰るゲージをfalseに
        ReturnText.enabled = false;      //テキストをfalseに
        GetComponent<BoxCollider>().enabled = false;

        //オーディオの情報取得
        sounds = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        if (Order == false && OneProces == false) //席についていない間実行
        {
            while (MyNumber >= 4 && GuestNumber[MyNumber - 1] == null) //4番地以上 かつ 1こ前の番地が空いていたら処理
            {
                MyNumber -= 1;  //番地を-1する
                GuestNumber[MyNumber] = this.gameObject;    //1つ前の番地にコピー
                GuestNumber[MyNumber + 1] = null;   //前いた番地に残ったコピーを初期化
                //Debug.Log(MyNumber);
            }

            if (MyNumber == 3 && (GuestNowPosition.x >= GuestPosition[3].x - 0.2)) //客がPosition[3]の許容範囲より左にきたら1~3へ通す
            {
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
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

            if (GuestNowPosition.x < GuestPosition[MyNumber].x - 0.03) GuestNowPosition.x += GuestSpeed;   //目的地よりz座標が小さければ-
            else if (GuestNowPosition.x > GuestPosition[MyNumber].x + 0.03) GuestNowPosition.x -= GuestSpeed; //目的地よりz座標が大きければ+
            if (GuestNowPosition.z < GuestPosition[MyNumber].z - 0.03) GuestNowPosition.z += GuestSpeed;   //目的地よりx座標が小さければ-
            else if (GuestNowPosition.z > GuestPosition[MyNumber].z + 0.03) GuestNowPosition.z -= GuestSpeed; //目的地よりx座標が大きければ+
        }

        this.gameObject.transform.position = GuestNowPosition;  //現在の位置を更新

        if (MyNumber >= 3) {
            ReturnCount += Time.deltaTime;
            if (ReturnCount >= RowTime) GuestReturn(); //席につかず20秒たつとGuestReturnが呼ばれる
            //Debug.Log(LineReturn);
        }
        if (Eat) {

            //エフェクトスタート
            if (!effectflag) Start_Effect();

            EatCount += Time.deltaTime;
            if (OneDelete == false) {
                Panel.SetActive(false);   //パネルを表示しない
                OrderItems[0].SetActive(false);
                OrderItems[1].SetActive(false);
                OrderItems[2].SetActive(false);
                Destroy(SideItems[0]);
                Destroy(SideItems[1]);
                ReturnImage.enabled = false;      //Imageをfalseに
                ReturnText.enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                OneDelete = true;
            }
            if (EatCount >= EatTime) GuestReturn();   //2秒たったら食べ終わり帰る
        }
        if (GuestNowPosition.z >= -2.1 && Order == false)  //席に着いたら処理
        {
            GuestNowPosition.y -= 0.5f;    //客を沈める(後でけす)
            ReturnCount = 0;    //客が帰るまでの時間を初期化
            Order = true;
            Panel.SetActive(true);   //パネルを表示する
            ReturnImage.enabled = true;      //Imageを表示する
            ReturnText.enabled = true;      //Textを表示する
            GetComponent<BoxCollider>().enabled = true;

            switch (flooredIntrandom) {
                case 0:
                    ItemScore = 100;
                    ItemString = "Dish_T_Shrimp"; //*(エビ、魚、ポテトの処理が同じなので) 後々エビフライを入れる
                    OrderString = "えびてん";
                    OrderItems[0].SetActive(true);
                    SideItems[0] = Instantiate(OrderItems[0], DisplayPosition[MyNumber], Quaternion.Euler(0, 90, 0));  //客生成(客番号,座標,回転)
                    SideItems[1] = Instantiate(OrderItems[0], DisplayPosition[MyNumber + 3], Quaternion.Euler(0, 90, 0));  //客生成(客番号,座標,回転)
                    SideItems[0].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    SideItems[1].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    break;
                case 1:
                    ItemScore = 100;
                    ItemString = "Dish_T_Fish"; //*(エビ、魚、ポテトの処理が同じなので) 後々魚フライを入れる
                    OrderString = "魚てん";
                    OrderItems[1].SetActive(true);
                    SideItems[0] = Instantiate(OrderItems[1], DisplayPosition[MyNumber], Quaternion.Euler(0, 90, 0));  //客生成(客番号,座標,回転)
                    SideItems[1] = Instantiate(OrderItems[1], DisplayPosition[MyNumber + 3], Quaternion.Euler(0, 90, 0));  //客生成(客番号,座標,回転)
                    SideItems[0].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    SideItems[1].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    break;
                case 2:
                    ItemScore = 100;
                    ItemString = "Dish_T_Potato"; //*(エビ、魚、ポテトの処理が同じなので) 後々ポテトフライを入れる
                    OrderItems[2].SetActive(true);
                    OrderString = "芋てん";
                    SideItems[0] = Instantiate(OrderItems[2], DisplayPosition[MyNumber], Quaternion.Euler(0, 90, 0));  //客生成(客番号,座標,回転)
                    SideItems[1] = Instantiate(OrderItems[2], DisplayPosition[MyNumber + 3], Quaternion.Euler(0, 90, 0)); //客生成(客番号,座標,回転)
                    SideItems[0].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    SideItems[1].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    break;
            }

        }
        else if (Order == true) {
            //帰る時間を加算
            ReturnCount += Time.deltaTime;
            ReturnTime = (int)SitTime - (int)ReturnCount;
            ReturnImage.fillAmount = 1 - ((ReturnCount / SitTime));
            ReturnText.text = "" + ReturnTime;
            //席について30秒たつとGuestReturnが呼ばれる
            if (ReturnCount >= SitTime) {
                GuestReturn();
                angryflag = true;
            }
        }

        if (GuestNowPosition.x >= 5) {
            //xが10以上になったら消える
            Destroy(gameObject);
        }
    }

    public void GuestReturn()  //客が帰る処理
    {
        
        //Angry'effectStart
        if (!effectflag_angry && angryflag) Start_Effect_Angry();

        if (GuestNowPosition.z > -3) {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            GuestNowPosition.z -= GuestSpeed;   //少し後ろに下がり
        }
        else {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            GuestNowPosition.x += GuestSpeed;   //-左に帰っていく
        }
        if (OneProces == false) {
            if (Order == true) GuestNowPosition.y += 0.5f;  //席に着いたとき沈めた客を戻す
            Number.Guest[MyNumber] = null;  //さっきまでいた席をnull
            GuestNumber[MyNumber] = null;   //ジェネレータの箱？
            if (OneDelete == false) {
                Panel.SetActive(false);   //パネルを表示しない
                OrderItems[0].SetActive(false);
                OrderItems[1].SetActive(false);
                OrderItems[2].SetActive(false);
                Destroy(SideItems[0]);
                Destroy(SideItems[1]);
                ReturnImage.enabled = false;
                ReturnText.enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                OneDelete = true;
            }
            OneProces = true;   //この処理が2回目以降通らないようにする
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == ItemString) {
            ReturnCount = 0; //客が帰るまでの時間を初期化
            Eat = true;      //客が商品を食べ始める
            GameManager.instance.score_num += ItemScore; //点数を加算する
            Destroy(other.gameObject);  //客が商品を食べる
        }
        else {
            ReturnCount += Mistake;
        }
    }

    /// <summary>
    /// ///エフェクトスタート
    /// </summary>
    //エフェクトが生成、スタート
    void Start_Effect() {
        //Resourceフォルダのプレハブを読み込む
        obj_Tave = (GameObject)Resources.Load("Effects/E_Taveru");
        obj_Heart = (GameObject)Resources.Load("Effects/E_Heart");

        //座標
        eff_pos = gameObject.transform.position;
        //角度
        eff_rot = gameObject.transform.rotation;
        //生成
        eff_Tabe = Instantiate(obj_Tave, new Vector3(eff_pos.x, eff_pos.y + 1.7f, eff_pos.z + 0.4f), eff_rot);
        eff_Heart = Instantiate(obj_Heart, new Vector3(eff_pos.x, eff_pos.y + 2.0f, eff_pos.z),
            new Quaternion(eff_rot.x - 1f, eff_rot.y, eff_rot.z, eff_rot.w));
        //エフェクト停止
        End_Effect();
        //サウンド再生
        Start_Sound();
        //二度読み防止
        effectflag = true;
    }

    //AngryModeスタート
    void Start_Effect_Angry() {
        //Resourceフォルダのプレハブを読み込む
        obj_Angry = (GameObject)Resources.Load("Effects/E_Angry");
        //座標
        eff_pos = gameObject.transform.position;
        //角度
        eff_rot = Quaternion.identity;
        //生成
        eff_Angry = Instantiate(obj_Angry, new Vector3(eff_pos.x, eff_pos.y + 2.0f, eff_pos.z), eff_rot);
        //親子付け
        Child = eff_Angry;
        Child.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Child.transform.parent = gameObject.transform;
        //エフェクト停止
        End_Effect_Angry();
        //二度読み防止
        effectflag_angry = true;
        angryflag = false;
        //サウンド再生
        Start_Sound_Angry();
    }


    /// <summary>
    /// ///エフェクトエンド
    /// </summary>
    //エフェクトを停止消去
    void End_Effect() {
        Destroy(eff_Tabe, 3f);
        Destroy(eff_Heart, 3f);
        //二度読み防止
        effectflag = false;
    }

    //AngryMode終了
    void End_Effect_Angry() {
        Destroy(eff_Angry, 3f);
        //二度読み防止
        effectflag_angry = false;
    }

    //食べた音
    void Start_Sound() {
        //サウンド再生
        sounds[0].Play();
    }

    //怒った音
    void Start_Sound_Angry() {
        //サウンド再生
        sounds[1].Play();
    }
}
