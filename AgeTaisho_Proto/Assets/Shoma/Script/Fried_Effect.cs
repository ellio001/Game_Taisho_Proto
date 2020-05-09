using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fried_Effect : MonoBehaviour
{
    /*　変数　*/
    float count = 100;
    bool Powder_flg = false; // 徐々に泡が少なる処理を始めるフラグ
    bool Fried_flg = false;

    /*　定数　*/
    const float DECREASE = 1f; // ここをいじれば減る量が変わる

    /*　Particle　*/
    public ParticleSystem ps;
    ParticleSystem.Burst burst;
    ParticleSystem PowderParticle;

    void Start()
    {
        burst.count = 300f; // 泡が同時に出る数
        ps.emission.SetBurst(0, burst);

    }

    void Update()
    {
        if (Powder_flg)
        {
            if (count > 10)
            {
                if (PowderParticle.startLifetime > 0.5f) PowderParticle.startLifetime -= 0.015f;
                 count -= DECREASE;
            }
            burst.count = count;
            ps.emission.SetBurst(0, burst);
        }
        else if (Fried_flg)
        {
            PowderParticle.playbackSpeed = 2f;
            //burst.count = count;
            ps.emission.SetBurst(0, burst);
            Debug.Log("揚げ物になった");
        }
    }

    void PowderFride()
    {
        Powder_flg = true;
        burst.count = 100f;
        //PowderParticle.startLifetime = 0.9f;
        PowderParticle.playbackSpeed = 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Powder"))
        {

            var Effect = (GameObject)Resources.Load("Effects/Powder_Fride");   //Resourceフォルダのプレハブを読み込む
            Vector3 pos = other.gameObject.transform.position;      // 粉の座標代入
            Quaternion rot = Effect.gameObject.transform.rotation;  // エフェクトの回転を代入
            var PowderFride = Instantiate(Effect, new Vector3(pos.x, pos.y - 0.15f, pos.z), rot); // プレハブを元にオブジェクトを生成する

            PowderParticle = PowderFride.GetComponent<ParticleSystem>();

            PowderParticle.startLifetime = 1f; // エフェクトが上に行く距離
            PowderParticle.playbackSpeed = 8f; // エフェクトの再生速度
            //burst.count = 300f; // 泡が同時に出る数
            //ps.emission.SetBurst(0, burst);
            Invoke("PowderFride", 0.1f);
        }

        if (other.gameObject.name.Contains("Fried"))
        {
            Powder_flg = false;
            Fried_flg = true;
            burst.count = 1f; // 泡が同時に出る数
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.name.Contains("Powder"))
    //    {
            
    //    }
    //    if (other.gameObject.name.Contains("Fried"))
    //    {
    //    }
    //}
}
