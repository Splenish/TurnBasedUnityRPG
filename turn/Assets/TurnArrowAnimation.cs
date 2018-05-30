using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnArrowAnimation : MonoBehaviour
{
    public AnimationCurve myCurve;

    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, myCurve.Evaluate((Time.time % myCurve.length)), transform.localPosition.z);
    }
}

