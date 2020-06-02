using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_plate_put : MonoBehaviour {

    //音を制御するフラグ
    bool flag;
    //音
    public AudioClip Audio_Dust;
    private AudioSource AudioSource;

    // Start is called before the first frame update
    void Start() {
        flag = false;
        AudioSource = gameObject.GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other) {
        if (!flag) {
            flag = true;
            AudioSource.PlayOneShot(Audio_Dust);
        }
        if (flag) {
            flag = false;
        }
    }
}
