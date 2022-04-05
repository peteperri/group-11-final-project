using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
        SceneManager.LoadScene("Future");
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
