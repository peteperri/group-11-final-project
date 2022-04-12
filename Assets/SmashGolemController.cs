using System;
using System.Collections;
using UnityEngine;


public class SmashGolemController : MonoBehaviour
{
    [SerializeField] private Transform[] spots;
    [SerializeField] private float speed;
    [SerializeField] private Material cooledMaterialFace;
    [SerializeField] private Material cooledMaterialOuter;
    private float _startHeight;
    private bool _canDamagePlayer = false;
    private int _currentSpot;
    public string State { get; set; } = "Patrolling";
    private PlayerController _player;
    
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _startHeight = spots[0].transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(State);
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
            BeDead();
        }
    }

    private void Patrol()
    {
        if (State == "Dead") return;
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
        State = "Dead";
        _canDamagePlayer = false;
        Material[] mats = gameObject.GetComponent<Renderer>().materials;
        mats[0] = cooledMaterialFace;
        mats[1] = cooledMaterialOuter;
        gameObject.GetComponent<Renderer>().materials = mats;
    }

    private IEnumerator Fall()
    {
        if (State != "Dead")
        {
            yield return new WaitForSeconds(2);
            _canDamagePlayer = true;
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, _player.transform.position.y, transform.position.z),
                speed * 2 * Time.deltaTime);
            yield return new WaitForSeconds(2);
            _canDamagePlayer = false;
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && _canDamagePlayer && State != "Dead")
        {
            _player.ChangeHealth(-3);
        }
    }
}
