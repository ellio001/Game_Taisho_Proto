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

    // Use this for initialization
    void Start()
    {
        TutorialTextList.Add("よく来たな！お前が今日から働く新人か！？ここでの働き方をお前に教えてやるからな！");
        TutorialTextList.Add("グズグズしている暇はないぞ！お客さんは待ってはくれないからな！...ほら、早速来たぞ!!");
        TutorialTextList.Add("お客さんを見てみろ！お客さんは○○の天ぷらを求めているだろ？");
        TutorialTextList.Add("天ぷらの作り方はまず食材を持つ！");
        TutorialTextList.Add("よし持てたな！そしたらそれを天ぷら専用の液につける！");
        TutorialTextList.Add("よし！じゃあそれをそのまま油の中へ\"ドォーン\"だ!!");
        TutorialTextList.Add("天ぷらの調理方法は全部同じ手順だからな!!");
        TutorialTextList.Add("よし！無事に揚げ終わったな！次はそれを皿に盛り付けだ!!");
        TutorialTextList.Add("揚がったものを皿の方に持っていけ");
        TutorialTextList.Add("よし！お前、器用だな!きれいに盛り付けできたぞ！");
        TutorialTextList.Add("じゃあそれをそのままお客さんに提供するんだ!!");
        TutorialTextList.Add("よし！お客さんが満足して帰っていったぞ上出来だ！これでてんぷらの作り方は完璧だな!!");

        StringCount = TutorialTextList.Count - 1;   //文字列がいくつあるか
        TutorialTextArea.text = TutorialTextList[TextNumber];   //最初のテキストを表示
    }

    // Update is called once per frame
    void Update()
    {
        TutorialTime += Time.deltaTime;

        //次のテキストがあれば5秒ごとにテキストを進める
        if (TutorialTime >= 5 && (StringCount > TextNumber))
        {
            TextNumber += 1;    //表示するテキストの番地を+1する
            TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
            TutorialTime = 0;
        }

        //次のテキストがあればzキーを押してテキストを進める
        if (Input.GetKeyDown(KeyCode.Z) && (StringCount > TextNumber))
        {
            TextNumber += 1;    //表示するテキストの番地を+1する
            TutorialTextArea.text = TutorialTextList[TextNumber];   //テキストを更新
        }
    }
}
