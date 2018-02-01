using UnityEngine;

public class NodeInitialize : MonoBehaviour {
    public GameObject[] nodeConnectionsArray;
    public bool playerHere = false;
    public bool isChecked = false;
    public int pathStartIndex = -1;
    public int pathConnectedIndex = -1;
    public GameObject pathConnectedObject = null;
}
