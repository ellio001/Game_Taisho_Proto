using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_Zoom : MonoBehaviour
{
    Vector3 nowPosi;
    float UpDownSpeed = 0.3f;   // 上下の移動幅の値
    int count = 0;

    void Start()
    {
        //nowPosi = this.transform.position;
    }

    void Update()
    {
        // その場で上下運動させている
        //transform.transform.position =
        //    new Vector3(
        //    transform.localScale.x + Mathf.PingPong(Time.time / 2, UpDownSpeed),
        //    transform.localScale.y + Mathf.PingPong(Time.time / 2, UpDownSpeed),
        //    transform.localScale.z + Mathf.PingPong(Time.time / 2, UpDownSpeed));
        if (count <= 60)
        {
            gameObject.transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
            count += 1;
        }
        else if(count <= 120)
        {
            gameObject.transform.localScale -= new Vector3(0.001f, 0.001f, 0.001f);
            count += 1;
            if (count == 120)count = 0;
        }
    }
}
