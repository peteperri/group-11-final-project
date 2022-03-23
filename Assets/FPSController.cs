using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 12f;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float sprintSpeedMultiplier = 1.2f;
    
    public bool HasRedKey { get; private set; }
    public bool HasBlueKey { get; private set; }
    public  bool HasYellowKey { get; private set; }

    public bool ShortFOV { get; private set; }

    private Camera _camera;
    private CharacterController _controller;
    private GameObject _pauseMenu;

    private float _xRotation;
    private float _yVelocity;
    
    private bool _isSprinting;
    private bool _isCrouching;
    private bool _gamePaused;

    private float _xMove;
    private float _zMove;
    
    
    
    private void Start()
    {
        _camera = Camera.main;
        _controller = GetComponent<CharacterController>();
        ///_pauseMenu = GameObject.FindWithTag("PauseMenu");
        ///_pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        Walk();
        Sprint();
        Jump();
        Crouch();
        MouseLook();
        ///PauseUnpause();
        EscToClose();
        //RayCast();
        SetFOV();
    }

    private void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp (_xRotation, -90, 90);
        
        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
        
    }

    private void Walk()
    {
        
        _xMove = Input.GetAxis ("Horizontal");
        _zMove = Input.GetAxis ("Vertical");

        if (_isSprinting)
        {
            _xMove *= sprintSpeedMultiplier;
            _zMove *= sprintSpeedMultiplier;
        }
        
        Vector3 move = transform.right * _xMove + transform.forward * _zMove;
        _controller.Move(movementSpeed * Time.deltaTime * new Vector3(move.x, _yVelocity, move.z));
        _yVelocity -= gravity * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _controller.isGrounded)
        {
            _yVelocity = jumpForce;
            Debug.Log("Space Pressed");
        }
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _controller.isGrounded && !_isCrouching && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            _isSprinting = true;
        }
        else
        {
            _isSprinting = false;
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _camera.transform.position -= new Vector3(0f, 0.3f, 0f);
            _controller.height -= 1.5f;
            _isCrouching = true;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _camera.transform.position += new Vector3(0f, 0.3f, 0f);
            _controller.height += 1.5f;
            _isCrouching = false;
        }
    }

    ///private void PauseUnpause()
    ///{
    ///    if (Input.GetKeyDown(KeyCode.P))
    ///   {
    ///        if (!_gamePaused)
    ///        {
    ///            //pause game
    ///            _pauseMenu.SetActive(true);
    ///            _gamePaused = true;
    ///           Time.timeScale = 0;
    ///        }
    ///        else
    ///        {
    ///            //unpause game
    ///            _pauseMenu.SetActive(false);
    ///            _gamePaused = false;
    ///            Time.timeScale = 1;
    ///        }
    ///    }
    ///}

    private void EscToClose()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /*private void RayCast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) //out keyword passes by reference instead of by value
            {
                Transform selection = hit.transform;
                if (selection.CompareTag("BlueKey") || selection.CompareTag("RedKey") || selection.CompareTag("YellowKey"))
                {
                    Destroy(selection.gameObject);
                    switch (selection.tag)
                    {
                        case "BlueKey":
                            HasBlueKey = true;
                            break;
                        case "RedKey":
                            HasRedKey = true;
                            break;
                        case "YellowKey":
                            HasYellowKey = true;
                            break;
                    }

                }
            }
        }
    }*/

    private void SetFOV()
    {
        if (_xMove == 0 && _zMove == 0 || _isCrouching)
        {
            ShortFOV = true;
        }
        else
        {
            ShortFOV = false;
        }
    }
}
