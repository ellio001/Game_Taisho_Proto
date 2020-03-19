using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseObjClick : MonoBehaviour {

    GameObject clickedGameObject;
    GameObject Resource;

    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            clickedGameObject = null;   //対象を入れる箱を初期化
            Resource = null;            //生成するプレハブの箱を初期化

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                switch (hit.collider.gameObject.tag)
                {

                    case "EbiBox":
                        Resource = (GameObject)Resources.Load("ItemEbi");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = (GameObject)Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        break;
                    case "FriedchickenBox":
                        Resource = (GameObject)Resources.Load("Itemchicken");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = (GameObject)Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        break;
                    case "FishBox":
                        Resource = (GameObject)Resources.Load("ItemFish");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = (GameObject)Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        break;
                    case "PotatoBox":
                        Resource = (GameObject)Resources.Load("ItemPotato");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = (GameObject)Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        break;
                    case "QuailBox":
                        Resource = (GameObject)Resources.Load("ItemQuail");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = (GameObject)Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        break;
                    case "Item":
                        clickedGameObject = hit.collider.gameObject;                                //タグがなければオブジェクトをclickedGameObjectにいれる
                        clickedGameObject.transform.position = this.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
                        break;
                    default:    //床や壁などをクリックしたらclickedGameObjectに何も入れない(null)
                        break;
                }
                if (clickedGameObject != null)  //nullでないとき処理
                {
                    clickedGameObject.transform.parent = this.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
                    clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && clickedGameObject != null)    //nullでないとき処理
        {
            clickedGameObject.transform.parent = null;                          //親子付けを解除
            clickedGameObject.GetComponent<Rigidbody>().isKinematic = false;    //重力を有効
        }
    }
}
