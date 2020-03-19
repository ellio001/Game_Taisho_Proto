using UnityEngine;
using System.Collections;

public class GrappleObject : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    public Vector3 TapPos;

    public GameObject ItemSet;//

    void OnMouseDown()
    {
        // マウスカーソルは、スクリーン座標なので、
        // 対象のオブジェクトもスクリーン座標に変換してから計算する。

        // このオブジェクトの位置(transform.position)をスクリーン座標に変換。
        //screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        //// ワールド座標上の、マウスカーソルと、対象の位置の差分。
        //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        TapPos = GetComponent<Transform>().position;

        //TapPos = Input.mousePosition;
        //Vector3 TapPos = Camera.;
        //GetComponent<Rigidbody>().isKinematic = true;
        transform.position = Camera.main.ScreenToWorldPoint(TapPos);
    }

    void OnMouseDrag()
    {
        //Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        //Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
        //transform.position = currentPosition;

        transform.position = Camera.main.ScreenToWorldPoint(TapPos);
    }
}