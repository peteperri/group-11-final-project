using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HA11HurtBoxController : MonoBehaviour
{
    private HA11Controller parent;

    private void Start()
    {
        parent = FindObjectOfType<HA11Controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            parent.Speed = 0;
            Destroy(other.gameObject);
            parent.State = "Dead";
            parent.Animation.Stop();
            Debug.Log("dead");
        }
    }
}
