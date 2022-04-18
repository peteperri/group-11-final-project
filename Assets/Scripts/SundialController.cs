using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SundialController : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject clock;
    [SerializeField] private GameObject pedestal;

    private AudioSource _audioSource;
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
            pedestal.SetActive(false);
        }

        if (HubDoorController.futureComplete)
        {
            clock.SetActive(true);
        }
        else
        {
            clock.SetActive(false);
        }

        if (HubDoorController.presentComplete
            || HubDoorController.pastComplete
            || HubDoorController.futureComplete)
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.Play();
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
