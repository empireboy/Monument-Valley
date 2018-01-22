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
        for (int i = nodeArray.Count; i > 0; i--)
        {
            _path.Add(nodeArray[i-1]);
            if (_debug) Debug.Log("Player node next: " + nodeArray[i - 1]);
        }
    }

    private GameObject GetNodeNext(List<GameObject> path)
    {
        return path[_currentNodeCounter + 1];
    }

    private void Move()
    {
        
    }

    private void GetDirection()
    {

    }
}
