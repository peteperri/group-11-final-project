using UnityEngine;

public class WheelController : MonoBehaviour
{
    public float rotationSpeed;
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        
    }
}
