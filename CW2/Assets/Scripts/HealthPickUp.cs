using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public float healthAmount;

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
