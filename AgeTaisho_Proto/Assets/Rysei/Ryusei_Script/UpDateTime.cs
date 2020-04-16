using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpDateTime : MonoBehaviour
{
    public GameObject ver_object;

    // Start is called before the first frame update
    void Start()
    {
        string filepath1 = @"D:\Unity_Project\GameTaisho_Age\Finishi_Proto\AgeTaisho_Proto.exe";

        System.DateTime dt = System.IO.File.GetLastWriteTime(filepath1);  //最終更新日
        //System.DateTime dt = System.IO.File.GetCreationTime(filepath1);   //作った日
        //System.DateTime dt = System.IO.File.GetLastAccessTime(filepath1);   //最終アクセス日
        Debug.Log(dt.ToString("yyyy/MM/dd hh:mm:ss"));

        Text ver_text = ver_object.GetComponent<Text>();
        ver_text.text = dt.ToString("yyyy/MM/dd hh:mm:ss");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
