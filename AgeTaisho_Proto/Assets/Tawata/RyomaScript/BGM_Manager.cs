using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


//タイトルから
public class BGM_Manager : MonoBehaviour {
    
    public static BGM_Manager instance = null;

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
        //if (DontDestroyEnabled) {
        //    // Sceneを遷移してもオブジェクトが消えないようにする
        //    DontDestroyOnLoad(this);
        //}
        sounds = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        //Title_Scene＆Difficulty_Scene
        if (SceneManager.GetActiveScene().name == "Difficulty_Scene" ||
            SceneManager.GetActiveScene().name == "Title_Scene") {
            if (!sounds[0].isPlaying) {
                //BGM初期化
                BGM_Stop();
                //BGM_Titoleスタート
                sounds[0].Play();
            }
        }
        //Easy_Scene
        else if (SceneManager.GetActiveScene().name == "Easy_Scene") {
            if (!sounds[1].isPlaying) {
                //BGM初期化
                BGM_Stop();
                //BGM_Easyスタート
                sounds[1].Play();
            }
        }
        //Normal_Scene
        else if (SceneManager.GetActiveScene().name == "Normal_Scene") {
            if (!sounds[2].isPlaying) {
                //BGM初期化
                BGM_Stop();
                //BGM_Normalスタート
                sounds[2].Play();
            }
        }
        //Hard_Scene
        else if (SceneManager.GetActiveScene().name == "Hard_Scene") {
            if (!sounds[3].isPlaying) {
                //BGM初期化
                BGM_Stop();
                //Hardスタート
                sounds[3].Play();
            }
        }
        else {
            //BGM初期化
            BGM_Stop();
        }
    }

    //サウンドの初期化
    void BGM_Stop() {
        for(int i = 0; i < 4; i++) {
            sounds[i].Stop();
        }
    }
}