
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthController : MonoBehaviour
{
    private PlayerController _player;
    private RawImage _image;
    [SerializeField] private Texture threeHealth;
    [SerializeField] private Texture twoHealth;
    [SerializeField] private Texture oneHealth;
    [SerializeField] private Texture noHealth;
    
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _image = GetComponent<RawImage>();
    }
    private void Update()
    {
        switch (_player.getHealth())
        {
            case 3:
                _image.texture = threeHealth;
                break;
            case 2:
                _image.texture = twoHealth;
                break;
            case 1:
                _image.texture = oneHealth;
                break;
            default:
                _image.texture = noHealth;
                break;
        }
    }
}
