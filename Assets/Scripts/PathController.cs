using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour {
    private GameObject _nextNode;
    public List<GameObject> _nodeArrayFinished = new List<GameObject>();
    private bool _pathCheckFinished = false;
    private float _nextNodeDistanceToPlayer;
    private int _counter = 10;

    [SerializeField] private bool _debug = false;

    private void Update()
    {
        GameObject hit = RaycastManager.GetRaycastHit();
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        if (RaycastManager.GetRaycastHit() != null && RaycastManager.GetRaycastHit().tag == "Path" && Input.GetMouseButtonDown(1))
        {
            GetPath(playerPos, hit);
        }
    }

    private void GetPath(Vector3 playerPos, GameObject endObject)
    {
        GameObject checkObject = endObject;
        _nodeArrayFinished.Add(checkObject);
        if (_debug) Debug.Log("End node: " + checkObject);

        while (!_pathCheckFinished)
        {
            GameObject[] nodeArray = checkObject.GetComponent<NodeInitialize>().nodes;

            for (int i = 0; i < nodeArray.Length; i++)
            {
                GameObject currentNode = nodeArray[i];
                float distanceToPlayer = Vector3.Distance(currentNode.transform.position, playerPos);       // Distance to the player from the current node

                if (_nextNode == null)          // If there is no next node, set current node as next node
                {
                    _nextNode = currentNode;
                    _nextNodeDistanceToPlayer = distanceToPlayer;
                }

                // Check if the current node position is smaller then the next node position
                if (distanceToPlayer < _nextNodeDistanceToPlayer)
                {
                    _nextNode = currentNode;
                    _nextNodeDistanceToPlayer = distanceToPlayer;
                }
            }

            if (_debug) Debug.Log("Next node to check: " + _nextNode);

            checkObject = _nextNode;

            _nodeArrayFinished.Add(checkObject);            // Add the next node the the finished node array

            if (checkObject.GetComponent<NodeInitialize>().playerHere) _pathCheckFinished = true;

            _counter--;
            if (_counter <= 0) _pathCheckFinished = true;
        }

        _pathCheckFinished = false;
    }
}
