using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_obj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        float width = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        print("width: " + width);

        // 高さ
        float height = gameObject.GetComponent<RectTransform>().sizeDelta.y;
        print("height: " + height);
    }
}
