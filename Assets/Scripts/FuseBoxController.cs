using UnityEngine;

public class FuseBoxController : MonoBehaviour
{
    [SerializeField] private GameObject fuse;
    [SerializeField] private GameObject icon;
    [SerializeField] private ParticleSystem sparks;
    private AudioSource _audioSource;
    public bool Fixed { private set; get; }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !fuse.activeInHierarchy)
        {
            fuse.transform.localPosition = new Vector3(0, 0, 0.00034f);
            fuse.transform.localRotation = new Quaternion(0, 0, 0, 0);
            fuse.GetComponent<PickupController>().rotates = false;
            fuse.SetActive(true);
            Fixed = true;
            icon.SetActive(true);
            _audioSource.Play();
            sparks.Stop();
            Debug.Log("fixed");
        }
    }
}
