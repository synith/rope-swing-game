using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    //private Rigidbody rb;
    //private float speed = 5f;

    //private Transform cameraRootTransform;
    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    cameraRootTransform = transform.Find("cameraRoot");
    //}
    //private void Update()
    //{
    //    Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    //    moveDirection = moveDirection.normalized;
    //    rb.AddForce(moveDirection * speed * rb.mass);

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        rb.AddForce(Vector3.up * speed * rb.mass * 50f, ForceMode.Impulse);
    //    }


    //    Vector3 angles = cameraRootTransform.localEulerAngles;
    //    angles.z = 0;

    //    float angle = cameraRootTransform.localEulerAngles.x;

    //    if (angle > 180 && angle < 340)
    //    {
    //        angles.x = 340;
    //    }
    //    else if (angle < 180 && angle > 40)
    //    {
    //        angles.x = 40;
    //    }

    //    cameraRootTransform.localEulerAngles = angles;
    //}
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction aimAction;
    private InputAction grappleAction;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        aimAction = playerInput.actions["Aim"];
        grappleAction = playerInput.actions["Grapple"];
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
