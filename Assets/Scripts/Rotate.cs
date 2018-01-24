using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    //Public Vars
    public Camera camera;
    public float speed = 100f;

    //Private Vars
    private Vector3 mousePosition;
    private Vector3 direction;
    private float distanceFromObject;

    //public float speed = 100.0f;
    private bool rotatingClockwise = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotatingObjects();
        //mousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - camera.transform.position.z));

        //Rotates toward the mouse
        //GetComponent<Rigidbody>().transform.eulerAngles = new Vector3(0, 0, -Mathf.Atan2((mousePosition.y - transform.position.y), -(mousePosition.x - transform.position.x)) * Mathf.Rad2Deg - 90);
        //distanceFromObject = (Input.mousePosition - camera.WorldToScreenPoint(transform.position)).magnitude;
    }

    void RotatingObjects()
    {
        /*
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

        if (Input.anyKeyDown)
        {
            rotatingClockwise = !rotatingClockwise;
        }
        */
        transform.Rotate(new Vector3(0, 0, Input.GetAxis("Mouse Y")) * Time.deltaTime * speed);

    }

}