using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour {
    private GameObject _nextNode;
    private GameObject _hit;
    private List<List<GameObject>> _nodeArrayFinished = new List<List<GameObject>>();
    private List<int> _pathSuccesIndexArray = new List<int>();
    private List<int> _pathFailedIndexArray = new List<int>();
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
            _hit = hit;
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
            _isAnalyzing = false;
        }
        else if (!_pathsCheckFinished)
        // Iterate trough all paths again
        {
            PathAnalyzing(playerPos, endObject);
        }
    }

    private void GetPath(Vector3 playerPos, GameObject endObject, int index)
    {
        if (IsSucceeded(index)) { PathReset(); return; }        // Return if this path already succeeded to find a player
        if (IsFailed(index)) { PathReset(); return; }        // Return if this path already succeeded to find a player

        // Get the path start index if exists
        GameObject checkObject = GetPathStartIndex(index);
        if (checkObject == null) checkObject = endObject;

        _nodeArrayFinished[index].Add(checkObject);         // Add the end node to the array
        checkObject.GetComponent<NodeInitialize>().isChecked = true;        // Set the isChecked bool to true in the end node so it won't be checked later
        if (_debug) Debug.Log("End node: " + checkObject);
        
        while (!_pathCheckFinished)
        {
            GameObject[] nodeConnectionsArray = checkObject.GetComponent<NodeInitialize>().nodeConnectionsArray;
            int nodeConnectionsArrayLength = nodeConnectionsArray.Length;
            int nodeConnectionsArrayCheckedNumber = 0;
            if (_debug) Debug.Log("The number of nodes to check: " + nodeConnectionsArray.Length);

            // Get the next node and create a new path if needed
            for (int i = 0; i < nodeConnectionsArrayLength; i++)
            {
                GameObject currentNode = nodeConnectionsArray[i];
                NodeInitialize currentNodeInit = currentNode.GetComponent<NodeInitialize>();
                if (_debug) Debug.Log("The current node to check: " + currentNode);

                // Keep track of the nodes that are checked already
                if (currentNodeInit.isChecked) nodeConnectionsArrayCheckedNumber++;

                if (_nextNode == null && !currentNodeInit.isChecked)
                {
                    _nextNode = currentNode;
                    currentNodeInit.isChecked = true;
                }
                else if (_nextNode != null && !currentNodeInit.isChecked)
                {
                    if (_totalPathsCountdown > 0)
                    {
                        if (_debug) Debug.Log("New path created on a node: " + currentNode);
                        _nodeArrayFinished.Add(new List<GameObject>());         // Create new path
                        currentNodeInit.pathStartIndex = _nodeArrayFinished.Count-1;      // Give the current path index to the current node
                        currentNodeInit.pathConnectedIndex = index;
                        currentNodeInit.pathConnectedObject = _nodeArrayFinished[index][_nodeArrayFinished[index].Count-1];
                        if (_debug) Debug.Log("Given the node: " + currentNode + " a pathStartIndex: " + currentNodeInit.pathStartIndex);
                        if (_debug) Debug.Log("Given the node: " + currentNode + " a pathConnectedIndex: " + currentNodeInit.pathConnectedIndex);
                        if (_debug) Debug.Log("Given the node: " + currentNode + " a pathConnectedObject: " + currentNodeInit.pathConnectedObject);
                    }
                    _totalPathsCountdown--;
                }
                else if (currentNodeInit.isChecked && i == nodeConnectionsArrayLength - 1 && nodeConnectionsArrayCheckedNumber == nodeConnectionsArrayLength)
                {
                    if (_debug) Debug.Log("Path finding failed for index: " + index);
                    _pathFailedIndexArray.Add(index);       // Add the index of the current array to an array that keeps track of all paths that failed
                    PathReset();
                    return;
                }
            }
            
            if (_debug) Debug.Log("Next node to check: " + _nextNode);

            if (_nextNode != null)
            {
                checkObject = _nextNode;
            }
            else
            {
                PathReset();
                return;
            }

            _nodeArrayFinished[index].Add(checkObject);            // Add the next node to the finished node array

            // Found a path
            if (checkObject.GetComponent<NodeInitialize>().playerHere)
            {
                if (_debug) Debug.Log("Path finding succes for index: " + index);
                _pathCheckFinished = true;
                _pathsCheckFinished = true;
                _pathSuccesIndexArray.Add(index);       // Add the index of the current array to an array that keeps track of all paths that succeed

                int currentNodePathConnectedIndex = _nodeArrayFinished[_pathSuccesIndexArray[0]][0].GetComponent<NodeInitialize>().pathConnectedIndex;
                if (_debug) Debug.Log("Current node connected index: " + currentNodePathConnectedIndex);
                if (currentNodePathConnectedIndex != -1)
                {
                    //int currentNode = _nodeArrayFinished[_pathSuccesIndexArray[0]][0].GetComponent<NodeInitialize>().pathStartIndex;
                    GameObject pathConnectedObject = _nodeArrayFinished[_pathSuccesIndexArray[0]][0].GetComponent<NodeInitialize>().pathConnectedObject;
                    bool pathConnectedObjectChecked = false;

                    List<GameObject> pathRemovedArray = new List<GameObject>();
                    for (int i = 0; i < _nodeArrayFinished[_pathSuccesIndexArray[0]].Count; i++)
                    {
                        pathRemovedArray.Add(_nodeArrayFinished[_pathSuccesIndexArray[0]][i]);
                        if (_debug) Debug.Log("Add a node to the path removed array: " + _nodeArrayFinished[_pathSuccesIndexArray[0]][i]);
                    }

                    _nodeArrayFinished[_pathSuccesIndexArray[0]].Clear();

                    for (int i = 0; i < _nodeArrayFinished[currentNodePathConnectedIndex].Count - 1; i++)
                    {
                        // Check for the raycast hit object
                        {
                            if (_nodeArrayFinished[currentNodePathConnectedIndex][i] == _hit)
                                pathConnectedObjectChecked = true;
                        }

                        if (pathConnectedObjectChecked)
                        // Add the connected path to the finished node array
                        {
                            _nodeArrayFinished[_pathSuccesIndexArray[0]].Add(_nodeArrayFinished[currentNodePathConnectedIndex][i]);
                            if (_debug) Debug.Log("Added a connected node: " + _nodeArrayFinished[currentNodePathConnectedIndex][i]);
                        }

                        // Check for the path connected object
                        {
                            if (_nodeArrayFinished[currentNodePathConnectedIndex][i] == pathConnectedObject)
                                pathConnectedObjectChecked = false;
                        }
                    }

                    for (int i = 0; i < pathRemovedArray.Count; i++)
                    {
                        _nodeArrayFinished[_pathSuccesIndexArray[0]].Add(pathRemovedArray[i]);
                        if (_debug) Debug.Log("Added a node from the path removed array back into the finished node array: " + pathRemovedArray[i]);
                    }
                }
                // Give the path array to the player
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().PathCreate(_nodeArrayFinished[_pathSuccesIndexArray[0]]);
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
        PathReset();
    }

    private bool IsSucceeded(int index)
    {
        for (int i = 0; i < _pathSuccesIndexArray.Count; i++)
        {
            if (_pathSuccesIndexArray[i] == index)
            {
                if (_debug) Debug.Log("The current path with index: " + index + " already succeeded to find a player");
                return true;
            }
        }
        return false;
    }

    private bool IsFailed(int index)
    {
        for (int i = 0; i < _pathFailedIndexArray.Count; i++)
        {
            if (_pathFailedIndexArray[i] == index)
            {
                if (_debug) Debug.Log("The current path with index: " + index + " failed to find a player");
                return true;
            }
        }
        return false;
    }

    private void PathReset()
    {
        _pathCountdown = _countdownMax;
        _pathCheckFinished = false;
    }

    private GameObject GetPathStartIndex(int index)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Path");

        for (int i = 0; i < objectsWithTag.Length; i++)
        {
            NodeInitialize currentNodeInit = objectsWithTag[i].GetComponent<NodeInitialize>();
            if (currentNodeInit.pathStartIndex == index) return objectsWithTag[i];
        }

        return null;
    }
}
