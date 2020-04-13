using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("〇")) Debug.Log("〇ボタン");
        if (0 < Input.GetAxisRaw("Cross_Horizontal")) Debug.Log("十字キー右");
        else if (0 > Input.GetAxisRaw("Cross_Horizontal")) Debug.Log("十字キー左");
        if (0 < Input.GetAxisRaw("Cross_Vertical")) Debug.Log("十字キー上");
        else if (0 > Input.GetAxisRaw("Cross_Vertical")) Debug.Log("十字キー下");
    }
}
