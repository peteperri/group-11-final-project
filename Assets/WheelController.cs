using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        
    }
}
