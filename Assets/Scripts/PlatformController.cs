using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    [SerializeField] private Color targetColor = new Color(255, 0, 0, 0); 
    [SerializeField] private Material materialToChange;
    [SerializeField] private float lifeTimeSeconds = 5;
    
    void Start()
    {
        materialToChange = gameObject.GetComponent<Renderer>().material;
        StartCoroutine(LerpFunction(targetColor, lifeTimeSeconds));
    }
    IEnumerator LerpFunction(Color endValue, float duration)
    {
        float time = 0;
        Color startValue = materialToChange.color;
        while (time < duration)
        {
            materialToChange.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        materialToChange.color = endValue;
        Destroy(gameObject);
    }
}
