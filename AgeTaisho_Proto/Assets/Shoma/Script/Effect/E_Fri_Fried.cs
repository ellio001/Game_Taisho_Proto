using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Fri_Fried : MonoBehaviour
{
    /*　変数　*/
    bool Fried_flg = false;

    /*　Powderを揚げているときに使う変数　*/
    ParticleSystem.Burst burst;
    ParticleSystem PowderParticle; // PFFの<ParticleSystem>が入っている
    GameObject P_Effect; // プレファブを入れる
    Vector3 pos;         // 鍋に入った物の座標を入れる
    Quaternion rot;      // エフェクトの回転を入れる
    GameObject PFF; // 表示したエフェクトを入れる

    GameObject HCB;
    HandControllerButton_S2 HCBscript;

    void Start()
    {
        HCB = GameObject.Find("hand");
        HCBscript = HCB.GetComponent<HandControllerButton_S2>();
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

            if(this.gameObject.name.Contains("_T_"))HCBscript.AgeCount += 1;
            else if(this.gameObject.name.Contains("_K_"))HCBscript.AgeCount2 += 1;

        }
    }

    // 当たっている間 ------------------------------------------
    void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.name.Contains("Nabe")&& Fried_flg)
        {
            Destroy(PowderParticle);
        }

    }

    // 出た瞬間 ------------------------------------------
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Nabe")){
            if (this.gameObject.name.Contains("Burn"))
            {
                switch (this.gameObject.name)
                {
                    case "Burn_Shrimp":
                    case "Burn_Fish":
                    case "Burn_Potato":
                        HCBscript.AgeCount -= 1;
                        break;
                    case "Burn_chicken":
                    case "Burn_Quail":
                        HCBscript.AgeCount2 -= 1;
                        break;
                }
            }
            switch (this.gameObject.name)
            {
                case "Fried_T_Shrimp":
                case "Fried_T_Potato":
                case "Fried_T_Fish":
                    HCBscript.AgeCount -= 1;
                    break;
                case "Fried_K_Chicken":
                case "Fried_K_Quail":
                    HCBscript.AgeCount2 -= 1;
                    break;
            }
        }
    }

    private void OnDestroy()
    {

        Destroy(GameObject.Find("E_Frying(Clone)"));
    }
}
