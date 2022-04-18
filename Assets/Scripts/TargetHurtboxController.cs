using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHurtboxController : MonoBehaviour
{
    public bool Shot { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("Hurtbox Shot");
            Shot = true;
        }
    }
}
