using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float JumpHeight = 6f;
    public float GravityForce = -9.8f;
    [SerializeField] private float speed = 6f;
    [SerializeField] private int health = 3;
    [SerializeField] private float sprintSpeedMultiplier = 2.0f;
    [SerializeField] private int extraJumpCount = 2;
    [SerializeField] private float launchForce = 1000;
    [SerializeField] private int platformAmmo = 0;
    [SerializeField] private bool canSpawnPlatforms = true;
    [SerializeField] private float turnSmoothTime = 0.1f;
    
    [SerializeField] private GameObject cineMachine;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text healthText;

    private CharacterController _controller;
    private Camera _camera;
    private float _turnSmoothVelocity;
    private InputHandler _playerActions;
    private CinemachineFreeLook _freeLook;
    private LayerMask _groundMask;
    private LayerMask _platMask;
    private int _currentJumps;
    private bool _hasSpawnedPlatform;
    private bool _canJump;
    private bool _isGrounded;
    bool _sprinting = false;
    
    public Vector3 velocity;
    
    
    private const float GroundDistance = 0.2f;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;
        
        _controller = GetComponent<CharacterController>();
        _freeLook = cineMachine.GetComponent<CinemachineFreeLook>();

        _playerActions = new InputHandler();
        _playerActions.Player.Enable();
        _groundMask = LayerMask.GetMask("Ground");
        _platMask = LayerMask.GetMask("Platform");

        healthText.text = "Health: " + health.ToString();
    }

    private void Update()
    {
        Move();
        Fall();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlatPickup"))
        {
            
            platformAmmo += 5;
            ammoText.text = "You can spawn platforms.\n" +
                            "Platform ammo: " + platformAmmo.ToString();
            canSpawnPlatforms = true;
            Destroy(other.gameObject);
        }
    }

    private void Move()
    {
        Vector2 moveInput = _playerActions.Player.Move.ReadValue<Vector2>();
        float horizontal = moveInput.x; //Input.GetAxisRaw("Horizontal"); //old input system way
        float vertical = moveInput.y; //Input.GetAxisRaw("Vertical"); //old input system way
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move( speed * Time.deltaTime * moveDirection.normalized);
        }
    }

    private void Fall()
    {
        Vector3 groundPos = groundCheck.position;
        
         _isGrounded = Physics.CheckSphere(groundPos, GroundDistance, _groundMask);
        _canJump = Physics.CheckSphere(groundPos, GroundDistance, _groundMask) ||
                   Physics.CheckSphere(groundPos, GroundDistance, _platMask);
        if (_canJump && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += GravityForce * Time.deltaTime;
            _controller.Move(velocity * Time.deltaTime);
        }

        if (_isGrounded || Physics.CheckSphere(groundPos, GroundDistance, _platMask))
        {
            if (_isGrounded)
            { 
                _hasSpawnedPlatform = false;  
            }
            _currentJumps = 0;
        }
        
    }

    public void ChangeHealth(int amount)
    {
        health += amount;
        healthText.text = "Health: " + health.ToString();
    }
    
    //inputActions method
    public void Look(InputAction.CallbackContext context)
    {
        _freeLook.m_XAxis.m_InputAxisValue = context.ReadValue<Vector2>().x;
        _freeLook.m_YAxis.m_InputAxisValue = context.ReadValue<Vector2>().y;
    }
    
    //inputActions method
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        
        if ( _canJump || _currentJumps < extraJumpCount)
        {
            _currentJumps++;
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * GravityForce);
        }
        else if(canSpawnPlatforms && !_hasSpawnedPlatform && platformAmmo > 0)
        {
            platformAmmo--;
            ammoText.text = "You can spawn platforms.\n" +
                            "Platform ammo: " + platformAmmo.ToString();
            _hasSpawnedPlatform = true;
            Vector3 pos = GetComponent<Transform>().position;
            Instantiate(platformPrefab, new Vector3(pos.x, pos.y - 2.0f, pos.z), Quaternion.identity);
            velocity.y = Mathf.Sqrt(0.5f * -2f * GravityForce);
        }
    }

    //inputActions method
    public void Sprint(InputAction.CallbackContext context)
    {
        
        if (context.performed && _canJump)
        {
            speed *= sprintSpeedMultiplier;
            _sprinting = true;
            Debug.Log("Initiated sprint. Speed: " + speed);
        }
        
        if (context.canceled && _sprinting)
        {
            _sprinting = false;
            speed /= sprintSpeedMultiplier;
            Debug.Log("Stopped sprint. Speed: " + speed);
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.canceled) return; 
        
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(this.transform.forward * launchForce);

    }
}
