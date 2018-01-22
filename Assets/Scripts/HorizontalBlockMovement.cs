using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBlockMovement : MonoBehaviour {

    public float speed;
    public float maxSpeed = 50;
    public float keerSpeed = 140;

    public string directionType = "x";

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
                //float delta = Input.mousePosition.x - lastPos.x;
                //OnMouseDown();
                OnMouseDrag();
                //lastPos = Input.mousePosition;
                //speed = delta * Time.deltaTime * 2;
            }      
        }
        if(!Input.GetMouseButton(0))
        {
            speed = 0;
        }
        _prevMousePos = Input.mousePosition;
        
        if(speed >= maxSpeed)
        {
            speed = maxSpeed;
        }
        
    }
    /*
       void OnMouseDown()
       {
           screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
           offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, gameObject.transform.position.y, screenPoint.z));
       }
    */
    void OnMouseDrag()
    {
        /*
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, gameObject.transform.position.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        */

        switch (directionType)
        {
            case "x":
                _mouseDelta = Input.mousePosition - _prevMousePos;
                _delta = Input.mousePosition.x - _lastPos.x;
                _direction = 0;
                if (_mouseDelta.x < 0)
                {
                    if (gameObject.transform.position.x <= firstPoint.transform.position.x - 1.01f)
                        _direction = Mathf.Floor(_mouseDelta.normalized.x);
                    if (gameObject.transform.position.x > firstPoint.transform.position.x - 1f)
                        gameObject.transform.position = new Vector3(firstPoint.transform.position.x - 1f, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else
                {
                    if (gameObject.transform.position.x >= secondPoint.transform.position.x + 1.01f)
                        _direction = Mathf.Ceil(_mouseDelta.normalized.x);
                    if (gameObject.transform.position.x < secondPoint.transform.position.x + 1f)
                        gameObject.transform.position = new Vector3(secondPoint.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                transform.position += new Vector3(-_direction,0, 0) * speed * Time.deltaTime;
                _lastPos = Input.mousePosition;
                speed = Mathf.Abs(_delta * Time.deltaTime * keerSpeed);
                //_prevMousePos = Input.mousePosition;
            break;
            case "y":
                _mouseDelta = Input.mousePosition - _prevMousePos;

                _delta = Input.mousePosition.y - _lastPos.y;
                _direction = 0;
                if (_mouseDelta.y < 0)
                {
                    if (gameObject.transform.position.y >= firstPoint.transform.position.y + 1.01f)
                        _direction = Mathf.Floor(_mouseDelta.normalized.y);
                    if (gameObject.transform.position.y < firstPoint.transform.position.y + 1f)
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, firstPoint.transform.position.y + 1f, gameObject.transform.position.z);
                }
                else
                {
                    if (gameObject.transform.position.y <= secondPoint.transform.position.y - 1.01f)
                        _direction = Mathf.Ceil(_mouseDelta.normalized.y);
                    if (gameObject.transform.position.y > secondPoint.transform.position.y - 1f)
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, secondPoint.transform.position.y - 1f, gameObject.transform.position.z);
                }
                transform.Translate(new Vector3(0, _direction, 0) * speed * Time.deltaTime);
                _lastPos = Input.mousePosition;
                speed = Mathf.Abs(_delta * Time.deltaTime * keerSpeed);
                _prevMousePos = Input.mousePosition;
            break;
            case "z":
                _mouseDelta = Input.mousePosition - _prevMousePos;
                _delta = Input.mousePosition.y - _lastPos.y;
                _direction = 0;
                if (_mouseDelta.y < 0)
                {
                    if (gameObject.transform.position.z <= firstPoint.transform.position.z - 1.01f)
                        _direction = Mathf.Floor(_mouseDelta.normalized.y);
                    if (gameObject.transform.position.z > firstPoint.transform.position.z - 1f)
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, firstPoint.transform.position.z -1);
                }
                else
                {
                    if (gameObject.transform.position.z >= secondPoint.transform.position.z + 1.01f)
                        _direction = Mathf.Ceil(_mouseDelta.normalized.y);
                    if (gameObject.transform.position.z < secondPoint.transform.position.z + 1f)
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, secondPoint.transform.position.z + 1f);

                }
                transform.Translate(new Vector3(0, 0, -_direction) * speed * Time.deltaTime);
                _lastPos = Input.mousePosition;
                speed = Mathf.Abs(_delta * Time.deltaTime * keerSpeed);
                _prevMousePos = Input.mousePosition;
                Debug.Log(Input.mousePosition.z - _lastPos.y);
             break;
        }
    }

}

