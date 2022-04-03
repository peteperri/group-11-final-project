using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RobotController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] spots;
    [SerializeField] private FOVController crouchStillFOV;
    [SerializeField] private FOVController movingFOV;
    private PlayerController _player;
    private int _currentSpot;
    private string _state = "Patrolling";
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _anim = GetComponent<Animator>();
        _anim.SetBool("IsWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_state.Equals("Patrolling"))
        {
            Patrol();
        }
        Look();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //load death scene
            //Debug.Log("You died!");
        }
    }

    private void Patrol()
    {
        if (_currentSpot == spots.Length - 1)
        {
            _currentSpot = -1;
        }
        transform.position = Vector3.MoveTowards(transform.position, spots[_currentSpot + 1].position, speed * Time.deltaTime);
        transform.LookAt(spots[_currentSpot + 1].position);
        
        if (transform.position.Equals(spots[_currentSpot + 1].position))
        {
            _currentSpot++;
        }
    }

    private void Look()
    {
    }
}
