using UnityEngine;

public class PickupController : MonoBehaviour
{
    [SerializeField] private bool rotates;
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;
    [SerializeField] private float zRotation;
    
    void Update()
    {
        if(rotates)
            transform.Rotate(new Vector3(xRotation, yRotation, zRotation) * Time.deltaTime);

        if (transform.childCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
