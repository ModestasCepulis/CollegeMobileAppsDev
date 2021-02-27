using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface iControllable
{
    void object_Has_Been_Tapped();
    void MoveTo(Vector3 destination);

    void scalingForObjects(Vector3 initialScale, float factor);

    void oneFingerRotationYAxis(Quaternion rotationYAxisValue);

    void TwoFingerRotationLeftRight(Vector3 rotation);

    void acceleratorObjMovement(float accelerationX, float accelerationY);
}
