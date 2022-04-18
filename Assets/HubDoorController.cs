using UnityEngine;
using UnityEngine.SceneManagement;

public class HubDoorController : MonoBehaviour
{

    [SerializeField] private bool isPastDoor;
    [SerializeField] private bool isPresentDoor;
    [SerializeField] private bool isFutureDoor;
    public static bool pastComplete;
    public static bool presentComplete;
    public static bool futureComplete;

    private void Awake()
    {
        Debug.Log($"Past Complete: {pastComplete} Present Complete: {presentComplete} Future Complete: {futureComplete}");
        if (pastComplete && isPastDoor)
        {
            Destroy(gameObject);
        }
        if (presentComplete && isPresentDoor)
        {
            Destroy(gameObject);
        }
        if (futureComplete && isFutureDoor)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPastDoor && !pastComplete) 
            {
                SceneManager.LoadScene("Past");
            }
            
            if (isPresentDoor && !presentComplete)
            {
                SceneManager.LoadScene("Present");
            }
            
            if (isFutureDoor && !futureComplete)
            {
                SceneManager.LoadScene("Future");
            }
        }
    }
}
