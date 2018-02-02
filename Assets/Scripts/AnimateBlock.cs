using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBlock : MonoBehaviour {

    [SerializeField] private float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;
    [SerializeField] private GameObject _blockInTower;
    [SerializeField] private GameObject _toBlockInTower;
    [SerializeField] private GameObject _blockUp;
    private Vector3 targetPosition;
    private Vector3 targetPositionUp;
    private bool triggered = false;

    void Update()
    {
        if(triggered)
        {
            targetPosition = new Vector3(_toBlockInTower.transform.position.x, _blockInTower.transform.position.y, _blockInTower.transform.position.z);
            _blockInTower.transform.position = Vector3.SmoothDamp(_blockInTower.transform.position, targetPosition, ref velocity, smoothTime);

            targetPositionUp = new Vector3(_blockUp.transform.position.x, _toBlockInTower.transform.position.y - 5.74f, _blockUp.transform.position.z);
            _blockUp.transform.position = Vector3.SmoothDamp(_blockUp.transform.position, targetPositionUp, ref velocity2, smoothTime);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            triggered = true;
        }
    }
}
