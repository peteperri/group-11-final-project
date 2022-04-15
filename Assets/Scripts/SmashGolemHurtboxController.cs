using UnityEngine;

public class SmashGolemHurtboxController : MonoBehaviour
{
    private SmashGolemController _parent;
    private PlayerController _player;
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        GameObject parentGameObject = transform.parent.gameObject;
        _parent = parentGameObject.GetComponent<SmashGolemController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            _parent.State = "Dead";
        }
        
        if (other.gameObject.CompareTag("Player") && _parent.CanDamagePlayer && _parent.State != "Dead")
        {
            _player.ChangeHealth(-3);
        }
    }
}
