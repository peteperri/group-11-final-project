using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KillMii());
    }

    private IEnumerator KillMii()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
