using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System;

public class BuildScript : MonoBehaviour
{

    [MenuItem("MyTools/ウィンドウズ用ビルド")]
    public static void BuildGame()
    {
        // ファイル名を取得
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        // 配列内が空欄の場合は現在のシーンをビルド
        string[] levels = new string[] { };

        var date = DateTime.Now;
        string timeStamp = date.ToString("yyyy-MM-dd-HH-mm-ss");

        // プレイヤーをビルド
        BuildPipeline.BuildPlayer(levels, path + "/sample_" + timeStamp + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }
}