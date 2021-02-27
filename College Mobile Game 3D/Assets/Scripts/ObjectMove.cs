using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour, iControllable
{
    private Vector3 drag_position;
    bool objectBeenTapped;



    // Start is called before the first frame update
    void Start()
    {
        drag_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!objectBeenTapped)
        {
            transform.position = Vector3.Lerp(transform.position, drag_position, 0.2f);
        }

    }
    public void MoveTo(Vector3 destination)
    {
        drag_position = destination;
    }

    public void object_Has_Been_Tapped()
    {
        objectBeenTapped = true;
    }

    public void scalingForObjects(Vector3 initialScale, float factor)
    {
        //float maxFactor = Mathf.Clamp(factor, 1, 4);
        gameObject.transform.localScale = initialScale * factor;

        //this didnt work

/*        float currentScaleValue = gameObject.transform.localScale.magnitude;
        float scalingValue = Mathf.Clamp(currentScaleValue - incrementValue, 1f, 5.5f);
        gameObject.transform.localScale = new Vector3(scalingValue, scalingValue, scalingValue);*/
    }

    //this didnt work for the object scaling (previous version before the working one)

    /*    public void increaseSize()
        {

                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + 0.01f, gameObject.transform.localScale.y + 0.01f, gameObject.transform.localScale.z + 0.01f);

        }

        public void decreaseSize()
        {
            if (gameObject.transform.localScale != new Vector3(1, 1, 1))
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - 0.01f, gameObject.transform.localScale.y - 0.01f, gameObject.transform.localScale.z - 0.01f);
            }
        }*/


    public void oneFingerRotationYAxis(Quaternion rotationYAxisValue)
    {
        gameObject.transform.rotation *= rotationYAxisValue;
    }

    public void TwoFingerRotationLeftRight(Vector3 rotation)
    {
        gameObject.transform.Rotate(rotation, Space.Self);
    }

    public void acceleratorObjMovement(float accex, float acceY)
    {
        gameObject.transform.Translate(new Vector2((accex * 2), (acceY * 2)));
    }
}
