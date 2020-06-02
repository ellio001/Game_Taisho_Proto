using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGuestGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject Guest;   //チュートリアルのゲスト

    [System.NonSerialized] public float GuestSpawn;   //ゲストの出現時間
    public bool GuestCount = false;    //最初の客が出現したか

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GuestSpawn += Time.deltaTime;

        if (GuestSpawn >= 7 && !GuestCount) //GuestCoutで1回だけ呼ばれる処理
        {
            Instantiate(Guest, new Vector3(-6.0f, 5.0f, -9.0f), transform.rotation);
            GuestCount = true;
        }
    }
}
