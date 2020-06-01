using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    private Text TutorialTextArea;  //Text型のTutorialTextをいれる
    [System.NonSerialized] public List<string> TutorialTextList = new List<string>(); //テキストのリスト

    [System.NonSerialized]  public int TextNumber = 0; //TextNumber番地のテキストをみる
    private int StringCount;    //TextNumberがいくつまであるか
    private float TutorialTime; //Time.deltatimeをいれる

    string SceneName; // sceneの名前を記憶する変数

    /* 師匠のアイコンをセリフごとで切り替える */
    [SerializeField] public GameObject Sisho;      // 師匠のアイコン入れる
    [SerializeField] public GameObject Sisho_Hasi; // 箸を持ってるアイコンを入れる

    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name; // scene名を記憶
        
        switch (SceneName)// 各難易度の師匠のセリフをリストに追加する
        {
            case "Easy_Tutorial_Scene":
                TutorialTextList.Add/*0*/("よく来たな！\nお前が今日から働く新人か！？\nここでの働き方をお前に教えてやるからな！");
                TutorialTextList.Add/*1*/("グズグズしている暇はないぞ！\nお客さんは待ってはくれないからな！\n...ほら、早速来たぞ!!");
                TutorialTextList.Add/*2*/("お客さんを見てみろ！\nお客さんはエビの天ぷらを求めているだろ？");
                TutorialTextList.Add/*3*/("天ぷらの作り方はまず食材を持つ！");
                TutorialTextList.Add/*4*/("よし持てたな！\nそしたらそれを天ぷら専用の液につける！");
                TutorialTextList.Add/*5*/("よし！じゃあそれをそのまま油の中へ\n\"ドォーン\"だ!!");
                TutorialTextList.Add/*6*/("天ぷらの調理方法は全部同じ手順だからな!!");
                TutorialTextList.Add/*7*/("いいぞ！無事に揚げ終わったな！\n次はそれを皿に盛り付けだ!!\n" +
                                          "揚がったものを皿の方に持っていけ");
                TutorialTextList.Add/*8*/("よし！お前、器用だな!\nきれいに盛り付けできたぞ！\n" +
                                          "じゃあそれをそのままお客さんに提供するんだ!!");
                TutorialTextList.Add/*9*/("よし！\nお客さんが満足して帰っていったぞ上出来だ！\nこれでてんぷらの作り方は完璧だな!!");
                break;
            case "Normal_Tutorial_Scene":
                TutorialTextList.Add/*0*/("NormalTutorialに入りました。");
                TutorialTextList.Add/*1*/("NormalSceneに移動します。");
                break;
            case "Hard_Tutorial_Scene":
                TutorialTextList.Add/*0*/("HardTutorialに入りました。");
                TutorialTextList.Add/*1*/("HardSceneに移動します。");
                break;
        }

        StringCount = TutorialTextList.Count - 1;   //文字列がいくつあるか
        TutorialTextArea.text = TutorialTextList[TextNumber];   //最初のテキストを表示

        Sisho.gameObject.SetActive(true); // 普通の師匠を表示
        Sisho_Hasi.gameObject.SetActive(false); // 箸持ち師匠を非表示
    }


    void Update()
    {
        TutorialTime += Time.deltaTime;
        switch (SceneName)
        {
            case "Easy_Tutorial_Scene":
                EasyTutorial();
                break;
            case "Normal_Tutorial_Scene":
                NormalTutorial();
                break;
            case "Hard_Tutorial_Scene":
                HardTutorial();
                break;
        }

        //次のテキストがあればzキーを押してテキストを進める
        if (Input.GetKeyDown(KeyCode.Z) && (StringCount > TextNumber))
        {
            TextNumber += 1;    //表示するテキストの番地を+1する
            TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
        }
    }

 /*----------------------------------------------------------------------------------------------------------------------------------------------------------------*/
    void EasyTutorial()
    {
        switch (TextNumber)
        {
            case 0:
            case 1:
            case 2:
                if (TutorialTime >= 5)
                {
                    TextNumber += 1;    //表示するテキストの番地を+1する
                    TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
                    TutorialTime = 0;
                }
                break;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
                TutorialTime = 0;
                break;
            case 9:
                TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
                if (TutorialTime >= 5)
                {
                    TutorialTime = 0;
                    ReadScene();
                }
                break;
            default:
                break;
        }

        switch (TextNumber)
        {
            case 3: // 箸持ち師匠を表示
                Sisho.gameObject.SetActive(false);
                Sisho_Hasi.gameObject.SetActive(true); 
                break;
            case 6: // 普通の師匠を表示
                Sisho.gameObject.SetActive(true);
                Sisho_Hasi.gameObject.SetActive(false);
                break;
            case 7:// 箸持ち
                Sisho.gameObject.SetActive(false);
                Sisho_Hasi.gameObject.SetActive(true);
                break;
            case 8:// 普通
                Sisho.gameObject.SetActive(true);
                Sisho_Hasi.gameObject.SetActive(false);
                break;
        }       
    }

    /*------------------------------------------*/

    void NormalTutorial()
    {
        switch (TextNumber)
        {
            case 0:
                if (TutorialTime >= 5)
                {
                    TextNumber += 1;    //表示するテキストの番地を+1する
                    TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
                    TutorialTime = 0;
                }
                break;
            case 1:
                TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
                if (TutorialTime >= 5)
                {
                    TutorialTime = 0;
                    ReadScene();
                }
                break;
            default:
                break;
        }

    }

    /*------------------------------------------*/

    void HardTutorial()
    {
        switch (TextNumber)
        {
            case 0:
                if (TutorialTime >= 5)
                {
                    TextNumber += 1;    //表示するテキストの番地を+1する
                    TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
                    TutorialTime = 0;
                }
                break;
            case 1:
                TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
                if (TutorialTime >= 5)
                {
                    TutorialTime = 0;
                    ReadScene();
                }
                break;
            default:
                break;
        }
    }
 /*----------------------------------------------------------------------------------------------------------------------------------------------------------------*/

    public void ReadScene() {
        //Endシーン読込
        switch (SceneName)
        {
            case "Easy_Tutorial_Scene":
                SceneManager.LoadScene("Recommendation_Scene");
                break;
            case "Normal_Tutorial_Scene":
                SceneManager.LoadScene("Normal_Scene");
                break;
            case "Hard_Tutorial_Scene":
                SceneManager.LoadScene("Hard_Scene");
                break;
        }
    }
}
