using System;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public float rotationSpeed;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip wheelSound;
    [SerializeField] private AudioClip wheelSoundFast;
    [SerializeField] private SmallRobotController parent;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);

        switch (rotationSpeed)
        {
            case 0:
                _audioSource.Pause();
                break;
            case 45:
                _audioSource.clip = wheelSound;
                if (!_audioSource.isPlaying) _audioSource.Play();
                break;
            case 90:
                _audioSource.clip = wheelSoundFast;
                if (!_audioSource.isPlaying) _audioSource.Play();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            parent.Speed = 0;
            Destroy(other.gameObject);
            parent.State = "Dead";
            Debug.Log("dead");
        }
    }
}
