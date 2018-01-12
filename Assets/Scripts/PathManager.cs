using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {
    public List<Vector3> pathArray = new List<Vector3>();

    private Transform[] allPathChildren;

    [SerializeField] private Transform _debugPrefab;
    [SerializeField] private bool _debug = false;

	void Start () {
        allPathChildren = transform.GetComponentsInChildren<Transform>();
        CreatePathArray();
    }

    private void CreatePathArray()
    {
        foreach (Transform child in allPathChildren)
        {
            if (child == transform) continue;
            if (_debug) Debug.Log("Child position: " + child.gameObject.transform.position);
            pathArray.Add(child.gameObject.transform.position);
        }
    }

    private void OnDrawGizmos()
    {

        foreach (Vector3 child in pathArray)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(child, 0.1f);
        }       
    }
}
