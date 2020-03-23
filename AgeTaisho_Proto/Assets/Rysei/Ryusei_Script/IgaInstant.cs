using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgaInstant : MonoBehaviour {

    GameObject obj;

    // Use this for initialization
    void Start () {
        // CubeプレハブをGameObject型で取得
        obj = (GameObject)Resources.Load("Iga");

        // Cubeプレハブを元に、インスタンスを生成、
        Instantiate(obj, new Vector3(0.0f, 5.0f, 0.0f), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("if マウスのボタンを離した");
        }
    }

    void OnMouseDown()
    {
        //if(gameObject.tag == "iga")
        //{
            // Cubeプレハブを元に、インスタンスを生成、
            Instantiate(obj, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
        //}
        Debug.Log("マウスボタンを押した");
    }

    void OnMouseDrag()
    {
        // Cubeプレハブを元に、インスタンスを生成、
        //Instantiate(obj, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
        Debug.Log("マウスドラッグ中");
    }

    void OnMouseUp()
    {
        //if(gameObject.tag == "iga")
        //{
        // Cubeプレハブを元に、インスタンスを生成、
        //Instantiate(obj, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
        //}
        Debug.Log("マウスボタンを離した");
    }
}
