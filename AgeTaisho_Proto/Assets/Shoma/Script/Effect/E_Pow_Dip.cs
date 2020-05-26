using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Pow_Dip : MonoBehaviour
{
    /*　変数　*/
    float count = 100;
    bool Powder_flg = false; // 徐々に泡が少なる処理を始めるフラグ
    bool Fried_flg = false;

    /*　Powderにつける時に使う変数　*/
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
        if (other.gameObject.name.Contains("Item") || other.gameObject.name.Contains("Powder"))
        {
            P_Effect = (GameObject)Resources.Load("Effects/E_Dip");   //Resourceフォルダのプレハブを読み込む
            pos = other.gameObject.transform.position;      // 粉の座標代入
            rot = P_Effect.gameObject.transform.rotation;  // エフェクトの回転を代入
            PFF = Instantiate(P_Effect, new Vector3(pos.x, pos.y - 0.1f, pos.z), rot); // プレハブを元にオブジェクトを生成する

            PowderParticle = PFF.GetComponent<ParticleSystem>();
        }

    }

}
