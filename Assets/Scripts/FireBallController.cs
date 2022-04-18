using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireBallController : FireEnemy
{
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private Material deadMaterial;
    private int _currentSpot;
    private string _state = "Patrolling";
    private bool _died = false;
    private PlayerController _player;
    private bool _canHurtPlayer = true;
    private float _animStartTime;
    private Renderer _renderer;
    private Rigidbody _rb;
    private AudioSource _audioSource;
    private Animation _animation;
    //private AudioSource _audioSource;

    private void Awake()
    {
        _animStartTime = Random.Range(0.1f, 3f);
        StartCoroutine(WaitToStartAnimation());
        _player = FindObjectOfType<PlayerController>();
        _animation = GetComponent<Animation>();
        _animation.Stop();
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();

        //_audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_state == "Dead")
        {
            _canHurtPlayer = false;
            if (!_died)
            {
                transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
                _animation.Stop();
                _died = true;
                _renderer.material = deadMaterial;
                _rb.isKinematic = false;
                transform.position += new Vector3(0, 3, 0);
                transform.localScale = new Vector3(1f, 1f, 1f);
                _audioSource.clip = deathSound;
                _audioSource.loop = false;
                _audioSource.volume += 1.0f;
                _audioSource.Play();
                _rb.AddForce(Random.Range(50f, 100f), Random.Range(50f, 1000f), Random.Range(50f, 1000f));
                StartCoroutine(WaitAndKillMe());
                //_audioSource.clip = deathSound;
                //_audioSource.Play();
            }
            
        }
        else
        {
            transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, transform.rotation.w);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_state == "Dead") return;

        if (other.CompareTag("Water"))
        {
            Destroy(other.gameObject);
            _state = "Dead";
            isDead = true;

        }
        
        if (other.gameObject.CompareTag("Player") && _canHurtPlayer)
        {
            _player.ChangeHealth(-1);
            _canHurtPlayer = false;
            StartCoroutine(WaitForSeconds());
        }
    }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1);
        _canHurtPlayer = true;
    }

    private IEnumerator WaitToStartAnimation()
    {
        yield return new WaitForSeconds(_animStartTime);
        _animation.Play();
    }

    private IEnumerator WaitAndKillMe()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
