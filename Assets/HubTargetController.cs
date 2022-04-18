using UnityEngine;

public class HubTargetController : MonoBehaviour
{
    [SerializeField] private Material successMat;

    private Renderer _renderer;
    private AudioSource _audioSource;
    private bool _green;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && !_green)
        {
            _renderer.material = successMat;
            _audioSource.Play();
            _green = true;
        }
    }
}
