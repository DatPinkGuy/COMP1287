using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private readonly List<IndexFinger> _indexFinger = new List<IndexFinger>();
    private readonly List<Collider> _fingerColliders = new List<Collider>();
    private static bool _doorState;
    private bool _animationStarted;
    private Renderer _buttonRenderer;
    private static readonly int DoorOpen = Animator.StringToHash("doorOpen");
    private AudioSource _buttonClick;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _doorState = false;
        _buttonRenderer = GetComponent<Renderer>();
        _indexFinger.AddRange(FindObjectsOfType<IndexFinger>());
        foreach (var finger in _indexFinger)
        {
            _fingerColliders.Add(finger.GetComponent<Collider>());
        }
        _buttonClick = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_animationStarted) return;
        foreach (var finger in _fingerColliders)
        {
            if(other != finger) continue;
            StartCoroutine(UseAnimation());
        }
    }

    private IEnumerator UseAnimation()
    {
        _animationStarted = true;
        _buttonRenderer.material.EnableKeyword("_EMISSION");
        if(_buttonClick) _buttonClick.Play();
        yield return new WaitForSeconds(0.5f);
        if(audioSource) audioSource.Play();
        _doorState = !_doorState;
        animator.SetBool(DoorOpen, _doorState);
        yield return new WaitForSeconds(3);
        _animationStarted = false;
        _buttonRenderer.material.DisableKeyword("_EMISSION");
    }
}
