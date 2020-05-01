using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Fried : MonoBehaviour
{
    float nowPosi;
    float UpDownSpeed = 0.2f;   // 上下の移動幅の値
    float RotationSpeed = 70f;// 回転させる速さ
    bool start = false;

    void Update()
    {
        if (start)
        {
            // その場で上下運動させている
            transform.position = 
                new Vector3(transform.position.x, nowPosi + Mathf.PingPong(Time.time / 3, UpDownSpeed), transform.position.z);
            // その場で回転させている
            transform.Rotate(new Vector3(0, RotationSpeed, 0) * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "tenpuranabe" || other.gameObject.tag == "karaagenabe")
        {
            nowPosi = this.transform.position.y-0.2f;
            start = true;
        }
    }

}
