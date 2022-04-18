using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Toggle InfiniteHealthToggle;
    public Toggle InfiniteAmmoToggle;
    public Toggle InfiniteStaminaToggle;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void ReturnToMainMenu ()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void StartGame ()
    {
        HubDoorController.pastComplete = false;
        HubDoorController.presentComplete = false;
        HubDoorController.futureComplete = false;
        SceneManager.LoadScene("Hub");
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(PlayerController.CurrentScene);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void SetInfiniteHealth()
    {
        PlayerController.InfiniteHealth = InfiniteHealthToggle.isOn;
    }
    
    public void SetInfiniteAmmo()
    {
        PlayerController.InfiniteAmmo = InfiniteAmmoToggle.isOn;
    }
    
    public void SetInfiniteStamina()
    {
        PlayerController.InfiniteStamina = InfiniteStaminaToggle.isOn;
    }
}
