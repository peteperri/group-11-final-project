using System;
using System.Collections;
using UnityEngine;

public class SmallRobotController : MonoBehaviour
{
    [SerializeField] private Transform[] spots;
    [SerializeField] private float speed;
    private int _currentSpot;
    private string _state = "Patrolling";
    private PlayerController _player;
    private bool _canHurtPlayer = true;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
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

        transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, transform.rotation.w);
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
        if (other.CompareTag("Player"))
        {
            _state = "Chasing";
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

    private void OnTriggerExit(Collider other)
    {
        //StartCoroutine(WaitForSeconds());
    }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(2);
        _canHurtPlayer = true;
    }
}
