using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials_HandControllerButton : MonoBehaviour
{
    //このスクリプトはControllerMouseClickと共存しない
    //このスクリプトはhandにいれる

    public GameObject clickedGameObject;
    public GameObject Resource;
    private GameObject ClickObj;
    float handspeed = 0.1f;

    public bool HoldingFlg;
    //ColliderFlagの説明
    /* 0はEbiBox
     * 1はChickenBox
     * 2FishBox
     * 3PotatoBox
     * 4QuailBox*/
    public int ColliderFlag;
    public Transform ClickObj2;
    private Transform TmpFood; // 手に持っている物を入れる
    private GameObject TmpObj; // レイで当たっているObjを入れる

    GameObject C2;    // Camera_2を入れる変数
    Camera_2 C2_script; // Camera_2のscriptを入れる変数

    bool ItemSara;  //アイテム名にSaraが含まれているか判定

    public TutorialUI tutorialUI;
    int TextNumber;

    void Start()
    {
        ClickObj = GameObject.Find("ControllerObjClick");
        HoldingFlg = false;
        ColliderFlag = 0;

        C2 = GameObject.Find("Main Camera");
        C2_script = C2.GetComponent<Camera_2>();
    }

    void Update()
    {
        TextNumber = tutorialUI.TextNumber; // TextNumberの値を常に更新
        Debug.Log("TextNumber"+TextNumber);

        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 天ぷらが生成されたら一度だけ次のテキストに進む
        if (GameObject.Find("ItemTenpura") && TextNumber == 6) tutorialUI.TextNumber = 7; // テキストを進める

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 5f) && Input.GetKeyDown(KeyCode.Space) && C2_script.space_flg)
        {
            if (HoldingFlg != true) // 手に何も持っていない時に入る
            {
                if (hit.collider.gameObject.tag == "Box")
                {
                    if (hit.collider.gameObject.name == "EbiBox" && TextNumber == 3)
                    {
                        Resource = (GameObject)Resources.Load("S_Resources/ItemEbi");   //Resourceフォルダのプレハブを読み込む
                        clickedGameObject = Instantiate(Resource, ClickObj.gameObject.transform.position, Quaternion.identity); // プレハブを元にオブジェクトを生成する
                        HoldingFlg = true;
                        ColliderFlag = 0;
                        tutorialUI.TextNumber = 4; // テキストを進める
                        //当たり判定をを外す
                        ColliderOut();
                    }
                    ItemSara = hit.collider.gameObject.name.Contains("Sara");
                }

                if (hit.collider.gameObject.tag == "Item" && (TextNumber != 6 && TextNumber != 9) )
                {
                    clickedGameObject = hit.collider.gameObject;                              //タグがなければオブジェクトをclickedGameObjectにいれる
                    clickedGameObject.transform.position = ClickObj.gameObject.transform.position;  //オブジェクトを目の前に持ってくる
                    HoldingFlg = true;
                    ItemSara = hit.collider.gameObject.name.Contains("Sara");

                    //当たり判定をを外す
                    ColliderOut();
                }

            }
            else if ((TextNumber == 4 && hit.collider.gameObject.tag == "kona") || (TextNumber == 5 && hit.collider.gameObject.tag == "tenpuranabe")||
                     (TextNumber == 7 && hit.collider.gameObject.tag == "Sara") || (TextNumber == 8 && hit.collider.gameObject.name == "Plate2"))
            // 粉や鍋にすでに食材があるなら食材を置けないようにしている(唐揚げは何個でも置ける)
            {
                //当たり判定を入れる
                ColliderIn();
                ClickObj2.GetChild(0).gameObject.transform.position = hit.point; // 見ているところに置く
                clickedGameObject.transform.parent = null;              //手との親子付けを解除

                clickedGameObject.GetComponent<Rigidbody>().isKinematic = false;    //重力を有効

                clickedGameObject = null;   //対象を入れる箱を初期化
                Resource = null;            //生成するプレハブの箱を初期化

                HoldingFlg = false;
                tutorialUI.TextNumber += 1; // テキストを進める
            }
            if (clickedGameObject != null)  //nullでないとき処理
            {
                clickedGameObject.transform.parent = ClickObj.gameObject.transform; //このスクリプトが入っているオブジェクトと親子付け
                clickedGameObject.GetComponent<Rigidbody>().isKinematic = true; //ヒットしたオブジェクトの重力を無効
            }
            
        }

    }

    //当たり判定を切る関数
    void ColliderOut()
    {
        clickedGameObject.GetComponent<Collider>().enabled = false;
    }

    //当たり判定を入れる関数
    void ColliderIn()
    {
        clickedGameObject.GetComponent<Collider>().enabled = true;
    }
}




