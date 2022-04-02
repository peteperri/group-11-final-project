using UnityEngine;

public class FOVController : MonoBehaviour
{
    public bool PlayerSeen { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        SetSeen(true, other);
        Debug.Log("Trigger Entered");
    }

    private void OnTriggerExit(Collider other)
    {
        SetSeen(false, other);
        Debug.Log("Trigger Exited");
    }

    private void SetSeen(bool enter, Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerSeen = enter;
        } 
    }
}
