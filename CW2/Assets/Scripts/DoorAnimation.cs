using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider fingerCollider;
    private bool _doorState;
    private bool _animationStarted;
    private Renderer _buttonRenderer;
    private static readonly int DoorOpen = Animator.StringToHash("doorOpen");

    // Start is called before the first frame update
    void Start()
    {
        _doorState = false;
        _buttonRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_animationStarted) return;
        if(other != fingerCollider) return;
        StartCoroutine(UseAnimation());

    }

    private IEnumerator UseAnimation()
    {
        _animationStarted = true;
        _buttonRenderer.material.EnableKeyword("_EMISSION");
        _doorState = !_doorState;
        animator.SetBool(DoorOpen, _doorState);
        yield return new WaitForSeconds(3);
        _animationStarted = false;
        _buttonRenderer.material.DisableKeyword("_EMISSION");
    }
}
