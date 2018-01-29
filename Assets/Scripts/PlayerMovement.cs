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

    private void Start()
    {
        _speed = _speed * Time.deltaTime;
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
                transform.Translate(-_speed, 0, 0);
                if (gameObject.transform.position.x <= _path[_currentNodeCounter + 1].transform.position.x)
                {
                    nextNodeInit();
                }
                break;
            case _directionStateEnum.right:
                transform.Translate(_speed, 0, 0);
                if (gameObject.transform.position.x >= _path[_currentNodeCounter + 1].transform.position.x)
                {
                    nextNodeInit();
                }
                break;
            case _directionStateEnum.up:
                transform.Translate(0, 0, _speed);
                if (gameObject.transform.position.z >= _path[_currentNodeCounter + 1].transform.position.z)
                {
                    nextNodeInit();
                }
                break;
            case _directionStateEnum.down:
                transform.Translate(0, 0, -_speed);
                if (gameObject.transform.position.z >= _path[_currentNodeCounter + 1].transform.position.z)
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
            gameObject.transform.position.y,
            node.transform.position.z
        );

        ResetAllNodePlayerHere();

        _path[_currentNodeCounter + 2].GetComponent<NodeInitialize>().playerHere = true;
    }

    private void nextNodeInit()
    {
        if (_currentNodeCounter >= _path.Count - 2)
        {
            _moveState = _moveStateEnum.idle;
            _directionState = _directionStateEnum.noDirection;
            return;
        }

        lockToNode(_path[_currentNodeCounter + 1]);
        _currentNodeCounter++;
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
