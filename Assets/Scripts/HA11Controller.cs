using System;
using System.Collections;
using UnityEngine;

public class HA11Controller : MonoBehaviour
{
    [SerializeField] private Transform[] spots;
    [SerializeField] private float speed;
    private int _currentSpot;
    private string _state = "Patrolling";
    private bool _died = false;
    private PlayerController _player;
    private bool _canHurtPlayer = true;
    private Animation _animation;
    [SerializeField] private AudioSource walkingSounds;
    [SerializeField] private AudioSource alertSound;
    [SerializeField] private AudioClip died;
    
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _animation = GetComponent<Animation>();
        StartCoroutine(PlayWalkSounds());
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
                alertSound.clip = died;
                alertSound.Play();
                transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
                _died = true;
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
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z), speed/1.5f * Time.deltaTime);
        transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_state == "Dead") return;
        
        if (other.CompareTag("Player"))
        {
            _state = "Chasing";
            alertSound.Play();
        }
        
        if (other.CompareTag("Water") && GetComponent<Collider>().GetType() == typeof(CapsuleCollider))
        {
            speed = 0;
            Destroy(other.gameObject);
            _state = "Dead";
            _animation.Stop();
            Debug.Log("dead");
        }
    }

    private void OnCollisionEnter(Collision other)
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
        yield return new WaitForSeconds(2);
        _canHurtPlayer = true;
    }
    
    private IEnumerator PlayWalkSounds()
    {
        while (true)
        {
            if (_state.Equals("Patrolling") || _state.Equals("Chasing"))
            {
                walkingSounds.Play();
                yield return new WaitForSeconds(0.49f);
            }
            yield return null;
        }
        
    }
}
