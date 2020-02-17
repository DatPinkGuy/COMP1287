using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private List<IndexFinger> _indexFinger = new List<IndexFinger>();
    private List<Collider> _fingerColliders = new List<Collider>();
    private static bool _doorState;
    private bool _animationStarted;
    private Renderer _buttonRenderer;
    private static readonly int DoorOpen = Animator.StringToHash("doorOpen");

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
    }

    // Update is called once per frame
    void Update()
    {
       
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
        _doorState = !_doorState;
        animator.SetBool(DoorOpen, _doorState);
        yield return new WaitForSeconds(3);
        _animationStarted = false;
        _buttonRenderer.material.DisableKeyword("_EMISSION");
    }
}
