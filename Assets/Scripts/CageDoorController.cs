using System.Runtime.CompilerServices;
using UnityEngine;

public class CageDoorController : MonoBehaviour
{
    [SerializeField] private FireEnemy[] enemies;
    private bool _opened;
    private float _currentDegrees;
    private const float DegreesPerSecond = 15;
    
    
    private void Update()
    {
        int deadCount = 0;
        foreach (FireEnemy enemy in enemies)
        {
            if(enemy.isDead)
                deadCount++;
        }

        if (deadCount >= enemies.Length && !_opened)
        {
            Open();
        }
    }

    private void Open()
    {
        transform.Rotate(0, DegreesPerSecond * Time.deltaTime, 0);
        _currentDegrees += DegreesPerSecond * Time.deltaTime;
        if (_currentDegrees >= 135)
        {
            _opened = true;
        }
    }
}
