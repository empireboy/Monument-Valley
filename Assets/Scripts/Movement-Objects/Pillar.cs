using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    private bool stopPillar = false;
    [SerializeField] private int speed = 4;
    [SerializeField] private Transform pillarBody;
    [SerializeField] private GameObject pillarPoint;
    public static bool PillarActive = false;
    private float _pillarBorder;

    private Vector3 _prevMousePos;
    private Vector3 _mouseDelta;
    private float _direction;

    // Update is called once per frame
    void Update ()
    {
        PillarMove();
	}

    void PillarMove()
    {
        _pillarBorder = gameObject.transform.position.y;

        if (Input.GetMouseButton(0))
        {
            if (MouseDrag.prevRay && PillarActive && RaycastManager.hit.transform.tag == "Handle")
            {
                _pillarBorder = transform.position.y;
                _mouseDelta = Input.mousePosition - _prevMousePos;
                _direction = 0;
                Debug.Log(_mouseDelta);
                if (_mouseDelta.x < 0)
                {
                        if (gameObject.transform.position.y >= pillarPoint.transform.position.y - 5)
                            _direction = Mathf.Floor(_mouseDelta.normalized.x);
                }
                else if(_mouseDelta.x > 0)
                {
                    if (gameObject.transform.position.y <= pillarPoint.transform.position.y - 0.5)
                        _direction = Mathf.Ceil(_mouseDelta.normalized.x);
                }
                //Debug.Log(_direction);
                transform.Translate(new Vector3(0, _direction, 0) * Time.deltaTime * speed);
                _prevMousePos = Input.mousePosition;
            }

            /*
            Debug.Log(_pillarBorder);
            if (MouseDrag.prevRay && PillarActive && RaycastManager.hit.transform.tag == "Handle")
            {
                if (gameObject.transform.position.y <= pillarPoint.transform.position.y - 0.5f)
                {
                    stopPillar = false;
                }
                else
                {
                    stopPillar = true;
                }

                //if (gameObject.transform.position.y <= pillarPoint.transform.position.y - 0.5f)
                if (!stopPillar)
                {
                    //transform.Translate(0, speed, 0);
                    transform.Translate(new Vector3(0, Input.GetAxis("Mouse X")) * Time.deltaTime * speed);
                }
                else
                {
                    //transform.position = new Vector3(-5.6f, -26.7f - 1, -19.8f);
                    //transform.position = new Vector3(pillarPoint.transform.position.x, -26.7f, pillarPoint.transform.position.z);
                }
            }

            /*
            else if (gameObject.transform.position.y <= pillarPoint.transform.position.y - 30f)
            {
                //Debug.Log();
            }

            else if (stopPillar && _pillarBorder < -30)
            {
                transform.Translate(new Vector3(0, 5) * Time.deltaTime * speed);
                stopPillar = false;
            }

                if (_pillarBorder > -26.7f)
                {

                    //pillarBody.position = new Vector3(0, -26.7f);
                    stopPillar = true;
                }
                else if (_pillarBorder < -30)
                {
                    Debug.Log("Too short");
                    //pillarBody.position = new Vector3(0, -30);
                    stopPillar = true;
                }
            }
            */
        }
        
    }
}
