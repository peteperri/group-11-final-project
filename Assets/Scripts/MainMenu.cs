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
    public Toggle InfinitePlatformToggle;
    public Toggle InvertYToggle;
    public Toggle InvertXToggle;
    
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
        Debug.Log("StartGame");
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
        Debug.Log("SetInfHealth");
        PlayerController.InfiniteHealth = InfiniteHealthToggle.isOn;
    }
    
    public void SetInfiniteAmmo()
    {
        Debug.Log("SetInfAmmo");
        PlayerController.InfiniteAmmo = InfiniteAmmoToggle.isOn;
    }
    
    public void SetInfinitePlatAmmo()
    {
        Debug.Log("SetInfPlat");
        PlayerController.InfinitePlatformAmmo = InfinitePlatformToggle.isOn;
    }
    
    public void SetInfiniteStamina()
    {
        Debug.Log("SetInfStam");
        PlayerController.InfiniteStamina = InfiniteStaminaToggle.isOn;
    }
    
    public void SetInverseXAxis()
    {
        Debug.Log("SetInverseXAxis");
        PlayerController.InverseX = InvertXToggle.isOn;
    }
    
    public void SetInverseYAxis()
    {
        Debug.Log("SetInverseYAxis");
        PlayerController.InverseY = InvertYToggle.isOn;
    }
}
