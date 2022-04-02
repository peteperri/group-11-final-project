using UnityEngine;

public class PickupController : MonoBehaviour
{
    public bool rotates;
    public float xRotation;
    public float yRotation;
    public float zRotation;
    
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
