using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Fried : MonoBehaviour
{
    Vector3 nowPosi;
    float UpDownSpeed = 0.2f;   // 上下の移動幅の値
    float RotationSpeed = 70f;// 回転させる速さ

    int count = 0;
    float a = 0.01f;

    bool start = false;
    bool moveflg = false;

    void Update()
    {
        if (start)
        {
            // その場で一度だけ上下運動させている
            if (!moveflg)
            {
                count += 1;
                if (count < 20) a -= 0.01f;
                else if (count < 40) a += 0.01f;
                else if (count == 60) moveflg = true;
                transform.position =
                    new Vector3(nowPosi.x, nowPosi.y + a, nowPosi.z);
            }
            //transform.position =
            //    new Vector3(transform.position.x, nowPosi + Mathf.PingPong(Time.time / 3, UpDownSpeed), transform.position.z);
            
            // その場で回転させている
            transform.Rotate(new Vector3(0, RotationSpeed, 0) * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "tenpuranabe" || other.gameObject.tag == "karaagenabe")
        {
            nowPosi = this.transform.position;
            nowPosi.y = this.transform.position.y-0.08f;
            start = true;
        }
    }

}
