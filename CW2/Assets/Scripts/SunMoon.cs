using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoon : MonoBehaviour
{
    private MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    [SerializeField] private Material[] materials;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
            //cycle == Cycle.Day ? Cycle.Night : Cycle.Day;
    }

    public void ChangeToSun()
    {
        MeshRenderer.material = materials[0];
    }

    public void ChangeToMoon()
    {
        MeshRenderer.material = materials[1];
    }
}
