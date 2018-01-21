using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour {
    private GameObject _nextNode;
    //private List<GameObject> _nodeArrayFinished = new List<GameObject>();
    private List<List<GameObject>> _nodeArrayFinished = new List<List<GameObject>>();
    private bool _pathCheckFinished = false;
    private bool _pathsCheckFinished = false;
    private bool _isAnalyzing = false;
    private float _nextNodeDistanceToPlayer;
    private int _pathCountdown;
    private int _pathsCountdown;
    private int _totalPathsCountdown;

    [SerializeField] private bool _debug = false;
    [SerializeField] private int _countdownMax;
    [SerializeField] private int _totalPathsMax;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject hit = RaycastManager.GetRaycastHit();
            if (PathCanStart(hit)) PathInitialize(hit);         // Initialize variables for path analyzing
        }
    }

    private bool PathCanStart(GameObject hit)
    {
        if (
            hit != null &&
            hit.tag == "Path" &&
            !_isAnalyzing
        )
        {
            return true;
        }

        return false;
    }

    private void PathInitialize(GameObject endObject)
    {
        _nodeArrayFinished.Add(new List<GameObject>());         // Create the first empty path

        _isAnalyzing = true;
        _pathCountdown = _countdownMax;
        _pathsCountdown = _countdownMax;
        _totalPathsCountdown = _totalPathsMax;

        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;

        PathAnalyzing(playerPos, endObject);        // Start path analyzing
    }

    private void PathAnalyzing(Vector3 playerPos, GameObject endObject)
    {
        if (_pathsCheckFinished) return;

        int i_count = _nodeArrayFinished.Count;
        for (int i = 0; i < i_count; i++)
        {
            if (_debug) Debug.Log("Analyzing node with index: " + i);
            GetPath(playerPos, endObject, i);
        }

        _pathsCountdown--;

        // Finish after counter is equals or less than 0. This should only happen if the path finding fails for all paths
        if (_pathsCountdown <= 0)
        {
            if (_debug) Debug.Log("Path finiding failed for all paths");
            _pathsCheckFinished = true;
        }
        else
        // Iterate trough all paths again
        {
            PathAnalyzing(playerPos, endObject);
        }
    }

    private void GetPath(Vector3 playerPos, GameObject endObject, int index)
    {
        GameObject checkObject = endObject;
        _nodeArrayFinished[index].Add(checkObject);
        if (_debug) Debug.Log("End node: " + checkObject);
        
        while (!_pathCheckFinished)
        {
            GameObject[] nodeConnectionsArray = checkObject.GetComponent<NodeInitialize>().nodeConnectionsArray;

            // Get the next node and create a new path if needed
            for (int i = 0; i < nodeConnectionsArray.Length; i++)
            {
                GameObject currentNode = nodeConnectionsArray[i];
                
                if (_nextNode == null)
                {
                    _nextNode = currentNode;
                }
                else
                {
                    if (_totalPathsCountdown > 0)
                    {
                        Debug.Log("New path created on a node: " + currentNode);
                        _nodeArrayFinished.Add(new List<GameObject>());         // Create a new path
                    }
                    _totalPathsCountdown--;
                }
            }
            
            if (_debug) Debug.Log("Next node to check: " + _nextNode);

            checkObject = _nextNode;

            _nodeArrayFinished[index].Add(checkObject);            // Add the next node the the finished node array

            // Found a path
            if (checkObject.GetComponent<NodeInitialize>().playerHere)
            {
                if (_debug) Debug.Log("Path finding succes for index: " + index);
                _pathCheckFinished = true;
                //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().PathCreate(_nodeArrayFinished);
            }

            _pathCountdown--;

            // Finish after counter is equals or less than 0. This should only happen if the path finding fails or if the path is to long
            if (_pathCountdown <= 0)
            {
                if (_debug) Debug.Log("Path finiding failed for index: " + index);
                _pathCheckFinished = true;
            }

            // Reset some variables after each node check
            _nextNode = null;
        }

        // Reset some variables after each path check
        _pathCountdown = _countdownMax;
        _pathCheckFinished = false;
    }
}
