using System.Collections;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _launching;
    private int _launchCount;
    private PlayerController _player;

    private Vector3 _startPosition;
    [SerializeField] private float launchForce = 10;

    private void Start()
    {
        _launchCount = 0;
        _startPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        Jump();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            _player.ChangeHealth(-3);
        }
    }

    private void Jump()
    {
        if (transform.position.y < _startPosition.y && _launchCount <= 0)
        {
            Debug.Log("Launching");
            _rb.AddForce(0, launchForce, 0);
            _launchCount++;
            StartCoroutine(WaitForSeconds());
        }
        
    }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1);
        _launchCount = 0;
    }
}
