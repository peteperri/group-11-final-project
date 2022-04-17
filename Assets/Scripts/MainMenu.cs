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

    private void Update() //delete me
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene("Past");
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("Present");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene("Art Test");
        }
    }
    
    public void ReturnToMainMenu ()
    {
        SceneManager.LoadScene("Hub");
    }
    
    public void StartGame ()
    {
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
