using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class touchManager : MonoBehaviour
{
    float tapCount;
    float moveCount;
    float endCount;
    GameObject ourCameraPlane;


    // Start is called before the first frame update

    iControllable selectedObject;
    private float starting_distance_to_selected_object;

    //Scaling:
    private Vector3 firstTouchPosition;
    private Vector3 secondTouchPosition;
    private float distance;

    //camera zooming
    private float pinchData;
    private float zoomOutMinValue = 4.5f;
    private float zoomOutMaxValue = 12;
    private float targetZoom;

    //to check if an object is being selected/messed with:
    private bool objectIsBeingUsed;

    //for object scaling:
    private float initialDistance;
    private Vector3 initialScale;

    //for y
    private float initialYDistance;

    //camera strafing:
    private Vector3 touchStart;

    //camera rotation
    private Vector3 firstPoint;
    private Vector3 secondPoint;
    private float xAngle = 0;
    private float yAngle = 0;
    private float xAngTemp = 0;
    private float yAngTemp = 0;

    //rotating object 2 fingers
    private Vector2 beganFirstFingerPosition;
    private Vector2 beganSecondFingerPosition;

    private Vector2 movedFirstFingerPosition;
    private Vector2 movedSecondFingerPosition;

    private Touch firstTouch;


    void Start()
    {
        ourCameraPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ourCameraPlane.transform.position = new Vector3(0, Camera.main.transform.position.y, 0);
        ourCameraPlane.transform.up = (Camera.main.transform.position - ourCameraPlane.transform.position).normalized;
        ourCameraPlane.GetComponent<MeshRenderer>().material.color = Color.red;

        targetZoom = Camera.main.orthographicSize;

        xAngle = 0;
        yAngle = 0;
        Camera.main.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0);
    }

    public void cameraZoomInorOut(float incrementValue)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - incrementValue, zoomOutMinValue, zoomOutMaxValue);
    }

    IEnumerator getSecondtouchPosition()
    {
        yield return new WaitForSeconds(2);

    }

    // Update is called once per frame
    void Update()
    {

        //CAMERA STUFF

        if (Input.touchCount > 0)
        {
            print(Input.GetTouch(0).position);

            if (!objectIsBeingUsed)
            {
                if(Input.touchCount == 1)
                {
                    //Camera Strafing
                    if (Input.GetMouseButtonDown(0))
                    {
                        touchStart = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    }
                    if (Input.GetMouseButton(0))
                    {
                        Vector3 cameraDirection = touchStart - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        Camera.main.transform.position += cameraDirection;
                    }

                    //camera rotation
                    Touch firstTouch = Input.GetTouch(0);
                    Vector2 firstTouchPos = Input.GetTouch(0).position;

                    StartCoroutine(getSecondtouchPosition());
                    // if(firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Stationary)
                    if (firstTouch.phase == TouchPhase.Stationary)
                    {
                        Camera.main.transform.Rotate(Camera.main.transform.forward, Space.World);
                    }
                }
     


                if (Input.touchCount >= 2)
                {
                        //camera zooming
                        firstTouch = Input.GetTouch(0);
                        Touch secondTouch = Input.GetTouch(1);

                        Vector2 touchFirstPrevPosition = firstTouch.position - firstTouch.deltaPosition;
                        Vector2 touchSecondPrevPosition = secondTouch.position - secondTouch.deltaPosition;

                        //magnitude calculates the length
                        float prevMagnitude = (touchFirstPrevPosition - touchSecondPrevPosition).magnitude;
                        float currentMagniute = (firstTouch.position - secondTouch.position).magnitude;

                        float difference = currentMagniute - prevMagnitude;

                        print("Camera difference: " + difference);

                        cameraZoomInorOut(difference * 0.01f);



                        //THIS WAS THE PREVIOUS METHOD OF ZOOMING IN DIDNT REALLY WORK WITH PINCING OUT OR IN
                        //BUT THE ZOOMING PART OF IT WORKED JUST THE PINCHING DIDNT WORK.

                        /*                distance = Vector3.Distance(firstTouchPosition, secondTouchPosition);
                                        print(distance);

                                        if (distance > 500)
                                        {
                                            pinchData = 0.1f;
                                        }
                                        else if (distance < 300)
                                        {
                                            pinchData = -0.1f;
                                        }

                                        targetZoom -= pinchData * 3f;
                                        targetZoom = Mathf.Clamp(targetZoom, 4.5f, 12f);
                                        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime * 10);*/

                        //Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 0.1f, Time.deltaTime * 2f);

                    }


                //camera rotaion 2 fingers

                if (Input.touchCount >= 2)
                {
                    Touch firstFingerPos = Input.GetTouch(0);
                    Touch secondFingerPos = Input.GetTouch(1);


                    if (firstFingerPos.phase == TouchPhase.Moved)
                    {
                        beganFirstFingerPosition = Input.GetTouch(0).position;
                    }
                    else if (secondFingerPos.phase == TouchPhase.Moved)
                    {
                        beganSecondFingerPosition = Input.GetTouch(0).position;
                    }

                    if (firstFingerPos.phase == TouchPhase.Stationary)
                    {
                        movedFirstFingerPosition = Input.GetTouch(0).position;
                    }
                    else if (secondFingerPos.phase == TouchPhase.Stationary)
                    {
                        movedSecondFingerPosition = Input.GetTouch(0).position;
                    }

                    float TwoFingersBeganMagnitudeX = beganFirstFingerPosition.x + beganSecondFingerPosition.x;
                    float TwoFingersMovedMagnitudeX = movedFirstFingerPosition.x + movedSecondFingerPosition.x;

                    float TwoFingersBeganMagnitudeY = beganFirstFingerPosition.y + beganSecondFingerPosition.y;
                    float TwoFingersMovedMagnitudeY = movedFirstFingerPosition.y + movedSecondFingerPosition.y;




                    if (TwoFingersBeganMagnitudeX < TwoFingersMovedMagnitudeX)
                    {
                        Camera.main.transform.Rotate(transform.up, Space.World);
                        //Camera.main.transform.Rotate(Camera.main.transform.right, Space.World);
                    }
                    else if (TwoFingersBeganMagnitudeX > TwoFingersMovedMagnitudeX)
                    {
                        Camera.main.transform.Rotate(-transform.up, Space.World);
                        //Camera.main.transform.Rotate(-Camera.main.transform.right, Space.World);
                    }


                    if (TwoFingersBeganMagnitudeY < TwoFingersMovedMagnitudeY)
                    {
                        Camera.main.transform.Rotate(transform.right, Space.World);
                        //Camera.main.transform.Rotate(Camera.main.transform.right, Space.World);
                    }
                    else if (TwoFingersBeganMagnitudeY > TwoFingersMovedMagnitudeY)
                    {
                        Camera.main.transform.Rotate(-transform.right, Space.World);
                        //Camera.main.transform.Rotate(-Camera.main.transform.right, Space.World);
                    }

                }

            }

            //OBJECT STUFF

            Touch touch = Input.touches[0];
            Ray ourRay = Camera.main.ScreenPointToRay(touch.position);

            RaycastHit hitInfo;
            if (Physics.Raycast(ourRay, out hitInfo))
            {
                if (hitInfo.collider != null)
                {
                   // Color changeColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                   // hitInfo.collider.GetComponent<MeshRenderer>().material.color = changeColor;


                    iControllable objectHit = hitInfo.transform.GetComponent<iControllable>();
                    selectedObject = objectHit;

                    Outline outline = hitInfo.transform.GetComponent<Outline>();




                    starting_distance_to_selected_object = Vector3.Distance(Camera.main.transform.position, hitInfo.transform.position);
                    //starting_distance_to_selected_object = Vector3.Distance(notParallelPlane.transform.position, hitInfo.transform.position);

                    if (objectHit != null)
                    {
                        outline.enabled = true;
                        selectedObject.acceleratorObjMovement(Input.acceleration.y, Input.acceleration.x);


                        if (Input.touchCount >=2)
                        {

                            //TRY TO USE THE SAME TECHNIQUE USED IN CAMERA ZOOMING FOR CUBE SCALING BUT IT DIDNT WORK.

                            /*                            //cube zooming
                                                        Touch firstTouch = Input.GetTouch(0);
                                                        Touch secondTouch = Input.GetTouch(1);

                                                        Vector2 touchFirstPrevPosition = firstTouch.position - firstTouch.deltaPosition;
                                                        Vector2 touchSecondPrevPosition = secondTouch.position - secondTouch.deltaPosition;

                                                        //magnitude calculates the length
                                                        float prevMagnitude = (touchFirstPrevPosition - touchSecondPrevPosition).magnitude;
                                                        float currentMagniute = (firstTouch.position - secondTouch.position).magnitude;

                                                        float difference = currentMagniute - prevMagnitude;

                                                        float scalingValue = Mathf.Clamp(hitInfo.transform.localScale.magnitude - difference, zoomOutMinValue, zoomOutMaxValue);

                                                        hitInfo.transform.localScale = new Vector3(scalingValue, scalingValue, scalingValue);
                            */

                            //I USED THE SAME METHOD (AS BEFORE FOR THE CAMERA SCALING), 
                            //THE ACTUAL SCALING WORKED BUT THE PINCHING DIDNT WORK

                                Touch firstTouchPositionForCube = Input.GetTouch(0);
                                Touch secondTouchPositionForCube = Input.GetTouch(1);

                                if (firstTouchPositionForCube.phase == TouchPhase.Began || secondTouchPositionForCube.phase == TouchPhase.Began)
                                {
                                    initialDistance = Vector2.Distance(firstTouchPositionForCube.position, secondTouchPositionForCube.position);

                                    if (selectedObject != null)
                                    {
                                        initialScale = hitInfo.transform.localScale;
                                    }
                                }
                                else
                                {

                                    float currentDistance = Vector2.Distance(firstTouchPositionForCube.position, secondTouchPositionForCube.position);

                                    if (Mathf.Approximately(initialDistance, 0))
                                    {
                                        return;
                                    }

                                    float factor = currentDistance / initialDistance;

                                    if (selectedObject != null)
                                    {
                                        selectedObject.scalingForObjects(initialScale, factor);
                                    }

                                }
                            



                            /*                            if (difference > 500)
                                                        {
                                                        selectedObject.increaseSize();
                                                        }
                                                        else if (difference < 300)
                                                        {
                                                        selectedObject.decreaseSize();
                                                        }*/


                        }

                        //if(selectedObject != null)
                      //  {
                            
                        //}

                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                tapCount = 1;
                                if(tapCount == 1)
                                {
                                    print("SCREEN TAPPED: The object/screen has been tapped");
                                    objectIsBeingUsed = true;
                                }

                                //objectHit.object_Has_Been_Tapped();
                                break;

                            case TouchPhase.Moved:
                                moveCount++;
                                if(moveCount > 1)
                                {
                                    objectIsBeingUsed = true;

                                    //DRAG
                                    print("OBJECT MOVED: The object/touch is being moved");
                                    //camera perpendicular method

                                        Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 15));
                                        selectedObject.MoveTo(transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime * 10));
                                    
                                    //the plane following one:
                                        Ray newPositionRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
                                        RaycastHit[] hits = Physics.RaycastAll(newPositionRay);
                                        int groundMask = LayerMask.NameToLayer("Ground");

                                        foreach (RaycastHit hit in hits)
                                        {
                                            if (hit.transform.gameObject.layer == groundMask)
                                            {
                                                selectedObject.MoveTo(hit.point);
                                            }
                                        }
                                }


                                break;

                            case TouchPhase.Ended:
                                endCount = 1;
                                if(endCount == 1)
                                {
                                    objectIsBeingUsed = false;
                                    outline.enabled = false;
                                    print("TAP/MOVE ENDED: The object/screen touching/tapping ended");
                                }
                                break;

                            case TouchPhase.Stationary:

                                //1 finger rotation
                                if (Input.touchCount == 1)
                                {
                                    float rotateSpeedModified = 0.1f;
                                    Quaternion rotationYaxis = Quaternion.Euler(0f, -Input.touches[0].position.x * rotateSpeedModified, 0f);

                                    selectedObject.oneFingerRotationYAxis(rotationYaxis);
                                }

                                //2 finger rotation
                                if (Input.touchCount >= 2)
                                {
                                    Touch firstFingerPos = Input.GetTouch(0);
                                    Touch secondFingerPos = Input.GetTouch(1);


                                    if (firstFingerPos.phase == TouchPhase.Began)
                                    {
                                        beganFirstFingerPosition = Input.GetTouch(0).position;
                                    }
                                    else if (secondFingerPos.phase == TouchPhase.Began)
                                    {
                                        beganSecondFingerPosition = Input.GetTouch(0).position;
                                    }

                                    if (firstFingerPos.phase == TouchPhase.Moved)
                                    {
                                        movedFirstFingerPosition = Input.GetTouch(0).position;
                                    }
                                    else if (secondFingerPos.phase == TouchPhase.Moved)
                                    {
                                        movedSecondFingerPosition = Input.GetTouch(0).position;
                                    }

                                    float TwoFingersBeganMagnitudeX = beganFirstFingerPosition.x + beganSecondFingerPosition.x;
                                    float TwoFingersMovedMagnitudeX = movedFirstFingerPosition.x + movedSecondFingerPosition.x;

                                    float TwoFingersBeganMagnitudeY = beganFirstFingerPosition.y + beganSecondFingerPosition.y;
                                    float TwoFingersMovedMagnitudeY = movedFirstFingerPosition.y + movedSecondFingerPosition.y;



                                    if (TwoFingersBeganMagnitudeX < TwoFingersMovedMagnitudeX)
                                    {
                                        selectedObject.TwoFingerRotationLeftRight(transform.right);
                                        //Camera.main.transform.Rotate(Camera.main.transform.right, Space.World);
                                    }
                                    else if (TwoFingersBeganMagnitudeX > TwoFingersMovedMagnitudeX)
                                    {
                                        selectedObject.TwoFingerRotationLeftRight(-transform.right);
                                        //Camera.main.transform.Rotate(-Camera.main.transform.right, Space.World);
                                    }


                                    if (TwoFingersBeganMagnitudeY < TwoFingersMovedMagnitudeY)
                                    {
                                        selectedObject.TwoFingerRotationLeftRight(transform.up);
                                        //Camera.main.transform.Rotate(Camera.main.transform.right, Space.World);
                                    }
                                    else if (TwoFingersBeganMagnitudeY > TwoFingersMovedMagnitudeY)
                                    {
                                        selectedObject.TwoFingerRotationLeftRight(-transform.up);
                                        //Camera.main.transform.Rotate(-Camera.main.transform.right, Space.World);
                                    }

                                }

                                break;
                        }

                    }
                    Debug.DrawRay(ourRay.origin, 30 * ourRay.direction, Color.black);

                }
            }
        }
    }
}
