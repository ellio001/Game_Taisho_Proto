using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Fri_Fried : MonoBehaviour
{
    /*　変数　*/
    float count = 100;
    bool Powder_flg = false; // 徐々に泡が少なる処理を始めるフラグ
    bool Fried_flg = false;

    /*　Powderを揚げているときに使う変数　*/
    ParticleSystem.Burst burst;
    ParticleSystem PowderParticle; // PFFの<ParticleSystem>が入っている
    GameObject P_Effect; // プレファブを入れる
    Vector3 pos;         // 鍋に入った物の座標を入れる
    Quaternion rot;      // エフェクトの回転を入れる
    GameObject PFF; // 表示したエフェクトを入れる

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 当たった瞬間 ------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Nabe"))
        {
            P_Effect = (GameObject)Resources.Load("Effects/E_Frying");   //Resourceフォルダのプレハブを読み込む
            pos = this.gameObject.transform.position;      // 粉の座標代入
            rot = P_Effect.gameObject.transform.rotation;  // エフェクトの回転を代入
            PFF = Instantiate(P_Effect, new Vector3(pos.x, pos.y - 0.1f, pos.z), rot); // プレハブを元にオブジェクトを生成する

            PowderParticle = PFF.GetComponent<ParticleSystem>();

            PowderParticle.startLifetime = 0.5f; // エフェクトが上に行く距離
            PowderParticle.playbackSpeed = 2f; // エフェクトの再生速度
            burst.count = 2f; // 泡が同時に出る数
            PowderParticle.emission.SetBurst(0, burst);
           
            Fried_flg = true;
        }

    }

    // 当たっている間 ------------------------------------------
    void OnTriggerStay(Collider other)
    {
        

        if (other.gameObject.name.Contains("Nabe") && Fried_flg)
        {
            Debug.Log("kita");
            //PowderParticle.playbackSpeed = 2f;
            //burst.count = 1f;
            //PowderParticle.emission.SetBurst(0, burst);
        }

    }

    // 当たり終わった後 ------------------------------------------
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Nabe"))
        {
            Destroy(PowderParticle);
        }
    }
}
