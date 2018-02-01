using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private List<GameObject> _path = new List<GameObject>();
    private GameObject _nextNode;
    private int _currentNodeCounter;

    [SerializeField] private bool _debug = false;


    public void PathCreate(List<GameObject> nodeArray)
    {
        // Get path array
        for (int i = nodeArray.Count; i > 0; i--)
        {
            _path.Add(nodeArray[i-1]);
            if (_debug) Debug.Log("Player node next: " + nodeArray[i - 1]);
        }

        // Start movement
        Move();
    }

    private GameObject GetNodeNext(List<GameObject> path)
    {
        _currentNodeCounter++;
        return path[_currentNodeCounter];
    }

    private void Move()
    {
        for (int i = 0; _path.Count > 0; i++)
        {
            gameObject.transform.position = _path[i].transform.position;
        }
    }

    private void GetDirection()
    {

    }
}
