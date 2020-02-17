using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _doorState;
    private static readonly int DoorOpen = Animator.StringToHash("doorOpen");

    // Start is called before the first frame update
    void Start()
    {
        //_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool(DoorOpen, _doorState);
        _doorState = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        _doorState = !_doorState;
    }
}
