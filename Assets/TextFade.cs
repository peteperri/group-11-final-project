using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    private Text myText;
    [SerializeField] private float time = 4f;
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<Text>();
        StartCoroutine(FadeTextToZeroAlpha(time));
    }
    public IEnumerator FadeTextToZeroAlpha(float t)
    {
        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, 1);
        yield return new WaitForSeconds(t);
        while (myText.color.a > 0.0f)
        {
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, myText.color.a - (Time.deltaTime / t*3));
            yield return null;
        }
    }
    
}
