using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverStaging : MonoBehaviour
{
    [SerializeField] GameObject[] Mobs;
    Rigidbody []rb;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        rb = new Rigidbody[2];
        rb[0] = Mobs[0].GetComponent<Rigidbody>();
        rb[1] = Mobs[1].GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.FiverFlag == true)
        {
            time += Time.deltaTime;

            if(time >= 0.5)
            {
                Debug.Log("timehatootta");
                rb[0].AddForce(transform.up * 1);
                rb[1].AddForce(transform.up * 1);
                time = 0;
            }

            if (Mobs[0].transform.position.x >= -1)
                Mobs[0].transform.position += new Vector3(-0.5f, 0, 0);

            if (Mobs[1].transform.position.x <= -8)
                Mobs[1].transform.position += new Vector3(0.5f, 0, 0);

        }else if (GameManager.instance.FiverFlag == false)
        {
            if (Mobs[0].transform.position.x <= 8)
                Mobs[0].transform.position += new Vector3(0.5f, 0, 0);
            
            if (Mobs[1].transform.position.x >= -17)
                Mobs[1].transform.position += new Vector3(-0.5f, 0, 0);
        }


    }
}
