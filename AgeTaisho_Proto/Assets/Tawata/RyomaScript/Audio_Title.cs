using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Title : MonoBehaviour {
    AudioSource[] sounds;

    // Use this for initialization
    void Start() {
        sounds = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        //キーボードの1か2を押したら音楽再生
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            sounds[0].Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            sounds[1].Play();
        }
    }


}