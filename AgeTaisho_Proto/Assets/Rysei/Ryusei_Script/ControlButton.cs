using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButton : MonoBehaviour {

    // GameObject cam;

    //[SerializeField] GameObject camera;

    // Use this for initialization

    float x = 2.0f;
    float y = 2.0f;

    void Start () {
        //cam = GameObject.Find("SubCamera");
    }

    // Update is called once per frame
    void Update()
    {

        //左スティック
        if (Input.GetAxisRaw("L_Vertical") < 0)
        {
            //Debug.Log("上に傾いている");
            transform.Rotate(new Vector3(-2.0f, 0, 0));

            //cam.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
            //x -= Time.deltaTime * 100;
            //transform.rotation = Quaternion.Euler(y, 0, 0);

        }
        else if (0 < Input.GetAxisRaw("L_Vertical")){

            //Debug.Log("下に傾いている");
            transform.Rotate(new Vector3(2.0f, 0, 0));

            //x += Time.deltaTime * 100;
            //transform.rotation = Quaternion.Euler(y, 0, 0);
        }
        else{
            //上下方向には傾いていない
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(new Vector3(-2.0f, 0, 0));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(new Vector3(2.0f, 0, 0));
        }

        //transform.rotation = Quaternion.Euler(x, y, 0);

        //if (Input.GetMouseButton(0))
        //{
        //    //Debug.Log("下に傾いている");
        //    //transform.Rotate(new Vector3(2, 2, 0));
        //    //transform.Rotate(2.0f, 2.0f, 0, Space.World);
        //    //transform.Rotate(new Quaternion.Euler(2, 0, 0));

        //    y += Time.deltaTime * 10;
        //    transform.rotation = Quaternion.Euler(30.0f, y, 0);

        //    //transform.Rotate(new Vector3(0, 2, 0));

        //    //}
        //    //if (transform.rotation.x > 0.6)
        //    //{
        //    //    Debug.Log("90を超えた");
        //    //}
        //}else if (Input.GetButton("〇"))
        //{
        //    y += Time.deltaTime * 10;
        //    transform.rotation = Quaternion.Euler(30.0f, -y, 0);
        //}
        //else
        //{
        //    y = Time.deltaTime * 0;
        //}
    }
}
