using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour {

    //追加
    float speed = 0;
    
    //Start is called before the first frame update
    void Start() {    
    }

    //Update is called once per frame
    private void Update() {

        //追加
        if (Input.GetMouseButtonDown(0)) {
            this.speed = -100;
        }
        transform.Rotate(0, 0, this.speed);
        this.speed *= 0.96f;
    }

}
