using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    public Collider ObjectCollider => GetComponent<Collider>();
    public Transform ParentTransform => transform.parent;
    public Transform ThisTransform => gameObject.transform;
    public bool built = false;
    public float neededAmount = 100f;
    public float currentAmount = 0f;
    

    private void Update()
    {
        if (built == true) return;
    }

    private void OnCollisionStay(Collision other)
    {
        
    }
}
