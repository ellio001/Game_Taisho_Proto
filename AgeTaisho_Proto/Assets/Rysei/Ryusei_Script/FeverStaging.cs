using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FeverStaging : MonoBehaviour
{
    [SerializeField] GameObject[] Mobs;
    Rigidbody []rb;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        rb = new Rigidbody[5];
        rb[0] = Mobs[0].GetComponent<Rigidbody>();
        rb[1] = Mobs[1].GetComponent<Rigidbody>();
        rb[2] = Mobs[2].GetComponent<Rigidbody>();
        rb[3] = Mobs[3].GetComponent<Rigidbody>();
        rb[4] = Mobs[4].GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (SceneManager.GetActiveScene().name)
        {
            case "Ryusei_Scene":
            case "Easy_Scene":

                if (GameManager.instance.FiverFlag == true)
                {
                    time += Time.deltaTime;

                    if (time >= 0.5)
                    {
                        rb[0].AddForce(transform.up * 1);
                        rb[1].AddForce(transform.up * 1);
                        rb[2].AddForce(transform.up * 1);
                        rb[3].AddForce(transform.up * 1);
                        rb[4].AddForce(transform.up * 1);
                        time = 0;
                    }

                    if (Mobs[0].transform.position.x >= -0.5)
                        Mobs[0].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[1].transform.position.x <= -6)
                        Mobs[1].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[2].transform.position.x >= 1)
                        Mobs[2].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[3].transform.position.x <= -3.5)
                        Mobs[3].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[4].transform.position.x <= -8)
                        Mobs[4].transform.position += new Vector3(0.5f, 0, 0);

                }
                else if (GameManager.instance.FiverFlag == false)
                {
                    if (Mobs[0].transform.position.x <= 9)
                        Mobs[0].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[1].transform.position.x >= -17)
                        Mobs[1].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[2].transform.position.x <= 5.5)
                        Mobs[2].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[3].transform.position.x >= -14)
                        Mobs[3].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[4].transform.position.x >= -20)
                        Mobs[4].transform.position += new Vector3(-0.5f, 0, 0);
                }
                break;
            case "Normal_Scene":

                if (GameManager.instance.FiverFlag == true)
                {
                    time += Time.deltaTime;

                    if (time >= 0.5)
                    {
                        rb[0].AddForce(transform.up * 1);
                        rb[1].AddForce(transform.up * 1);
                        rb[2].AddForce(transform.up * 1);
                        rb[3].AddForce(transform.up * 1);
                        rb[4].AddForce(transform.up * 1);
                        time = 0;
                    }

                    if (Mobs[0].transform.position.x >= 2.5)
                        Mobs[0].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[1].transform.position.x <= -6)
                        Mobs[1].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[2].transform.position.x >= 5)
                        Mobs[2].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[3].transform.position.x <= -12)
                        Mobs[3].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[4].transform.position.x <= -9)
                        Mobs[4].transform.position += new Vector3(0.5f, 0, 0);

                }
                else if (GameManager.instance.FiverFlag == false)
                {
                    if (Mobs[0].transform.position.x <= 15)
                        Mobs[0].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[1].transform.position.x >= -17)
                        Mobs[1].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[2].transform.position.x <= 10)
                        Mobs[2].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[3].transform.position.x >= -25)
                        Mobs[3].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[4].transform.position.x >= -20)
                        Mobs[4].transform.position += new Vector3(-0.5f, 0, 0);
                }
                break;
            case "Hard_Scene":

                if (GameManager.instance.FiverFlag == true)
                {
                    time += Time.deltaTime;

                    if (time >= 0.5)
                    {
                        rb[0].AddForce(transform.up * 1);
                        rb[1].AddForce(transform.up * 1);
                        rb[2].AddForce(transform.up * 1);
                        rb[3].AddForce(transform.up * 1);
                        rb[4].AddForce(transform.up * 1);
                        time = 0;
                    }

                    if (Mobs[0].transform.position.x >= 2.5)
                        Mobs[0].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[1].transform.position.x <= -2.5)
                        Mobs[1].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[2].transform.position.x >= 5)
                        Mobs[2].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[3].transform.position.x <= -9)
                        Mobs[3].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[4].transform.position.x <= -6)
                        Mobs[4].transform.position += new Vector3(0.5f, 0, 0);

                }
                else if (GameManager.instance.FiverFlag == false)
                {
                    if (Mobs[0].transform.position.x <= 15)
                        Mobs[0].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[1].transform.position.x >= -17)
                        Mobs[1].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[2].transform.position.x <= 10)
                        Mobs[2].transform.position += new Vector3(0.5f, 0, 0);

                    if (Mobs[3].transform.position.x >= -25)
                        Mobs[3].transform.position += new Vector3(-0.5f, 0, 0);

                    if (Mobs[4].transform.position.x >= -20)
                        Mobs[4].transform.position += new Vector3(-0.5f, 0, 0);
                }
                break;
        }
    }
}
