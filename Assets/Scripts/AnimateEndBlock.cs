using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateEndBlock : MonoBehaviour {


    [SerializeField] private float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;
    [SerializeField] private GameObject _bridge;
    [SerializeField] private GameObject _toBridge;
    [SerializeField] private GameObject _tower;
    private Vector3 targetPosition;
    private Quaternion target;
    private float damping = 1f;
    private bool triggered;

    void Update()
    {
        if(triggered)
        {
            targetPosition = new Vector3(_bridge.transform.position.x, _toBridge.transform.position.y, _bridge.transform.position.z);
            _bridge.transform.position = Vector3.SmoothDamp(_bridge.transform.position, targetPosition, ref velocity, smoothTime);

            var desiredRotQ = Quaternion.Euler(_tower.transform.eulerAngles.x, 180, _tower.transform.eulerAngles.z);
            _tower.transform.rotation = Quaternion.Lerp(_tower.transform.rotation, desiredRotQ, Time.deltaTime * damping);
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
