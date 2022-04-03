using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
    [SerializeField] private int waterAmmo = 5;
    [SerializeField] private bool canSpawnPlatforms = true;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Animator animator;
    
    [SerializeField] private GameObject cineMachine;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [FormerlySerializedAs("ammoText")] [SerializeField] private Text platformPickupAmmoText;
    [SerializeField] private Text waterAmmoText;

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
    private bool _sprinting = false;
    
    public Vector3 velocity;
    
    
    private const float GroundDistance = 0.05f;
    
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
        
        platformPickupAmmoText.text = "";
        waterAmmoText.text = waterAmmo.ToString();
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
            platformPickupAmmoText.text = "You can spawn platforms.\n" +
                            "Platform ammo: " + platformAmmo.ToString();
            canSpawnPlatforms = true;
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.CompareTag("WaterPickup"))
        {
            waterAmmo += 5;
            waterAmmoText.text = waterAmmo.ToString();
            Destroy(other.transform.parent.gameObject);
        }

        if (other.gameObject.CompareTag("Fuse"))
        {
            Debug.Log("Collided with Fuse");
            other.gameObject.SetActive(false);
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
            animator.SetBool("IsWalking", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move( speed * Time.deltaTime * moveDirection.normalized);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsSprinting", false);
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
            animator.SetBool("IsJumping", false);
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
    }
    
    //inputActions method
    public void Look(InputAction.CallbackContext context)
    {
        _freeLook.m_XAxis.m_InputAxisValue = context.ReadValue<Vector2>().x * 0.05f;
        _freeLook.m_YAxis.m_InputAxisValue = context.ReadValue<Vector2>().y * 0.05f;
    }
    
    //inputActions method
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        
        if ( _canJump || _currentJumps < extraJumpCount)
        {
            animator.SetBool("IsJumping", true);
            _currentJumps++;
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * GravityForce);
        }
        else if(canSpawnPlatforms && !_hasSpawnedPlatform && platformAmmo > 0)
        {
            platformAmmo--;
            platformPickupAmmoText.text = "You can spawn platforms.\n" +
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
            animator.SetBool("IsSprinting", true);
            speed *= sprintSpeedMultiplier;
            _sprinting = true;
            //Debug.Log("Initiated sprint. Speed: " + speed);
        }
        
        if (context.canceled && _sprinting)
        {
            animator.SetBool("IsSprinting", false);
            _sprinting = false;
            speed /= sprintSpeedMultiplier;
            //Debug.Log("Stopped sprint. Speed: " + speed);
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (!context.started || waterAmmo <= 0) return;

        waterAmmo--;
        waterAmmoText.text = waterAmmo.ToString();
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(this.transform.forward * launchForce);

    }

    public int getHealth()
    {
        return health;
    }
}
