using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private FuseBoxController[] fuseBoxes;
    [SerializeField] private float stopHeight;
    
    private void Update()
    {
        foreach(FuseBoxController fuseBox in fuseBoxes)
        {
            if (!fuseBox.Fixed)
                return;
        }
        Open();
    }

    private void Open()
    {
        if (transform.position.y < stopHeight)
            transform.position += new Vector3(0, 1f, 0) * Time.deltaTime;
    }
}