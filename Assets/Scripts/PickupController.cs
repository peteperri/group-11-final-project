using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [SerializeField] private bool rotates;
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;
    [SerializeField] private float zRotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rotates)
            transform.Rotate(new Vector3(xRotation, yRotation, zRotation) * Time.deltaTime);
    }
}
