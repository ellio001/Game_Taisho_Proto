using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_arrow : MonoBehaviour
{
    [System.NonSerialized] public float nowPosi;
    float UpDownSpeed = 0.3f;   // 上下の移動幅の値
    float RotationSpeed = 90f;// 回転させる速さ

    void Start()
    {
        nowPosi = this.transform.position.y;
    }

    void Update()
    {
        // その場で上下運動させている
        transform.position = new Vector3(transform.position.x, 
            nowPosi + Mathf.PingPong(Time.time/2, UpDownSpeed) /*+ 0.9f*/, transform.position.z);
        // その場で回転させている
        transform.Rotate(new Vector3(0, RotationSpeed, 0) * Time.deltaTime, Space.World);
    }
}
