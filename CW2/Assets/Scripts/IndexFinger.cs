using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexFinger : MonoBehaviour
{
    [SerializeField] private Transform indexToFollow;

    // Update is called once per frame
    void Update()
    {
        transform.position = indexToFollow.position;
    }
}
