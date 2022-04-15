using System;
using System.Collections;
using UnityEngine;

public class SmallRobotController : MonoBehaviour
{
    [SerializeField] private Transform[] spots;
    [SerializeField] private float speed;
    [SerializeField] private WheelController wheel;
    [SerializeField] private AudioClip deathSound;
    private int _currentSpot;
    private string _state = "Patrolling";
    private bool _died = false;
    private PlayerController _player;
    private bool _canHurtPlayer = true;
    private AudioSource _audioSource;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_state == "Patrolling")
        {
            Patrol();
        }

        if (_state == "Chasing")
        {
            Chase();
        }

        if (_state == "Dead")
        {
            _canHurtPlayer = false;
            if (!_died)
            {
                transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
                wheel.rotationSpeed = 0.0f;
                _died = true;
                _audioSource.clip = deathSound;
                _audioSource.Play();
            }
            
        }
        else
        {
            transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, transform.rotation.w);
        }
        
    }

    private void Patrol()
    {
        if (_currentSpot == spots.Length - 1)
        {
            _currentSpot = -1;
        }
        transform.position = Vector3.MoveTowards(transform.position, spots[_currentSpot + 1].position, (speed/2) * Time.deltaTime);
        transform.LookAt(new Vector3(spots[_currentSpot + 1].position.x, spots[_currentSpot + 1].position.y, spots[_currentSpot + 1].position.z));
        
        if (transform.position.Equals(spots[_currentSpot + 1].position))
        {
            _currentSpot++;
        }
    }

    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z), speed * 2 * Time.deltaTime);
        transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_state == "Dead") return;
        
        if (other.CompareTag("Player") && _state == "Patrolling")
        {
            wheel.rotationSpeed = 90;
            _audioSource.Play();
            _state = "Chasing";
        }
        
        if (other.CompareTag("Water"))
        {
            speed = 0;
            Destroy(other.gameObject);
            _state = "Dead";
            Debug.Log("dead");
        }
        
    }

    private void OnCollisionStay(Collision other)
    {
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
}
