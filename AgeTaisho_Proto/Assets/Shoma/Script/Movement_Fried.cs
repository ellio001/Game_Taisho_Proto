using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Fried : MonoBehaviour
{
    Vector3 nowPosi; // スクリプトがついているObjの座標を記憶させる
    float UpDownSpeed = 0.2f;   // 上下の移動幅の値
    float RotationSpeed = 70f;// 回転させる速さ

    int count = 0;
    float a = 0.01f; // 上下移動の計算の代入用
    bool moveflg = false; // false =上下移動中 ・ true =上下移動終了中

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "tenpuranabe" || other.gameObject.tag == "karaagenabe")
        {
            nowPosi = this.transform.position;
            nowPosi.y = this.transform.position.y - 0.08f;
            moveflg = false;
            count = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "tenpuranabe" || other.gameObject.tag == "karaagenabe")
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
            // その場で回転させている
            transform.Rotate(new Vector3(0, RotationSpeed, 0) * Time.deltaTime, Space.World);


        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}