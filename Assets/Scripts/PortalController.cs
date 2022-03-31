using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private Transform otherPortal;
    [SerializeField] private float launchForce;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = otherPortal.position + new Vector3(0, 2.0f, 0);
            
            PlayerController player = other.GetComponent<PlayerController>();
            player.velocity.y = Mathf.Sqrt(launchForce * -2f * player.GravityForce);
        }
    }
}
