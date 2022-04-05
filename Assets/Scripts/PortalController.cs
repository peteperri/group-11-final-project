using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private Transform otherPortal;
    [SerializeField] private float launchForce;
    private AudioSource _audioSource;

    private void Update()
    {
        transform.Rotate(-45.0f * Time.deltaTime, 0.0f, 0.0f);
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = otherPortal.position + new Vector3(0, 2.5f, 0);
            _audioSource.Play();
            PlayerController player = other.GetComponent<PlayerController>();
            player.velocity.y = Mathf.Sqrt(launchForce * -2f * player.GravityForce);
        }
    }
}
