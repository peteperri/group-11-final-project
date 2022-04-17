using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private GameObject movingPlatform;
    private TargetHurtboxController _hurtboxController;
    private float _currentDegrees;
    private const float DegreesPerSecond = 45;
    private bool _done;
    void Start()
    {
        _hurtboxController = transform.GetChild(0).GetComponent<TargetHurtboxController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_hurtboxController.Shot)
        {
            movingPlatform.transform.position = Vector3.MoveTowards(
                movingPlatform.transform.position, 
                new Vector3(8.1f, 32.903f, 6.46f), 
                3 * Time.deltaTime);

            if (!_done)
            {
                transform.Rotate(0, -DegreesPerSecond * Time.deltaTime, 0);
                _currentDegrees += DegreesPerSecond * Time.deltaTime;
                if (_currentDegrees >= 90)
                {
                    _done = true;
                } 
            }

            
        }
    }
}
