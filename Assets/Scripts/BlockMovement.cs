using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft;

public class BlockMovement : MonoBehaviour {


    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed = 50;
    [SerializeField] private float keerSpeed = 140;

    [SerializeField] private string directionType = "x";

    [SerializeField]
    private GameObject firstPoint;
    [SerializeField]
    private GameObject secondPoint;

    private Vector3 _screenPoint;
    private Vector3 _offset;

    private Vector3 _mouseDelta;
    private Vector3 _lastPos;
    private Vector3 _prevMousePos;
    private float _delta;
    private float _direction;
    
    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(RaycastManager.GetRaycastHit());
            if (RaycastManager.GetRaycastHit() == gameObject)
            {
                Cursor.lockState = CursorLockMode.Confined;
                OnMouseDrag();
            }      
        }
        if(!Input.GetMouseButton(0))
        {
            //makes speed 0 s
            speed = 0;
        }
        _prevMousePos = Input.mousePosition;
        
        //Keeps the speed low
        if(speed >= maxSpeed)
        {
            speed = maxSpeed;
        }
    }

    void OnMouseDrag()
    {

        switch (directionType)
        {
            case "x": //For moving in the X direction
                //mouseDelta for direction
                _mouseDelta = Input.mousePosition - _prevMousePos;
                //mouseDelta for speed
                _delta = Input.mousePosition.x - _lastPos.x;
                _direction = 0;
                if (_mouseDelta.x < 0)
                {
                    if (gameObject.transform.parent.position.x <= firstPoint.transform.position.x - 1.451f)
                        _direction = Mathf.Floor(_mouseDelta.normalized.x);
                }
                else
                {
                    if (gameObject.transform.parent.position.x >= secondPoint.transform.position.x + 1.451f)
                        _direction = Mathf.Ceil(_mouseDelta.normalized.x);
                }
                if (gameObject.transform.parent.position.x > firstPoint.transform.position.x - 1.45f)
                    gameObject.transform.parent.position = new Vector3(firstPoint.transform.position.x - 1.45f, gameObject.transform.parent.position.y, gameObject.transform.parent.position.z);
                if (gameObject.transform.parent.position.x < secondPoint.transform.position.x + 1.45f)
                    gameObject.transform.parent.position = new Vector3(secondPoint.transform.position.x + 1.45f, gameObject.transform.parent.position.y, gameObject.transform.parent.position.z);

                //moves the parent in a direction
                gameObject.transform.parent.Translate(new Vector3(0, 0, -_direction) * speed * Time.deltaTime);

                _lastPos = Input.mousePosition;//last position of the mouse for speed
                speed = Mathf.Abs(_delta * Time.deltaTime * keerSpeed);
                _prevMousePos = Input.mousePosition;//last mouse position if the mouse for direction
                break;
            case "y": //For moving in the Y direction
                //mouseDelta for direction/
                _mouseDelta = Input.mousePosition - _prevMousePos;
                //mouseDelta for speed
                _delta = Input.mousePosition.y - _lastPos.y;
                _direction = 0;
                if (_mouseDelta.y < 0)
                {
                    if (gameObject.transform.parent.position.y >= firstPoint.transform.position.y + 1.51f)
                        _direction = Mathf.Floor(_mouseDelta.normalized.y);
                }
                else
                {
                    if (gameObject.transform.parent.position.y <= secondPoint.transform.position.y - 1.51f)
                        _direction = Mathf.Ceil(_mouseDelta.normalized.y);
                }
                if (gameObject.transform.parent.position.y < firstPoint.transform.position.y + 1.5f)
                    gameObject.transform.parent.position = new Vector3(gameObject.transform.parent.position.x, firstPoint.transform.position.y + 1.5f, gameObject.transform.parent.position.z);
                if (gameObject.transform.parent.position.y > secondPoint.transform.position.y - 1.5f)
                    gameObject.transform.parent.position = new Vector3(gameObject.transform.parent.position.x, secondPoint.transform.position.y - 1.5f, gameObject.transform.parent.position.z);
                
                //moves the parent in a direction
                gameObject.transform.parent.Translate(new Vector3(0, _direction, 0) * speed * Time.deltaTime);

                _lastPos = Input.mousePosition;//last position of the mouse for speed
                speed = Mathf.Abs(_delta * Time.deltaTime * keerSpeed);
                _prevMousePos = Input.mousePosition;//last mouse position if the mouse for direction
            break;
            case "z": //For moving in the Z direction
                //mouseDelta for direction
                _mouseDelta = Input.mousePosition - _prevMousePos;
                //mouseDelta for speed
                _delta = Input.mousePosition.y - _lastPos.y;
                _direction = 0;
                if (_mouseDelta.y < 0)
                {
                    if (gameObject.transform.parent.position.z <= firstPoint.transform.position.z - 1.45f)
                        _direction = Mathf.Floor(_mouseDelta.normalized.y);
                }
                else
                {
                    if (gameObject.transform.parent.position.z >= secondPoint.transform.position.z + 1.45f)
                        _direction = Mathf.Ceil(_mouseDelta.normalized.y);

                }
                if (gameObject.transform.parent.position.z > firstPoint.transform.position.z - 1.44f)
                    gameObject.transform.parent.position = new Vector3(gameObject.transform.parent.position.x, gameObject.transform.parent.position.y, firstPoint.transform.position.z - 1.5f);
                if (gameObject.transform.parent.position.z < secondPoint.transform.position.z + 1.44f)
                    gameObject.transform.parent.position = new Vector3(gameObject.transform.parent.position.x, gameObject.transform.parent.position.y, secondPoint.transform.position.z + 1.5f);

                //moves the parent in a direction
                gameObject.transform.parent.Translate(new Vector3(0, 0, -_direction) * speed * Time.deltaTime);

                _lastPos = Input.mousePosition; //last position of the mouse for speed
                speed = Mathf.Abs(_delta * Time.deltaTime * keerSpeed);
                _prevMousePos = Input.mousePosition;//last mouse position if the mouse for direction
                break;
        }
    }

}

