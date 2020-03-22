using UnityEngine;
using System.Collections;

public class SceneManager_S : MonoBehaviour {
    //public string cubeTag = "Cube";
    //public string ebiTag = "ebi";
    //public string tenTag = "tenpura";
    //public string karaTag;
    //public int objTag = 0;
    //private bool beRay = false;
    //private Vector3 moveTo;

    public GameObject clickedGameObject;
    public GameObject Resource;
    GameObject ClickObj;
    float handspeed = 0.1f;

    public Transform ClickObj2;

    GameObject ebiPrefab;

    private void Start()
    {
        // CubeプレハブをGameObject型で取得
        ebiPrefab = (GameObject)Resources.Load("ebi");

        ClickObj = GameObject.Find("ControllerObjClick");


        // Cubeプレハブを元に、インスタンスを生成、
        //Instantiate(ebiPrefab, new Vector3(0.0f, 5.0f, 0.0f), Quaternion.identity);
    }

    // Update is called once per frame  
    void Update() {
        
        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Rayを飛ばし、オブジェクトがあればtrue 
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 5f)){
            

        }
        //if (Input.GetButtonUp("〇") || Input.GetKeyUp(KeyCode.Space))
        //{
        //    clickedGameObject = null;   //対象を入れる箱を初期化
        //    Resource = null;            //生成するプレハブの箱を初期化
        //}
    }


}



