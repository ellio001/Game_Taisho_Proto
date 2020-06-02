using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_Logo_Moving : MonoBehaviour
{
    public List<ParticleSystem> Particle_List = new List<ParticleSystem>();
    int cnt; // listの最後の要素数を入れる
    ParticleSystem.Burst burst;
    float Atai = 10;

    int count = 0;

    GameObject me; // 自分のオブジェクト取得用変数
    public float fadeStart = 1f; // フェード開始時間
    public bool fadeIn = true; // trueの場合はフェードイン
    public float fadeSpeed = 1f; // フェード速度指定


    void Start()
    {
        cnt = Particle_List.Count;// listの最後の要素数を入れている
        me = this.gameObject; // 自分のオブジェクト取得
    }

    void Update()
    {
        
        if (fadeStart > 0f)
            fadeStart -= Time.deltaTime;
        else
            if (fadeIn)
                fadeInFunc();

        // 徐々にエフェクトの数を減らしている
        count += 1;
        if (count % 10 == 0 && Atai !=0)
            Atai -= 0.5f;
        
        if(Atai == 0)
            for (int i = 0; i <= cnt; i++)
            {
                // 徐々に小さくしている
                Particle_List[i].startSize -= 0.001f;
            }
        

        for (int i = 0; i <= cnt; i++)
        {
            burst.count = Atai;
            Particle_List[i].emission.SetBurst(0, burst);
        }
        

    }


    void fadeInFunc()
    { // フェードインの処理
        if (me.GetComponent<Image>().color.a < 255)
        {
            UnityEngine.Color tmp = me.GetComponent<Image>().color;
            tmp.a = tmp.a + (Time.deltaTime * fadeSpeed);
            me.GetComponent<Image>().color = tmp;
        }
    }
}
