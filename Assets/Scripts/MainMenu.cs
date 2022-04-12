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
