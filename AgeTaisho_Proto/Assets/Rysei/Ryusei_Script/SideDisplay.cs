using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDisplay : MonoBehaviour
{
    [SerializeField] GameObject GuestGenerator; //GuestGenerator取得
    GuestGenerator GGScript;    //GuestGeneratorのスクリプト
    int SeatNumber;             //席の番号

    [SerializeField] GameObject[] Guest;         //3席のゲストを入れる箱
    [SerializeField] GameObject[] DisplayItems;  //3席の注文アイテム(6)を入れる箱
    string GuestName;           //客の名前判定
    [SerializeField] string []OrderString;         //注文した商品名
    [SerializeField] GameObject []OrderItems;        //アイテムのゲームオブジェクト
    public Vector3[] DisplayPosition;  //各ディスプレイの位置座標

    [SerializeField] bool []Seatbool;


    [SerializeField] float []time; //けす
    //-----------------------------------------------------------
    //それぞれの客のスクリプトを入れる箱
    TenpuraMan_Move TenpuraMan;
    Custmer_script Custmer;
    NotReturn_Script NotReturn_Man;
    Obaachan_script Obaachan;
    Ossan_script Ossan;
    Rich_Script Rich;
    //-----------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        GGScript = GuestGenerator.GetComponent<GuestGenerator>();
        Seatbool[0] = false;
        Seatbool[1] = false;
        Seatbool[2] = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (GGScript.Guest[0] != null && Seatbool[0] == false)    //常に0の席に誰がいるかみる
        //{
        //    time[0] += Time.deltaTime; //けす
        //    if (time[0] > 0.7) {         //けす
        //        Debug.Log("とおった");
        //        SeatNumber = 0;

        //        Guest[0] = GGScript.Guest[0];   //0番席の情報をGuest[0]にいれる
        //        GuestName = Guest[0].name;      //Guest[0]の客の名前をいれる
        //        GuestScriptGet();               //どの客が来たのか名前で判定しスクリプトゲット
        //        DisplayPicture();               //ゲットしたスクリプトのItemStringを取得
        //        Seatbool[0] = true;    //1回だけ処理を通す
        //    }                       //けす
        //}
        //else if (GGScript.Guest[0] == null && Seatbool[0] == true)
        //{
        //    time[0] = 0;    ///けす
        //    Guest[0] = null;
        //    Seatbool[0] = false;
        //    Debug.Log("Guest[0]がnull");
        //}

        //if (GGScript.Guest[1] != null && Seatbool[1] == false)    //常に1の席に誰がいるかみる
        //{
        //    time[1] += Time.deltaTime; //けす
        //    if (time[1] > 0.7)
        //    {         //けす
        //        Debug.Log("とおった");
        //        SeatNumber = 1;

        //        Guest[1] = GGScript.Guest[1];   //0番席の情報をGuest[1]にいれる
        //        GuestName = Guest[1].name;      //Guest[1]の客の名前をいれる
        //        GuestScriptGet();               //どの客が来たのか名前で判定しスクリプトゲット
        //        DisplayPicture();               //ゲットしたスクリプトのItemStringを取得

        //        Seatbool[1] = true;    //1回だけ処理を通す
        //    }                       //けす
        //}
        //else if (GGScript.Guest[1] == null && Seatbool[1] == true)
        //{
        //    time[1] = 0;    //けす
        //    Guest[1] = null;
        //    Seatbool[1] = false;
        //    Debug.Log("Guest[1]がnull");
        //}

        //if (GGScript.Guest[2] != null && Seatbool[2] == false)    //常に1の席に誰がいるかみる
        //{
        //    time[2] += Time.deltaTime; //けす
        //    if (time[2] > 0.7)
        //    {         //けす
        //        Debug.Log("とおった");
        //        SeatNumber = 2;

        //        Guest[2] = GGScript.Guest[2];   //0番席の情報をGuest[1]にいれる
        //        GuestName = Guest[2].name;      //Guest[1]の客の名前をいれる
        //        GuestScriptGet();               //どの客が来たのか名前で判定しスクリプトゲット
        //        DisplayPicture();               //ゲットしたスクリプトのItemStringを取得

        //        Seatbool[2] = true;    //1回だけ処理を通す
        //    }                       //けす
        //}
        //else if (GGScript.Guest[2] == null && Seatbool[2] == true)
        //{
        //    time[2] = 0;    //けす
        //    Guest[2] = null;
        //    Seatbool[2] = false;
        //    Debug.Log("Guest[2]がnull");
        //}
    }

    public void StartSideDisplay()
    {
        if (GGScript.Guest[0] != null && Seatbool[0] == false)    //常に0の席に誰がいるかみる
        {

                Debug.Log("とおった");
                SeatNumber = 0;

                //Guest[0] = GGScript.Guest[0];   //0番席の情報をGuest[0]にいれる
                GuestName = GGScript.Guest[0].name;      //Guest[0]の客の名前をいれる
                GuestScriptGet();               //どの客が来たのか名前で判定しスクリプトゲット
                DisplayPicture();               //ゲットしたスクリプトのItemStringを取得
                Seatbool[0] = true;    //1回だけ処理を通す

        }
        else if (GGScript.Guest[0] == null && Seatbool[0] == true)
        {
            //Guest[0] = null;
            Seatbool[0] = false;
            Debug.Log("Guest[0]がnull");
        }

        if (GGScript.Guest[1] != null && Seatbool[1] == false)    //常に1の席に誰がいるかみる
        {
                Debug.Log("とおった");
                SeatNumber = 1;

                //Guest[1] = GGScript.Guest[1];   //0番席の情報をGuest[1]にいれる
                GuestName = GGScript.Guest[1].name;      //Guest[1]の客の名前をいれる
                GuestScriptGet();               //どの客が来たのか名前で判定しスクリプトゲット
                DisplayPicture();               //ゲットしたスクリプトのItemStringを取得

                Seatbool[1] = true;    //1回だけ処理を通す
        }
        else if (GGScript.Guest[1] == null && Seatbool[1] == true)
        {
            //Guest[1] = null;
            Seatbool[1] = false;
            Debug.Log("Guest[1]がnull");
        }

        if (GGScript.Guest[2] != null && Seatbool[2] == false)    //常に1の席に誰がいるかみる
        {
                Debug.Log("とおった");
                SeatNumber = 2;

                //Guest[2] = GGScript.Guest[2];   //0番席の情報をGuest[1]にいれる
                GuestName = GGScript.Guest[2].name;      //Guest[1]の客の名前をいれる
                GuestScriptGet();               //どの客が来たのか名前で判定しスクリプトゲット
                DisplayPicture();               //ゲットしたスクリプトのItemStringを取得

                Seatbool[2] = true;    //1回だけ処理を通す
        }
        else if (GGScript.Guest[2] == null && Seatbool[2] == true)
        {
            //Guest[2] = null;
            Seatbool[2] = false;
            Debug.Log("Guest[2]がnull");
        }
    }

    void GuestScriptGet() //客の名前からスクリプトの箱を判定
    {
        switch (GuestName)
        {
            case "Guest0(Clone)":
                TenpuraMan = GGScript.Guest[SeatNumber].GetComponent<TenpuraMan_Move>();
                OrderString[SeatNumber] = TenpuraMan.ItemString;
                break;
            case "Guest1(Clone)":
                Ossan = GGScript.Guest[SeatNumber].GetComponent<Ossan_script>();
                OrderString[SeatNumber] = Ossan.ItemString;
                break;
            case "Guest2(Clone)":
                Custmer = GGScript.Guest[SeatNumber].GetComponent<Custmer_script>();
                OrderString[SeatNumber] = Custmer.ItemString;
                break;
            case "Guest3(Clone)":
                Obaachan = GGScript.Guest[SeatNumber].GetComponent<Obaachan_script>();
                OrderString[SeatNumber] = Obaachan.ItemString;
                break;
            case "Guest4(Clone)":
                NotReturn_Man = GGScript.Guest[SeatNumber].GetComponent<NotReturn_Script>();
                //OrderString[0] = NotReturn_Man.ItemString;
                break;
            case "Guest5(Clone)":
                Rich = GGScript.Guest[SeatNumber].GetComponent<Rich_Script>();
                OrderString[SeatNumber] = Rich.ItemString;
                break;
            default:
                break;
        }
    }

    void DisplayPicture()
    {
        switch(OrderString[SeatNumber])
        {
            case "Dish_T_Shrimp":
                DisplayItems[SeatNumber] =Instantiate(OrderItems[0], DisplayPosition[SeatNumber], transform.rotation);  //客生成(客番号,座標,回転)
                DisplayItems[SeatNumber+3] = Instantiate(OrderItems[0], DisplayPosition[SeatNumber + 3], transform.rotation);  //客生成(客番号,座標,回転)
                break;
            case "Dish_T_Fish":
                DisplayItems[SeatNumber] = Instantiate(OrderItems[1], DisplayPosition[SeatNumber], transform.rotation);  //客生成(客番号,座標,回転)
                DisplayItems[SeatNumber + 3] = Instantiate(OrderItems[1], DisplayPosition[SeatNumber + 3], transform.rotation);  //客生成(客番号,座標,回転)
                break;
            case "Dish_T_Potato":
                DisplayItems[SeatNumber] = Instantiate(OrderItems[2], DisplayPosition[SeatNumber], transform.rotation);  //客生成(客番号,座標,回転)
                DisplayItems[SeatNumber + 3] = Instantiate(OrderItems[2], DisplayPosition[SeatNumber + 3], transform.rotation);  //客生成(客番号,座標,回転)
                break;
            case "ItemSara(Chicken)":
                DisplayItems[SeatNumber] = Instantiate(OrderItems[3], DisplayPosition[SeatNumber], transform.rotation);  //客生成(客番号,座標,回転)
                DisplayItems[SeatNumber + 3] = Instantiate(OrderItems[3], DisplayPosition[SeatNumber + 3], transform.rotation);  //客生成(客番号,座標,回転)
                break;
            case "Dish_K_Quail":
                DisplayItems[SeatNumber] = Instantiate(OrderItems[4], DisplayPosition[SeatNumber], transform.rotation);  //客生成(客番号,座標,回転)
                DisplayItems[SeatNumber + 3] = Instantiate(OrderItems[4], DisplayPosition[SeatNumber + 3], transform.rotation);  //客生成(客番号,座標,回転)
                break;
            default:
                Debug.Log("とおらないはず");
                break;
        }
    }
}
