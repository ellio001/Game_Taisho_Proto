using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate0 : MonoBehaviour
{

    GameObject Guest;
    GameObject GuestGenerator;
    GuestMove script;
    GuestGenerator Number;

    bool provide;

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
        if (Number.counter[0] == true)
        {
            Guest = Number.Guest[0];
            script = Guest.GetComponent<GuestMove>();
        }
        else if (Number.counter[0] == false)
        {
            provide = false;
        }
        if (provide == true)
        {
            script.pos.z -= 0.04f;    // z座標へ0.01加算
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (script != null && other.gameObject.name == script.ItemString)
        {
            provide = true;
            GameManager.instance.score_num += script.ItemScore;
            Destroy(other.gameObject);  //客が商品を食べる
        }
    }
}
