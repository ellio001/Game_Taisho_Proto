using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    [SerializeField]
    GameObject Update_object; // Textオブジェクトをいれる箱
    // Use this for initialization
    void Start()
    {
        // ファイル名を取得
        string path1 = Application.dataPath;
        System.DateTime dt = System.IO.File.GetLastWriteTime(path1);
        // オブジェクトからTextコンポーネントを取得
        Text Update_text = Update_object.GetComponent<Text>();
        // テキストの表示を入れ替える
        Update_text.text = dt.ToString("yyyy/MM/dd hh:mm");

        //string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("〇"))
        {
            SceneManager.LoadScene("Difficulty_Scene");
        }
    }
}
