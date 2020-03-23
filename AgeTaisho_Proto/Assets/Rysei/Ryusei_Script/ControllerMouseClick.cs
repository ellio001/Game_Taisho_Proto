using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMouseClick : MonoBehaviour {

    //このスクリプトはHandControllerButtonと共存しない
    GameObject clickedGameObject;
    GameObject Resource;
    GameObject gameobj;
    GameObject hand;
    GameObject DropPosition;
    float handspeed = 0.1f;

    // Use this for initialization
    void Start()
    {
        gameobj = this.gameObject;
        hand = GameObject.Find("hand");
        DropPosition = GameObject.Find("DropPosition");
    }

    void Update()
    {

        //if (Input.GetMouseButtonDown(0))
        if (Input.GetButtonDown("〇") || Input.GetKeyDown(KeyCode.Space))
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
                        clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        break;
                    case "FriedchickenBox":
                    case "ChickenBox":
                        Resource = (GameObject)Resources.Load("ItemChicken");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        break;
                    case "FishBox":
                        Resource = (GameObject)Resources.Load("ItemFish");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        break;
                    case "PotatoBox":
                        Resource = (GameObject)Resources.Load("ItemPotato");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        break;
                    case "QuailBox":
                        Resource = (GameObject)Resources.Load("ItemQuail");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
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
                    //hand.gameObject.SetActive(false);   //物を持ったら手を非表示
                    clickedGameObject.transform.parent = this.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
                    clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
                }
            }
        }
        //else if (Input.GetMouseButtonUp(0) && clickedGameObject != null)    //nullでないとき処理
        else if ((Input.GetButtonUp("〇") && clickedGameObject != null) || (Input.GetKeyUp(KeyCode.Space) && clickedGameObject != null))
        {
            //hand.gameObject.SetActive(true);    //ボタンを離した時手を表示
            clickedGameObject.transform.parent = null;                          //親子付けを解除
            clickedGameObject.GetComponent<Rigidbody>().isKinematic = false;    //重力を有効
            clickedGameObject.transform.position = DropPosition.transform.position; //DropPositionの位置にドロップ
        }

        //右スティック
        if (Input.GetAxisRaw("R_Vertical") < 0)
        {
            //Debug.Log("上に傾いている");
            //hand.transform.position += new Vector3(0, 0, 0.1f);
            hand.transform.Translate(Vector3.forward * handspeed);
            //Debug.Log("R上");
            //cam.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
            //x -= Time.deltaTime * 100;
            //transform.rotation = Quaternion.Euler(y, 0, 0);

        }
        else if (0 < Input.GetAxisRaw("R_Vertical"))
        {

            //Debug.Log("下に傾いている");
            //hand.transform.position += new Vector3(0, 0, -0.1f);
            hand.transform.Translate(Vector3.forward * -handspeed);
            //Debug.Log("R下");
            //x += Time.deltaTime * 100;
            //transform.rotation = Quaternion.Euler(y, 0, 0);
        }
        else
        {
            //上下方向には傾いていない
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (Input.GetButtonDown("〇") || Input.GetKeyDown(KeyCode.Space))
    //    {
    //        clickedGameObject = null;   //対象を入れる箱を初期化
    //        Resource = null;            //生成するプレハブの箱を初期化

    //        switch (other.gameObject.tag)
    //        {

    //                case "EbiBox":
    //                    Resource = (GameObject)Resources.Load("ItemEbi");   //Resourceフォルダのプレハブを読み込む
    //                    clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
    //                break;
    //                case "ChickenBox":
    //                    Resource = (GameObject)Resources.Load("ItemChicken");   //Resourceフォルダのプレハブを読み込む
    //                    clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
    //                    break;
    //                case "FishBox":
    //                    Resource = (GameObject)Resources.Load("ItemFish");   //Resourceフォルダのプレハブを読み込む
    //                    clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
    //                    break;
    //                case "PotatoBox":
    //                    Resource = (GameObject)Resources.Load("ItemPotato");   //Resourceフォルダのプレハブを読み込む
    //                    clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
    //                    break;
    //                case "QuailBox":
    //                    Resource = (GameObject)Resources.Load("ItemQuail");   //Resourceフォルダのプレハブを読み込む
    //                    clickedGameObject = Instantiate(Resource, this.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
    //                    break;
    //                case "Item":
    //                    clickedGameObject = other.gameObject;                                //タグがなければオブジェクトをclickedGameObjectにいれる
    //                    clickedGameObject.transform.position = this.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
    //                    break;
    //                default:    //床や壁などをクリックしたらclickedGameObjectに何も入れない(null)
    //                    break;
    //            }
    //            if (clickedGameObject != null)  //nullでないとき処理
    //            {
    //                clickedGameObject.transform.parent = this.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
    //                clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
    //            }
    //    }
    //    else if ((Input.GetButtonUp("〇") && clickedGameObject != null) || (Input.GetKeyUp(KeyCode.Space) && clickedGameObject != null))
    //    {
    //        Debug.Log("Spase 〇　入力");
    //        clickedGameObject.transform.parent = null;                          //親子付けを解除
    //        clickedGameObject.GetComponent<Rigidbody>().isKinematic = false;    //重力を有効
    //    }
    //}
}

