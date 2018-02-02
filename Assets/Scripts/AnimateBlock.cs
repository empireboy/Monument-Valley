using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBlock : MonoBehaviour {

    private Animator _animation;
    [SerializeField] private GameObject _animationObject;


    void Start()
    {
        _animation = _animationObject.GetComponent<Animator>();
    }
    void Update()
    {
        _animation.Play("CINEMA_4D_Main");
    }
    void OnColliderEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            //_animation.Play("CINEMA_4D_Main");
            Debug.Log("Moan");
        }
    }
}
