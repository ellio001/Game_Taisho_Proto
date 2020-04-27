using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip_Botton : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("PS4_joystick_button_3"))   ReadScene();
    }
    public void ReadScene() {
        //Endシーン読込
        SceneManager.LoadScene("Cursor_Maseter_Scene", LoadSceneMode.Single);
    }
}
