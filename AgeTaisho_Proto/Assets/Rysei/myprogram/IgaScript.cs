using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgaScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.position.y >= 30 || gameObject.transform.position.y <= -2)
        {
            Destroy(gameObject);
        }
    }
}
