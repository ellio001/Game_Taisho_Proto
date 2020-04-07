using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    private Text TutorialTextArea;  //Text型のTutorialTextをいれる
    //[SerializeField]
    public List<string> TutorialTextList = new List<string>(); //テキストのリスト

    public int TextNumber = 0; //TextNumber番地のテキストをみる
    private int StringCount;    //TextNumberがいくつまであるか
    private float TutorialTime; //Time.deltatimeをいれる

    void Start()
    {
                            //↓リスト番号
        TutorialTextList.Add/*0*/("よく来たな！お前が今日から働く新人か！？ここでの働き方をお前に教えてやるからな！");
        TutorialTextList.Add/*1*/("グズグズしている暇はないぞ！お客さんは待ってはくれないからな！...ほら、早速来たぞ!!");
        TutorialTextList.Add/*2*/("お客さんを見てみろ！お客さんは○○の天ぷらを求めているだろ？");
        TutorialTextList.Add/*3*/("天ぷらの作り方はまず食材を持つ！");
        TutorialTextList.Add/*4*/("よし持てたな！そしたらそれを天ぷら専用の液につける！");
        TutorialTextList.Add/*5*/("よし！じゃあそれをそのまま油の中へ\"ドォーン\"だ!!");
        TutorialTextList.Add/*6*/("天ぷらの調理方法は全部同じ手順だからな!!");
        TutorialTextList.Add/*7*/("よし！無事に揚げ終わったな！次はそれを皿に盛り付けだ!!\n" +
                                  "揚がったものを皿の方に持っていけ");
        //TutorialTextList.Add/*8*/("揚がったものを皿の方に持っていけ");
        TutorialTextList.Add/*8*/("よし！お前、器用だな!きれいに盛り付けできたぞ！\n" +
                                  "じゃあそれをそのままお客さんに提供するんだ!!");
        //TutorialTextList.Add/*9*/("じゃあそれをそのままお客さんに提供するんだ!!");
        TutorialTextList.Add/*9*/("よし！お客さんが満足して帰っていったぞ上出来だ！これでてんぷらの作り方は完璧だな!!");

        StringCount = TutorialTextList.Count - 1;   //文字列がいくつあるか
        TutorialTextArea.text = TutorialTextList[TextNumber];   //最初のテキストを表示
    }

    void Update()
    {
        TutorialTime += Time.deltaTime;

        //次のテキストがあれば5秒ごとにテキストを進める
        //if (TutorialTime >= 5 || ((3 > TextNumber)|| 4 <= TextNumber))
        //{
        //    TextNumber += 1;    //表示するテキストの番地を+1する
        //    TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
        //    TutorialTime = 0;
        //}

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
            case 9:
                TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
                TutorialTime = 0;
                break;
        }

        //次のテキストがあればzキーを押してテキストを進める
        if (Input.GetKeyDown(KeyCode.Z) && (StringCount > TextNumber))
        {
            TextNumber += 1;    //表示するテキストの番地を+1する
            TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
        }
    }
}
