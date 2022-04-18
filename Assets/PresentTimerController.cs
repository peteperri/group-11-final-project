using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PresentTimerController : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 90;
    [SerializeField] private Text timeText;
    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            SceneManager.LoadScene("LoseMenu");
        }
        
    }
    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}