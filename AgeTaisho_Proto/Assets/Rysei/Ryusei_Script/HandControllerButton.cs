using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerButton_S : MonoBehaviour
{
    //このスクリプトはControllerMouseClickと共存しない
    //このスクリプトはhandにいれる

    public GameObject clickedGameObject;
    public GameObject Resource;
    GameObject ClickObj;
    float handspeed = 0.1f;

    // Use this for initialization
    void Start()
    {
        ClickObj = GameObject.Find("ControllerObjClick");
        //clickedGameObject = null;   //対象を入れる箱を初期化
        //Resource = null;            //生成するプレハブの箱を初期化
    }

    void Update()
    {
        //右スティック
        if (Input.GetAxisRaw("R_Vertical") < 0 || Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * handspeed);
            //Debug.Log("R上");
        }
        else if (0 < Input.GetAxisRaw("R_Vertical") || Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.forward * -handspeed);
            //Debug.Log("R下");
        }
        else
        {
            //上下方向には傾いていない
        }

        if (Input.GetButtonUp("〇") || Input.GetKeyUp(KeyCode.Space)){
            clickedGameObject = null;   //対象を入れる箱を初期化
            Resource = null;            //生成するプレハブの箱を初期化
        }
    }

    void OnTriggerStay(Collider other)
    {
        if ((Input.GetButtonDown("〇") || Input.GetKeyDown(KeyCode.Space)) && clickedGameObject == null)
        {

            switch (other.gameObject.tag)
            {
                case "EbiBox":
                    Resource = (GameObject)Resources.Load("ItemEbi");   //Resourceフォルダのプレハブを読み込む
                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                    break;
                case "ChickenBox":
                    Resource = (GameObject)Resources.Load("ItemChicken");   //Resourceフォルダのプレハブを読み込む
                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                    break;
                case "FishBox":
                    Resource = (GameObject)Resources.Load("ItemFish");   //Resourceフォルダのプレハブを読み込む
                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                    break;
                case "PotatoBox":
                    Resource = (GameObject)Resources.Load("ItemPotato");   //Resourceフォルダのプレハブを読み込む
                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                    break;
                case "QuailBox":
                    Resource = (GameObject)Resources.Load("ItemQuail");   //Resourceフォルダのプレハブを読み込む
                    clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                    break;
                case "Item":
                    clickedGameObject = other.gameObject;                                //タグがなければオブジェクトをclickedGameObjectにいれる
                    clickedGameObject.transform.position = ClickObj.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
                    break;
                default:    //床や壁などをクリックしたらclickedGameObjectに何も入れない(null)
                    break;
            }
            if (clickedGameObject != null)  //nullでないとき処理
            {
                clickedGameObject.transform.parent = ClickObj.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
                clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
            }
        }
        //else if (Input.GetMouseButton(0) && clickedGameObject != null)
        //{
        //}
        else if ((Input.GetButtonUp("〇") && clickedGameObject != null) || (Input.GetKeyUp(KeyCode.Space) && clickedGameObject != null))
        {
            clickedGameObject.transform.parent = null;                          //親子付けを解除
            clickedGameObject.GetComponent<Rigidbody>().isKinematic = false;    //重力を有効
        }
    }
}


