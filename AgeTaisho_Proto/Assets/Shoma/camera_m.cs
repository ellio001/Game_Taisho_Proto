using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_m : MonoBehaviour
{
	float minAngle = 270.0F;
    float maxAngle = 100.0F;

    void Start()
    {
        
    }

    void Update()
    {
        float angle = Mathf.LerpAngle(minAngle, maxAngle, Time.time/9);
        transform.eulerAngles = new Vector3(0, angle, 0);

    }
}
