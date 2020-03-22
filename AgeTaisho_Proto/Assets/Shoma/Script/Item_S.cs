using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_S : MonoBehaviour {

    private int agecount;
    private bool kona;
    private bool BredPowder;
    private bool liquid;
    private bool QuailFry;
    private bool Secondliquid;
    private bool ThirdBreadPowder;
    float alpha_Sin;    //オブジェクト発光の間隔(Sin波)

    GameObject gameobject;
    GameObject Resource;
    GameObject objcolor;
    GameObject dummy;

    // Use this for initialization
    void Start () {
        gameobject = this.gameObject;   //このオブジェクトの情報をいれる
        gameObject.name = gameObject.name.Replace("(Clone)", ""); //プレハブ生成時の(Clone)を消す
        Resource = null;            //生成するプレハブの箱を初期化
        agecount = 0;               //カウント初期化
        kona = false;               //konaをfalseに
        BredPowder = false;
        liquid = false;
        QuailFry = false;
        Secondliquid = false;
        ThirdBreadPowder = false;
        dummy = GameObject.Find("dummy");

    }
	
	// Update is called once per frame
	void Update () {
        alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;    //Sin波
        //Debug.Log("液"+liquid);
        //Debug.Log("粉"+BredPowder);
        //Debug.Log("揚"+QuailFry);
    }

    private void OnTriggerStay(Collider other)
    {

        switch (gameobject.name)
        {
            case "ItemKoge":
                if (other.gameObject.tag == "Garbage can")
                {
                    Destroy(gameObject);
                }
                break;
            case "ItemPotato":
            case "ItemFish":
            case "ItemEbi":
                if (kona == false)
                {
                   objcolor = GameObject.Find("kona");
                }

                if (other.gameObject.tag == "kona")
                {
                    kona = true;
                    GetComponent<Renderer>().material.color = Color.white;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("tenpuranabe");
                }
                else if (kona == true && other.gameObject.tag == "tenpuranabe")
                {

                    agecount++;

                    if (agecount >= 150)
                    {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("S_Resources/ItemTenpura");   //Resourceフォルダのプレハブを読み込む
                        Debug.Log("揚がった");
                    }
                }
                else if(other.gameObject.tag != "Click")
                {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);
                break;

            case "ItemTenpura":
                if (other.gameObject.tag == "tenpuranabe") agecount++;
                if (other.gameObject.tag == "Garbage can") Destroy(gameObject);

                if (agecount >= 150)
                {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                    Debug.Log("焦げた");
                }
                break;
            case "ItemChicken":

                objcolor = GameObject.Find("karaagenabe");

                if (other.gameObject.tag == "Garbage can") Destroy(gameObject);
                if (other.gameObject.tag == "karaagenabe")
                {
                    agecount++;

                    if (agecount >= 150)
                    {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("S_Resources/ItemFriedchicken");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag != "Click")
                {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);
                break;
            case "ItemFriedchicken":
                if (other.gameObject.tag == "karaagenabe") agecount++;
                if (other.gameObject.tag == "Garbage can") Destroy(gameObject);
                if (agecount >= 150)
                {
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                    Debug.Log("焦げた");
                }
                break;

            case "ItemQuail":
                if (other.gameObject.tag == "Garbage can") Destroy(gameObject);
                if (BredPowder == false)
                {
                    objcolor = GameObject.Find("BreadPowder");
                }
                if (QuailFry == true && other.gameObject.tag == "karaagenabe")
                {
                    agecount++;

                    if (agecount >= 150)
                    {
                        objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        objcolor = dummy;
                        Resource = (GameObject)Resources.Load("S_Resources/ItemQuailFry");   //Resourceフォルダのプレハブを読み込む
                    }
                }
                else if (other.gameObject.tag == "Bread powder" && ThirdBreadPowder == true)
                {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                }
                else if (other.gameObject.tag == "liquid" && Secondliquid == true)
                {
                    ThirdBreadPowder = true;
                    QuailFry = true;
                    GetComponent<Renderer>().material.color = Color.blue;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("karaagenabe");
                    Debug.Log("液2回目");
                }
                else if (other.gameObject.tag == "Bread powder" && BredPowder == true && liquid == true)
                {
                    Secondliquid = true;
                    GetComponent<Renderer>().material.color = Color.red;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("liquid");
                    Debug.Log("粉2回目");
                }
                else if (other.gameObject.tag == "Bread powder")
                {
                    BredPowder = true;
                    GetComponent<Renderer>().material.color = Color.red;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("liquid");
                    Debug.Log("粉1回目");
                }
                else if(BredPowder == true && other.gameObject.tag == "liquid")
                {
                    //BredPowder = false;
                    liquid = true;
                    GetComponent<Renderer>().material.color = Color.blue;
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = GameObject.Find("BreadPowder");
                    Debug.Log("液1回目");
                }
                else if (other.gameObject.tag != "Click") //OnCollisionStayだと1液が処理された瞬間呼ばれてしまう
                {
                    objcolor.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                    objcolor = dummy;
                    Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                    Debug.Log("失敗");
                }
                objcolor.GetComponent<Renderer>().material.color = new Color(alpha_Sin, alpha_Sin, alpha_Sin);
                break;

            case "ItemQuailFry":
                if (other.gameObject.tag == "Garbage can") Destroy(gameObject);
                if (other.gameObject.tag == "karaagenabe")
                {
                    agecount++;

                    if (agecount >= 150)
                    {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemKoge");   //Resourceフォルダのプレハブを読み込む
                        Debug.Log("焦げた");
                    }
                }
                break;
            default:
                //case文にあるもの以外の場合
                break;

        }   //switc文終了
        if (Resource != null)
        {
            Destroy(gameObject);    //今あるオブジェクトを消す
            gameobject = (GameObject)Instantiate(Resource, this.gameObject.transform.position, this.gameObject.transform.rotation); //焼きあがった(焦げた)オブジェクト生成
        }
    }
}