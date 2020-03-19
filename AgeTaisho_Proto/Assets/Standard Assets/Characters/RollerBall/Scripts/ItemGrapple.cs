using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemGrapple : MonoBehaviour
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
        screenPoint = Camera.main.WorldToScreenPoint(objGet.transform.position);
        // ワールド座標上の、マウスカーソルと、格納したGameObjectの位置の差分。
        offset = objGet.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Sphere")
        {
            objGet = GameObject.Find("Sphere");


        }
    }
}
