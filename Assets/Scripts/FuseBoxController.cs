using UnityEngine;

public class FuseBoxController : MonoBehaviour
{
    [SerializeField] private GameObject fuse;
    public bool Fixed { private set; get; }
    
    private void OnTriggerEnter(Collider other)
    {
        
        
        if (other.CompareTag("Player") && !fuse.activeInHierarchy)
        {
            fuse.transform.localPosition = new Vector3(0, 0, 0.00034f);
            fuse.transform.localRotation = new Quaternion(0, 0, 0, 0);
            fuse.GetComponent<PickupController>().rotates = false;
            fuse.SetActive(true);
            Fixed = true;
            Debug.Log("fixed");
        }
    }
}
