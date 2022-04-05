using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private FuseBoxController[] fuseBoxes;
    [SerializeField] private float stopHeight;
    [SerializeField] private bool devOpen;
    private AudioSource _audioSource;
    private bool _opened;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!devOpen)
        {
            foreach (FuseBoxController fuseBox in fuseBoxes)
            {
                if (!fuseBox.Fixed)
                    return;
            }
        }

        if (!_opened)
        {
            Open();
        }
        
    }

    private void Open()
    {
        if (!_opened && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        
        if (transform.position.y < stopHeight)
        {
            transform.position += new Vector3(0, 1f, 0) * Time.deltaTime;
        }
        else
        {
            _opened = true;
            _audioSource.Stop();
        }
    }
}