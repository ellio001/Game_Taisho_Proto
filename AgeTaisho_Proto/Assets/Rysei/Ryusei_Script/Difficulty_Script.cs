using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty_Script : MonoBehaviour
{
    [System.NonSerialized] public int Difficulty = 0; //難易度選択の変数

    [SerializeField]
    GameObject Choice_Image;    //Chois_Imageをいれる箱
    Transform Choice_Image_Transform;   //Chois_ImageのTransformをいれる箱
    Vector3 Choice_Image_Vector;    //Chois_Imageの

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Choice_Image_Transform = Choice_Image.transform;    //Choice_Image.transform(本体)の位置をChoice_Image_Transform(仮)にいれる
        Choice_Image_Vector = Choice_Image_Transform.position;  //仮にはいっている位置のVector3をChoice_Image_Vectorにいれる

        if ((Input.GetKeyDown(KeyCode.UpArrow) ||  0 < Input.GetAxisRaw("Cross_Vertical")) && Difficulty == 0) Difficulty = 2;
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || 0 < Input.GetAxisRaw("Cross_Vertical")) && Difficulty > 0) Difficulty -= 1;

        if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("Cross_Vertical")) && Difficulty == 2) Difficulty = 0;
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || 0 > Input.GetAxisRaw("Cross_Vertical")) && Difficulty < 2) Difficulty += 1;

        Choice_Image_Vector.y = -100* (Difficulty + 1) + 145;   //仮のほうの座標を動かす
        Choice_Image_Transform.position = Choice_Image_Vector;  //仮の座標を本決定

        if (Input.GetButtonUp("〇"))
        {
            switch (Difficulty)
            {
                case 0:
                    SceneManager.LoadScene("Easy_Tutorial_Scene");
                    break;
                case 1:
                    SceneManager.LoadScene("Normal_Tutorial_Scene");
                    break;
                case 2:
                    SceneManager.LoadScene("Hard_Tutorial_Scene");
                    break;
            }
        }
    }
}
