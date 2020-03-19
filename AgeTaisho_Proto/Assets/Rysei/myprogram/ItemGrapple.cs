using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemGrapple : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    private bool kona = false;

    public GameObject objGet;  //GameObjectを格納する箱

    Animator animator;  //アニメーターを格納する箱
    int nowAnim = 0;

    void Start()
    {
        animator = GetComponent<Animator>();    //アニメーターを格納する
    }

    void Update()
    {
        
    }

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
        if (other.gameObject.tag == "kona" && kona == false)
        {
            GetComponent<Renderer>().material.color = Color.white;
            kona = true;
        }
        if (other.gameObject.tag == "kona" && kona == true)
        {
            // GetComponent<Renderer>().material.color = Color.white;
            //kona = true;
            //animator.Play();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Sphere")
        {
            objGet = GameObject.Find("Sphere");

            animator.SetBool("Change", true);

            Destroy(other.gameObject);
        }
    }
}
