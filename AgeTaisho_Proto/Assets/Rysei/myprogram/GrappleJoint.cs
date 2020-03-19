using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrappleJoint : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public GameObject objGet;  //GameObjectを格納する箱

    void OnMouseDown()
    {
        // マウスカーソルは、スクリーン座標なので、
        // 対象のオブジェクトもスクリーン座標に変換してから計算する。

        //クリックしたGameObjectを格納
        objGet = GameObject.Find(gameObject.name);
        //objGet = GameObject.Find("Sphere");

        // 格納GameObjectの位置(transform.position)をスクリーン座標に変換。
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        // ワールド座標上の、マウスカーソルと、格納したGameObjectの位置の差分。
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
        transform.position = currentPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "abura")
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        if (other.gameObject.tag == "kona")
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Sphere")
        {
            
            //otherをトリガーにする(反発しないように)
            other.gameObject.GetComponent<Collider>().isTrigger = true;

            //othergameObjectを触れたオブジェクトと同じ座標に移動させる
            other.gameObject.transform.position = transform.position;

            //otherobjectを持っているオブジェクトに結合する
            gameObject.GetComponent<Joint>().connectedBody = other.gameObject.GetComponent<Rigidbody>();

            //重力を受けなくする
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;

            //othergameObjectを触れたオブジェクトの子にする
            //other.gameObject.transform.parent = gameObject.transform;

            //Destroy(other.gameObject);
        }
    }
}
