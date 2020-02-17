using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider fingerCollider;
    private bool _doorState;
    private static readonly int DoorOpen = Animator.StringToHash("doorOpen");

    // Start is called before the first frame update
    void Start()
    {
        _doorState = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != fingerCollider) return;
        _doorState = !_doorState;
        animator.SetBool(DoorOpen, _doorState);
    }
}
