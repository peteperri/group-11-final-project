using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SundialController : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject clock;
    [SerializeField] private GameObject pedestal;
    // Start is called before the first frame update
    private void Awake()
    {
        if (HubDoorController.presentComplete)
        {
            arrow.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }

        if (HubDoorController.pastComplete)
        {
            pedestal.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }

        if (HubDoorController.futureComplete)
        {
            clock.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") 
           && HubDoorController.presentComplete 
           && HubDoorController.pastComplete 
           && HubDoorController.futureComplete)
        {
            SceneManager.LoadScene("WinMenu");
        }
    }
}
