using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KillMii());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        if(other.CompareTag("SmashGolem")) return; 
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player")) return;
        if(other.collider.CompareTag("SmashGolem")) return;
        Destroy(gameObject);
    }
    
    private IEnumerator KillMii()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
