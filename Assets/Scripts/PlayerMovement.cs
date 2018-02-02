using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private List<GameObject> _path = new List<GameObject>();
    private GameObject _nextNode;
    private enum _moveStateEnum { walk, idle };
    private enum _directionStateEnum { left, right, up, down, noDirection };
    private _moveStateEnum _moveState = _moveStateEnum.idle;
    private _directionStateEnum _directionState = _directionStateEnum.noDirection;
    private int _currentNodeCounter = 0;

    [SerializeField] private bool _debug = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private void Start()
    {
        _speed = _speed * Time.deltaTime;
        _rotationSpeed = _rotationSpeed * Time.deltaTime;
    }

    private void Update()
    {
        StateMachine();
    }


    public void PathCreate(List<GameObject> nodeArray)
    {
        _currentNodeCounter = 0;
        _path.Clear();
        for (int i = nodeArray.Count; i > 0; i--)
        {
            _path.Add(nodeArray[i-1]);
            if (_debug) Debug.Log("Player node next: " + nodeArray[i - 1]);
        }

        _directionState = _directionStateEnum.noDirection;
        _moveState = _moveStateEnum.walk;
    }

    private void Move()
    {
        switch (_directionState)
        {
            case _directionStateEnum.left:
                transform.position += new Vector3(-_speed, 0, 0);
                // Rotation
                if (transform.eulerAngles.y < 90)
                {
                    transform.Rotate(new Vector3(0, _rotationSpeed, 0));
                }
                else if (transform.eulerAngles.y > 90)
                {
                    transform.Rotate(new Vector3(0, -_rotationSpeed, 0));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                // Next node
                if (gameObject.transform.position.x <= _path[_currentNodeCounter + 1].transform.position.x)
                {
                    nextNodeInit();
                }
                break;
            case _directionStateEnum.right:
                transform.position += new Vector3(_speed, 0, 0);
                // Rotation
                if (transform.eulerAngles.y < 270)
                {
                    transform.Rotate(new Vector3(0, _rotationSpeed, 0));
                }
                else if (transform.eulerAngles.y > 270)
                {
                    transform.Rotate(new Vector3(0, -_rotationSpeed, 0));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                }
                // Next node
                if (gameObject.transform.position.x >= _path[_currentNodeCounter + 1].transform.position.x)
                {
                    nextNodeInit();
                }
                break;
            case _directionStateEnum.up:
                transform.position += new Vector3(0, 0, _speed);
                // Rotation
                if (transform.eulerAngles.y < 180)
                {
                    transform.Rotate(new Vector3(0, _rotationSpeed, 0));
                }
                else if (transform.eulerAngles.y > 180)
                {
                    transform.Rotate(new Vector3(0, -_rotationSpeed, 0));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                // Next node
                if (gameObject.transform.position.z >= _path[_currentNodeCounter + 1].transform.position.z)
                {
                    nextNodeInit();
                }
                break;
            case _directionStateEnum.down:
                transform.position += new Vector3(0, 0, -_speed);
                // Rotation
                if (transform.eulerAngles.y > 0 && transform.eulerAngles.y < 360 - 5)
                {
                    transform.Rotate(new Vector3(0, -_rotationSpeed, 0));
                }
                else if (transform.eulerAngles.y > 360 + 5)
                {
                    transform.Rotate(new Vector3(0, _rotationSpeed, 0));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                // Next node
                if (gameObject.transform.position.z <= _path[_currentNodeCounter + 1].transform.position.z)
                {
                    nextNodeInit();
                }
                break;
        }
    }

    private void GetDirection(GameObject node1, GameObject node2)
    {
        if (node1.transform.position.x > node2.transform.position.x) _directionState = _directionStateEnum.left;
        else if (node1.transform.position.x < node2.transform.position.x) _directionState = _directionStateEnum.right;
        else if (node1.transform.position.z < node2.transform.position.z) _directionState = _directionStateEnum.up;
        else if (node1.transform.position.z > node2.transform.position.z) _directionState = _directionStateEnum.down;
    }

    private void StateMachine()
    {
        switch (_moveState)
        {
            case _moveStateEnum.idle:
                lockToNode(_path[_currentNodeCounter + 1]);
                break;
            case _moveStateEnum.walk:
                if (_directionState == _directionStateEnum.noDirection) GetDirection(_path[_currentNodeCounter], _path[_currentNodeCounter+1]);
                Move();
                break;
        }
    }

    private void lockToNode(GameObject node)
    {
        gameObject.transform.position = new Vector3
        (
            node.transform.position.x,
            node.transform.position.y,
            node.transform.position.z
        );

        if (_path.Count - 1 >= _currentNodeCounter + 2)
        {
            ResetAllNodePlayerHere();
            _path[_currentNodeCounter + 2].GetComponent<NodeInitialize>().playerHere = true;
        }
    }

    private void nextNodeInit()
    {
        lockToNode(_path[_currentNodeCounter + 1]);

        if (_currentNodeCounter >= _path.Count - 2)
        {
            _moveState = _moveStateEnum.idle;
            return;
        }

        if (_path.Count - 1 >= _currentNodeCounter + 2)
        {
            _currentNodeCounter++;
        }
        if (_debug) Debug.Log("Player current node: " + _path[_currentNodeCounter + 1]);
        _directionState = _directionStateEnum.noDirection;
    }

    private void ResetAllNodePlayerHere()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Path");

        for (int i = 0; i < objectsWithTag.Length; i++)
        {
            NodeInitialize currentNodeInit = objectsWithTag[i].GetComponent<NodeInitialize>();
            currentNodeInit.playerHere = false;
        }
    }
}
