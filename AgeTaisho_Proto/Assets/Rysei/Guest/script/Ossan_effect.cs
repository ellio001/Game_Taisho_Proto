using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ossan_effect : MonoBehaviour {
    /*　変数　*/
    float count = 100;
    bool Powder_flg = false; // 徐々に泡が少なる処理を始めるフラグ
    bool Fried_flg = false;

    /*　おっさんが食べているときに使う変数　*/
    ParticleSystem.Burst burst;
    ParticleSystem PowderParticle;  // PFFの<ParticleSystem>が入っている
    GameObject P_Effect;            // プレファブを入れる
    Vector3 pos;                    // 鍋に入った物の座標を入れる
    Quaternion rot;                 // エフェクトの回転を入れる
    GameObject PFF;                 // 表示したエフェクトを入れる

    /*　定数　*/
    const float DECREASE = 1f; // ここをいじれば減る量が変わる

    void Start() {

    }

    void Update() {

    }

    void PowderFride() // Powderを鍋に入れたときに読み込む
    {
        Powder_flg = true;
        burst.count = 100f;
        PowderParticle.emission.SetBurst(0, burst);
        PowderParticle.playbackSpeed = 2f;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("Nabe")) {
            P_Effect = (GameObject)Resources.Load("Effects/E_Frying");   //Resourceフォルダのプレハブを読み込む
            pos = this.gameObject.transform.position;      // 粉の座標代入
            rot = P_Effect.gameObject.transform.rotation;  // エフェクトの回転を代入
            PFF = Instantiate(P_Effect, new Vector3(pos.x, pos.y - 0.15f, pos.z), rot); // プレハブを元にオブジェクトを生成する

            PowderParticle = PFF.GetComponent<ParticleSystem>();

            PowderParticle.startLifetime = 1f; // エフェクトが上に行く距離
            PowderParticle.playbackSpeed = 8f; // エフェクトの再生速度
            burst.count = 10f; // 泡が同時に出る数
            PowderParticle.emission.SetBurst(0, burst);
            Invoke("PowderFride", 0.1f);
            count = 100;
        }

    }

    // 当たっている間 ------------------------------------------
    void OnTriggerStay(Collider other) {
        if (other.gameObject.name.Contains("Nabe") && Powder_flg) {
            if (count > 10) {
                if (PowderParticle.startLifetime > 0.5f) PowderParticle.startLifetime -= 0.015f;
                count -= DECREASE;
            }
            burst.count = count;
            PowderParticle.emission.SetBurst(0, burst);
        }
    }

    // 粉から揚げに変わった時にエフェクトを削除する
    private void OnDestroy() {
        Destroy(GameObject.Find("E_Frying(Clone)"));
    }
}
