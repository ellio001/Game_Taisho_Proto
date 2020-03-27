using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerButton_S2 : MonoBehaviour
{
    //このスクリプトはControllerMouseClickと共存しない
    //このスクリプトはhandにいれる

    public GameObject clickedGameObject;
    public GameObject Resource;
    private GameObject ClickObj;
    float handspeed = 0.1f;

    bool HoldingFlg;
    public Transform ClickObj2;

    void Start()
    {
        ClickObj = GameObject.Find("ControllerObjClick");
        HoldingFlg = false;
    }

    void Update()
    {

        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 5f))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (HoldingFlg != true) // 手に何も持っていない時に入る
                {
                    if (hit.collider.gameObject.tag == "Box")
                    {
                        switch (hit.collider.gameObject.name)
                        {
                            case "EbiBox":
                                Resource = (GameObject)Resources.Load("S_Resources/ItemEbi");   //Resourceフォルダのプレハブを読み込む
                                clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                HoldingFlg = true;
                                break;
                            case "ChickenBox":
                                Resource = (GameObject)Resources.Load("S_Resources/ItemChicken");   //Resourceフォルダのプレハブを読み込む
                                clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                HoldingFlg = true;
                                break;
                            case "FishBox":
                                Resource = (GameObject)Resources.Load("S_Resources/ItemFish");   //Resourceフォルダのプレハブを読み込む
                                clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                HoldingFlg = true;
                                break;
                            case "PotatoBox":
                                Resource = (GameObject)Resources.Load("S_Resources/ItemPotato");   //Resourceフォルダのプレハブを読み込む
                                clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                HoldingFlg = true;
                                break;
                            case "QuailBox":
                                Resource = (GameObject)Resources.Load("S_Resources/ItemQuail");   //Resourceフォルダのプレハブを読み込む
                                clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                                HoldingFlg = true;
                                break;
                            default:    //床や壁などをクリックしたらclickedGameObjectに何も入れない(null)
                                break;
                        }
                    }

                    if (hit.collider.gameObject.tag == "Item")
                    {
                        clickedGameObject = hit.collider.gameObject;                              //タグがなければオブジェクトをclickedGameObjectにいれる
                        clickedGameObject.transform.position = ClickObj.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
                        HoldingFlg = true;
                    }

                    //ClickObj2.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = false;
                }
                else if (hit.collider.gameObject.tag != "Item" && hit.collider.gameObject.tag != "Box" || clickedGameObject.name == "ItemChicken")
                // 粉や鍋にすでに食材があるなら食材を置けないようにしている(唐揚げは何個でも置ける)
                {
                    ClickObj2.GetChild(0).gameObject.transform.position = hit.point;
                    clickedGameObject.transform.parent = null;                          //親子付けを解除
                    clickedGameObject.GetComponent<Rigidbody>().isKinematic = false;    //重力を有効
                    clickedGameObject = null;   //対象を入れる箱を初期化
                    Resource = null;            //生成するプレハブの箱を初期化

                    HoldingFlg = false;
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

            }
        }

    }
}




