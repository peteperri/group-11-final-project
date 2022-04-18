using System;
using System.Collections;
using UnityEngine;


public class SmashGolemController : FireEnemy
{
    [SerializeField] private Transform[] spots;
    [SerializeField] private float speed;
    [SerializeField] private Material cooledMaterialFace;
    [SerializeField] private Material cooledMaterialOuter;
    private AudioSource _audioSource;
    private float _startHeight;
    private int _currentSpot;
    
    public bool CanDamagePlayer { get; set; }
    public string State { get; set; } = "Patrolling";
    private PlayerController _player;
    private bool _soundPlayed;
    
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _startHeight = spots[0].transform.position.y;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (State == "Patrolling")
        {
            Patrol();
        }

        if (State == "Falling")
        {
            StartCoroutine(Fall());
        }

        if (State == "Dead")
        {
            isDead = true;
            BeDead();
        }
    }

    private void Patrol()
    {
        if (State == "Dead" || spots == null) return;
        if (_currentSpot == spots.Length - 1)
        {
            _currentSpot = -1;
        }
        transform.position = Vector3.MoveTowards(transform.position, spots[_currentSpot + 1].position, (speed/2) * Time.deltaTime);
        if (transform.position.Equals(spots[_currentSpot + 1].position))
        {
            _currentSpot++;
        }
    }

    private void BeDead()
    {
        if (!_soundPlayed)
        {
            _audioSource.Play();
            _soundPlayed = true;
        }

        State = "Dead";
        CanDamagePlayer = false;
        Material[] mats = gameObject.GetComponent<Renderer>().materials;
        mats[0] = cooledMaterialOuter;
        mats[1] = cooledMaterialFace;
        gameObject.GetComponent<Renderer>().materials = mats;
    }

    private IEnumerator Fall()
    {
        if (State != "Dead")
        {
            yield return new WaitForSeconds(2);
            CanDamagePlayer = true;
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, _player.transform.position.y, transform.position.z),
                speed * 2 * Time.deltaTime);
            yield return new WaitForSeconds(2);
            CanDamagePlayer = false;
            if (State != "Dead")
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x, _startHeight, transform.position.z),
                    speed * 2 * Time.deltaTime);
                yield return new WaitForSeconds(2f);
                if (State != "Dead")
                {
                    State = "Patrolling"; 
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && State == "Patrolling" && State != "Dead")
        {
            State = "Falling";
        }
    }
}
