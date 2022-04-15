using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    private Image _barImage;
    
    void Start()
    {
        _barImage = GetComponent<Image>();
    }

    public void SetBarValue(float value)
    {
        _barImage.fillAmount = value / 10.0f;
        if(_barImage.fillAmount < 0.2f)
        {
            _barImage.color = Color.red;
        }
        else if(_barImage.fillAmount < 0.4f)
        {
            _barImage.color = Color.yellow;
        }
        else
        {
            _barImage.color = Color.green;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
