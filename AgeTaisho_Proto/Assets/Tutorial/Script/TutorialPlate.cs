using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlate : MonoBehaviour
{

    GameObject Guest;
    GameObject GuestGenerator;
    TutorialGuestMove script;
    TutorialGuestGenerator T_Generator;
    private bool OneCount = false;

    // Use this for initialization
    void Start()
    {
        GuestGenerator = GameObject.Find("GuestGenerator"); //GuestGeneratorがはいったgameobject
        //Guest = GameObject.Find("Guest1");
        T_Generator = GuestGenerator.GetComponent<TutorialGuestGenerator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(T_Generator.GuestCount == true && !OneCount)
        {
            Guest = GameObject.Find("TutorialGuest(Clone)");
            script = Guest.GetComponent<TutorialGuestMove>();
            OneCount = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (script != null && other.gameObject.name == script.ItemString)
        {
            script.ReturnCount += 999;
            GameManager.instance.score_num += script.ItemScore; //点数を加算する
            Destroy(other.gameObject);  //客が商品を食べる

        }
    }
}