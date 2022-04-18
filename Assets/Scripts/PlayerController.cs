using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float JumpHeight = 6f;
    public float GravityForce = -9.8f;
    [SerializeField] private float speed = 6f;
    [SerializeField] private int health = 3;
    [SerializeField] private float sprintSpeedMultiplier = 2.0f;
    [SerializeField] private float maxStamina = 10;
    [SerializeField] private int extraJumpCount = 2;
    [SerializeField] private float launchForce = 1000;
    [SerializeField] private int platformAmmo = 0;
    [SerializeField] private int waterAmmo = 5;
    [SerializeField] private bool canSpawnPlatforms = true;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject cineMachine;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip healthSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip gotWater;
    [SerializeField] private AudioClip gotFuse;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private Text platformPickupAmmoText;
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
    private bool _paused = false;
    private float _currentStamina;
    private AudioSource _playerSounds;
    private StaminaBarController _staminaBar;
    private float _coyoteTimeCounter;

    public Vector3 velocity;
    public float GroundDistance = 0.075f;

    public static string CurrentScene;

    public static bool InfiniteHealth;
    public static bool InfiniteStamina;
    public static bool InfiniteAmmo;

    private bool isMoving;

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
        _currentStamina = maxStamina;
        _staminaBar = FindObjectOfType<StaminaBarController>();

        if (platformPickupAmmoText != null)
        {
            platformPickupAmmoText.text = "";
        }
        
        if (pauseMenu != null)
        {
            waterAmmoText.text = waterAmmo.ToString();
        }
        _playerSounds = GetComponent<AudioSource>();
    }

    private void Awake()
    {
         CurrentScene = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        Move();
        Fall();

        if (health <= 0)
        {
            SceneManager.LoadScene("LoseMenu");
        }

        if ((!_sprinting || !isMoving) && _currentStamina < maxStamina)
        {
            _currentStamina += 1.0f * Time.deltaTime;
        }

        if (_sprinting && !InfiniteStamina && isMoving)
        {
            _currentStamina -= 1.0f * Time.deltaTime;
            if (_currentStamina <= 0)
            {
                animator.SetBool("IsSprinting", false);
                _sprinting = false;
                speed /= sprintSpeedMultiplier;
            }
        }

        if (_staminaBar != null)
        {
            _staminaBar.SetBarValue(_currentStamina);
        }

        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (_canJump || _isGrounded)
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlatPickup"))
        {
            GameObject pickupText = platformPickupAmmoText.gameObject;
            pickupText.SetActive(true);
            platformAmmo += 10;
            _playerSounds.clip = healthSound;
            _playerSounds.Play();
            if (platformPickupAmmoText != null)
            {
                platformPickupAmmoText.text = platformAmmo.ToString();
            }
            canSpawnPlatforms = true;
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.CompareTag("WaterPickup"))
        {
            _playerSounds.clip = gotWater;
            _playerSounds.Play();
            waterAmmo += 10;
            if (waterAmmoText != null)
            {
                waterAmmoText.text = waterAmmo.ToString();
            } 
            Destroy(other.transform.parent.gameObject);
        }

        if (other.gameObject.CompareTag("Fuse"))
        {
            _playerSounds.clip = gotFuse;
            _playerSounds.Play();
            Debug.Log("Collided with Fuse");
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("HealthPickup") && health < 3)
        {
            _playerSounds.clip = healthSound;
            _playerSounds.Play();
            other.gameObject.SetActive(false);
            ChangeHealth(1);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("SmashGolem"))
        {
            ChangeHealth(-3);
        }
    }

    private void Move()
    {
        Vector2 moveInput = _playerActions.Player.Move.ReadValue<Vector2>();
        float horizontal = moveInput.x; //Input.GetAxisRaw("Horizontal"); //old input system way
        float vertical = moveInput.y; //Input.GetAxisRaw("Vertical"); //old input system way

        if (horizontal != 0 || vertical != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

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
        _canJump = Physics.CheckSphere(groundPos, GroundDistance, _groundMask) || Physics.CheckSphere(groundPos, GroundDistance, _platMask);
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
            StartCoroutine(WaitAndResetCurrentJumps());
        }
        
    }

    public void ChangeHealth(int amount)
    {
        if (InfiniteHealth) return;
        
        if (amount > 0)
        {
            _playerSounds.clip = healthSound;
            _playerSounds.Play();
        }
        else
        {
            _playerSounds.clip = hurtSound;
            _playerSounds.Play();
        }
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
        if (context.started)
        {
            Debug.Log("Jump Button Pressed");
            if (_canJump || _coyoteTimeCounter > 0f || _currentJumps < extraJumpCount)
            {
                animator.SetBool("IsJumping", true);
                _currentJumps++;
                Debug.Log("Adding to _currentJumps");
                velocity.y = Mathf.Sqrt(JumpHeight * -2f * GravityForce);
                _playerSounds.clip = jumpSound;
                _playerSounds.Play();
            }
            else if((canSpawnPlatforms && !_hasSpawnedPlatform && platformAmmo > 0) || (InfiniteAmmo))
            {
                platformAmmo--;
                if (platformPickupAmmoText != null)
                {
                    platformPickupAmmoText.text = platformAmmo.ToString();
                }
                _hasSpawnedPlatform = true;
                Vector3 pos = GetComponent<Transform>().position;
                Instantiate(platformPrefab, new Vector3(pos.x, pos.y - 2.0f, pos.z), Quaternion.identity);
                velocity.y = Mathf.Sqrt(0.5f * -2f * GravityForce);
            }
        }

        if (context.canceled)
        {
            _coyoteTimeCounter = 0f;
        }

        
    }

    //inputActions method
    public void Sprint(InputAction.CallbackContext context)
    {
        
        if (context.performed && _canJump && _currentStamina >= 0)
        {
            animator.SetBool("IsSprinting", true);
            speed *= sprintSpeedMultiplier;
            _sprinting = true;
            Debug.Log(_currentStamina + "/" + maxStamina);
        }
        
        if (context.canceled && _sprinting)
        {
            animator.SetBool("IsSprinting", false);
            _sprinting = false;
            speed /= sprintSpeedMultiplier;
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (waterAmmo > 0 || InfiniteAmmo)
        {

            _playerSounds.clip = shootSound;
            _playerSounds.Play();
            if (!InfiniteAmmo)
            {
                waterAmmo--;
            }
            if (waterAmmoText != null)
            {
                waterAmmoText.text = waterAmmo.ToString();
            }

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddRelativeForce(this.transform.forward * launchForce);
        }

    }

    public void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("pause key pressed");
        if(pauseMenu == null) return; 
        if (!context.started) return;
        if (_paused)
        {
            pauseMenu.SetActive(false);
            _paused = false;
            Time.timeScale = 1;
        }
        else
        {
            pauseMenu.SetActive(true);
            _paused = true;
            Time.timeScale = 0;
        }
    }

    public int getHealth()
    {
        return health;
    }

    private IEnumerator WaitAndResetCurrentJumps()
    {
        yield return new WaitForSeconds(0.15f);
        _currentJumps = 0;
    }
}
