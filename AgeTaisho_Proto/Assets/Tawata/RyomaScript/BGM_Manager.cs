using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


//タイトルから
public class BGM_Manager : MonoBehaviour {

    public static BGM_Manager instance = null;

    int SceneFlag;
    bool Stopflag;
    bool Peakflag;
    bool SceneLoadflag;

    GameObject obj_GM;
    GameManager script;

    AudioSource[] sounds;

    private void Awake()    //スタートよりも最初に呼ばれる
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        //初期化
        Stopflag = false;
        Peakflag = false;
        SceneLoadflag = false;
        SceneFlag = 0;

        //BGmの情報とる
        sounds = GetComponents<AudioSource>();

        //先読込
        Scene_switch();
    }

    //何のシーンを読んでいるか判定
    void Scene_switch() {
        //Title_Scene＆Difficulty_Scene
        if (SceneManager.GetActiveScene().name == "Difficulty_Scene" ||
            SceneManager.GetActiveScene().name == "Title_Scene") {
            SceneFlag = 0;
        }
        //Easy_Scene
        else if (SceneManager.GetActiveScene().name == "Easy_Scene") {
            SceneFlag = 1;
            if (script == null) {
                //ゲームマネージャーの情報をもらう
                obj_GM = GameObject.Find("GameManager");
                script = obj_GM.GetComponent<GameManager>();
            }
        }
        //Normal_Scene
        else if (SceneManager.GetActiveScene().name == "Normal_Scene") {
            SceneFlag = 2;
            if (script == null) {
                //ゲームマネージャーの情報をもらう
                obj_GM = GameObject.Find("GameManager");
                script = obj_GM.GetComponent<GameManager>();
            }
        }
        //Hard_Scene
        else if (SceneManager.GetActiveScene().name == "Hard_Scene") {
            SceneFlag = 3;
            if (script == null) {
                //ゲームマネージャーの情報をもらう
                obj_GM = GameObject.Find("GameManager");
                script = obj_GM.GetComponent<GameManager>();
            }
        }
        else {
            SceneFlag = 10;
            //BGM初期化
            BGM_Stop();
        }
    }

    //シーンをロードしたら呼ばれる
    void OnSceneLoaded(Scene nextScene, LoadSceneMode mode) {
        Scene_switch();
        Stopflag = false;
    }

    // Update is called once per frame
    void Update() {

        SceneManager.sceneLoaded += OnSceneLoaded;
        switch (SceneFlag) {

            //Title_Scene＆Difficulty_Scene
            case 0:
                if (!sounds[0].isPlaying) {
                    //BGM初期化
                    BGM_Stop();
                    //BGM_Titoleスタート
                    sounds[0].Play();
                }
                break;

            //Easy_Scene
            case 1:
                if (!script.FiverFlag) {
                    if (!sounds[1].isPlaying) {
                        if (!Stopflag) {
                            //BGM初期化
                            BGM_Stop();
                        }
                        //BGM_Easyスタート
                        sounds[1].Play();
                    }
                    //BGM_Titoleスタート
                    sounds[4].Stop();
                }
                else if (script.FiverFlag) {
                    //BGM_Easyスタート
                    sounds[1].Stop();
                    //フィーバーかどうか判定
                    BGM_Peak_Judg();
                }
                break;

            //Normal_Scene
            case 2:
                if (!script.FiverFlag) {
                    if (!sounds[2].isPlaying) {
                        if (!Stopflag) {
                            //BGM初期化
                            BGM_Stop();
                        }
                        //BGM_Normalスタート
                        sounds[2].Play();
                    }
                    //BGM_Titoleスタート
                    sounds[4].Stop();
                }
                else if (script.FiverFlag) {
                    //BGM_Easyスタート
                    sounds[2].Stop();
                    //フィーバーかどうか判定
                    BGM_Peak_Judg();
                }
                //フィーバーかどうか判定
                BGM_Peak_Judg();
                break;

            //Hard_Scene
            case 3:
                if (!script.FiverFlag) {
                    if (!sounds[3].isPlaying) {
                        if (!Stopflag) {
                            //BGM初期化
                            BGM_Stop();
                        }
                        //Hardスタート
                        sounds[3].Play();
                    }
                    //BGM_Titoleスタート
                    sounds[4].Stop();
                }
                else if (script.FiverFlag) {
                    //BGM_Easyスタート
                    sounds[3].Stop();
                    //フィーバーかどうか判定
                    BGM_Peak_Judg();
                }
                //フィーバーかどうか判定
                BGM_Peak_Judg();
                break;

        }
    }

    //サウンドの初期化
    void BGM_Stop() {
        for (int i = 0; i < 4; i++) {
            sounds[i].Stop();
        }
        Peakflag = false;
        Stopflag = true;
    }

    //ピーク時かどうか判定
    void BGM_Peak_Judg() {

        //二度読み防止
        if (!Peakflag) {
            //フィーバータイム
            if (script.FiverFlag) {
                //ピーク再生
                BGM_Peak();
                Peakflag = true;
            }
        }

        //二度読み防止
        else if (Peakflag) {
            //フィーバータイムじゃないとき
            if (!script.FiverFlag) {
                //止めているBGMを再生
                switch (SceneFlag) {
                    case 1:
                        sounds[1].UnPause();
                        break;
                    case 2:
                        sounds[2].UnPause();
                        break;
                    case 3:
                        sounds[3].UnPause();
                        break;
                }
                Peakflag = false;
            }
        }
    }

    //ピーク時のBGM再生
    void BGM_Peak() {
        //流れているBGMを止める
        switch (SceneFlag) {
            case 1:
                print("Easy");
                sounds[1].Stop();
                if (!sounds[4].isPlaying) {
                    //Hardスタート
                    sounds[4].Play();
                }
                break;
            case 2:
                print("Nomal");
                sounds[2].Stop();
                if (!sounds[4].isPlaying) {
                    //Hardスタート
                    sounds[4].Play();
                }
                break;
            case 3:
                print("Hard");
                sounds[3].Stop();
                if (!sounds[4].isPlaying) {
                    //Hardスタート
                    sounds[4].Play();
                }
                break;
        }
    }


}

////Title_Scene＆Difficulty_Scene
//if (SceneManager.GetActiveScene().name == "Difficulty_Scene" ||
//    SceneManager.GetActiveScene().name == "Title_Scene") {
//    if (!sounds[0].isPlaying) {
//        //BGM初期化
//        BGM_Stop();
//        //BGM_Titoleスタート
//        sounds[0].Play();
//    }
//}
////Easy_Scene
//else if (SceneManager.GetActiveScene().name == "Easy_Scene") {
//    if (!sounds[1].isPlaying) {
//        //BGM初期化
//        BGM_Stop();
//        //BGM_Easyスタート
//        sounds[1].Play();
//    }
//    //フィーバーかどうか判定
//    BGM_Peak_Judg();
//}
////Normal_Scene
//else if (SceneManager.GetActiveScene().name == "Normal_Scene") {
//    if (!sounds[2].isPlaying) {
//        //BGM初期化
//        BGM_Stop();
//        //BGM_Normalスタート
//        sounds[2].Play();
//    }
//    //フィーバーかどうか判定
//    BGM_Peak_Judg();
//}
////Hard_Scene
//else if (SceneManager.GetActiveScene().name == "Hard_Scene") {
//    if (!sounds[3].isPlaying) {
//        //BGM初期化
//        BGM_Stop();
//        //Hardスタート
//        sounds[3].Play();
//    }
//    //フィーバーかどうか判定
//    BGM_Peak_Judg();
//}
//else {
//    //BGM初期化
//    BGM_Stop();
//}