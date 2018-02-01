using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

     public static bool rotatingClockwise = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotatingObjects();
    }

    void RotatingObjects()
    {
        if (rotatingClockwise && transform.eulerAngles.z < 90)
        {
            transform.Rotate(0, 0, 1);
        }
        else if (rotatingClockwise)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        if (!rotatingClockwise && transform.eulerAngles.z >= 1)
        {
            transform.Rotate(0, 0, -1);
        }
        else if (!rotatingClockwise)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        /*

        if (Input.anyKeyDown)
        {
            rotatingClockwise = !rotatingClockwise;
        }

        */
    }
}