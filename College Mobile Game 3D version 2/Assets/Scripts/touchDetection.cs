using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchDetection : MonoBehaviour
{

    Vector2 touchPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            print(Input.touches[0].position);
            touchPosition = Input.touches[0].position;

            if(touchPosition.x < 750)
            {
                transform.position += Vector3.left * 3f * Time.deltaTime;
            }
            else if(touchPosition.x > 1200)
            {
                transform.position += transform.right * 3f * Time.deltaTime;

            }
            else if (touchPosition.y < 400)
            {
                transform.position += Vector3.down * 3f * Time.deltaTime;

            }
            else if (touchPosition.y > 700)
            {
                transform.position += Vector3.up * 3f * Time.deltaTime;

            }
            else if(touchPosition.x > 1200 && touchPosition.y > 750)
            {
                transform.position += Vector3.up * 1.5f * Time.deltaTime;
                transform.position += Vector3.right * 1.5f * Time.deltaTime;
            }

        }

    }

   
}
