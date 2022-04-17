using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    [SerializeField] private bool isPast;
    [SerializeField] private bool isPresent;
    [SerializeField] private bool isFuture;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPast)
            {
                HubDoorController.pastComplete = true;
            }

            if (isPresent)
            {
                HubDoorController.presentComplete = true;
            }
            
            if (isFuture)
            {
                HubDoorController.futureComplete = true;
            }
            
            SceneManager.LoadScene("Hub");
        }
    }
}
