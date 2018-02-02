using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    public float rotSpeed = 20;
    private float border;

    public static bool MouseDown = false;
    public static Transform prevRay;
    public Transform Rotation;

    // Update is called once per frame
    void Update()
    {
        RotateObject();
    }
    void RotateObject()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log(transform.eulerAngles.y);
            float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
            //float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

            if (prevRay == null)
            {
                prevRay = RaycastManager.GetRaycastHit().transform;
                MouseDown = true;
                Pillar.PillarActive = true;
            }

            //if (prevRay == Rotation && MouseDown && border < 90 && border >= 0) 
            if (prevRay == Rotation && MouseDown && RaycastManager.hit.transform.tag == "Handle")
            {
                //transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X")) * Time.deltaTime * speed);

                transform.Rotate(Vector3.up, -rotX, Space.World);
                //transform.RotateAround(Vector3.right, rotY);
            }
        }
        else
        {
            border = transform.localEulerAngles.y;
            border = Mathf.Clamp(border, 0.5f, 90);

            //Debug.Log(border);

            if (transform.eulerAngles.y > 90 && transform.eulerAngles.y < 270)
            {
                Debug.Log("Too far");
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }

            else if (transform.eulerAngles.y > 270)
            {
                Debug.Log("Too short");
                transform.rotation = Quaternion.Euler(0, 0.5f, 0);
            }

            prevRay = null;
            MouseDown = false;
            Pillar.PillarActive = false;
        }
    }
}
