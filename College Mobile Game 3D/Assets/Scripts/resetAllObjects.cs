using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetAllObjects : MonoBehaviour
{
    public GameObject camera;
    public GameObject firstCube;
    public GameObject secondCube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onButtonClickToReset()
    {
        camera.transform.position = new Vector3(11.22f, 7.89f, -27.2f);
        Camera.main.orthographicSize = 24.15614f;
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);

        firstCube.transform.position = new Vector3(0.98f, 6, -3.82f);
        firstCube.transform.rotation = Quaternion.Euler(0, 0, 0);
        firstCube.transform.localScale = new Vector3(3, 3, 3);


        secondCube.transform.position = new Vector3(12.34f, 6, -6.51f);
        secondCube.transform.rotation = Quaternion.Euler(0, 0, -90);
        secondCube.transform.localScale = new Vector3(7.5625f, 14.58468f, 7.2567f);


    }
}
