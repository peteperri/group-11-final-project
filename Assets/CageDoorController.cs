using UnityEngine;

public class CageDoorController : MonoBehaviour
{
    [SerializeField] private SmashGolemController[] enemies;
    private bool _opened;
    private float _currentDegrees;
    private const float DegreesPerSecond = 15;
    
    
    private void Update()
    {
        foreach (SmashGolemController enemy in enemies)
        {
            if (!enemy.State.Equals("Dead") || _opened)
            {
                return;
            }
            transform.Rotate(0, DegreesPerSecond * Time.deltaTime, 0);
            _currentDegrees += DegreesPerSecond * Time.deltaTime;
            if (_currentDegrees >= 135)
            {
                _opened = true;
            }
        }
    }
}
