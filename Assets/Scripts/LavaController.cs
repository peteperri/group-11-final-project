using System;
using System.Collections;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] private Material cooled;

    private PlayerController _player;
    private Renderer _renderer;
    private bool _damagingPlayer;
    private bool _hot = true;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_player == null)
            _damagingPlayer = false;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollsionEnter Triggered");
        if(other.gameObject.CompareTag("Player") && _hot)
        {
            _damagingPlayer = true;
            StartCoroutine(DamagePlayer());
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _damagingPlayer = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            _hot = false;
            _renderer.material = cooled;
            Destroy(other.gameObject);
        }
    }

    private IEnumerator DamagePlayer()
    {
        while (_damagingPlayer)
        {
            _player.ChangeHealth(-damageAmount);
            yield return new WaitForSeconds(1);
        }
    }
}
