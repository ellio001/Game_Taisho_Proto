using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMouse : MonoBehaviour {

    public float speed = 0.1f; //カメラアングルの移動速度

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //マウスカーソルの位置を取得
        Vector3 mousePos = Input.mousePosition;

        //中央の位置に合わせる(マウス座標の開始位置は画面左下なので調整が必要)
        mousePos.x = mousePos.x - (Screen.width / 2);
        mousePos.y = mousePos.y - (Screen.height / 2);

        //カメラのアングルを取得
        Vector3 cameraAngle = transform.eulerAngles;

        cameraAngle.x = -mousePos.y * speed; //マウスを横に動かしたとき
        cameraAngle.y = mousePos.x * speed; //マウスを縦に動かしたとき
        cameraAngle.z = 0; //ｚは変化しないので0にする

        //動かした変化をカメラのアングルに適用させる
        transform.eulerAngles = cameraAngle;
    }
}
