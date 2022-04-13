using UnityEngine;

public class SmashGolemHurtboxController : MonoBehaviour
{
    private SmashGolemController _parent;
    private void Start()
    {
        GameObject parentGameObject = transform.parent.gameObject;
        _parent = parentGameObject.GetComponent<SmashGolemController>();
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Water"))
        {
            _parent.State = "Dead";
        }
    }
}
