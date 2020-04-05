using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate2 : MonoBehaviour
{

    GameObject Guest;
    GameObject GuestGenerator;
    GuestMove script;
    GuestGenerator Number;

    // Use this for initialization
    void Start()
    {
        GuestGenerator = GameObject.Find("GuestGenerator"); //GuestGeneratorがはいったgameobject
        //Guest = GameObject.Find("Guest1");
        Number = GuestGenerator.GetComponent<GuestGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Number.Guest[2] != null)    //常に2の席に誰がいるかみる
        {
            Guest = Number.Guest[2];
            script = Guest.GetComponent<GuestMove>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (script != null && other.gameObject.name == script.ItemString)
        {
            script.ReturnCount += 100;
            GameManager.instance.score_num += script.ItemScore; //点数を加算する
            Destroy(other.gameObject);  //客が商品を食べる

        }
    }
}