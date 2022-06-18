using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 5f;

    private Transform cameraTransform;
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction grappleAction;


    // Grapple Gun fields
    [SerializeField] private LayerMask grappleLayerMask;
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform shootPoint;
    private SpringJoint springJoint;

    private Vector3 grapplePoint;
    private float grappleDistance = 100f;

    private CharacterController playerController;
    private Rigidbody rigidbody;


    private void Awake()
    {
        cameraTransform = Camera.main.transform;

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        grappleAction = playerInput.actions["Grapple"];

        Cursor.lockState = CursorLockMode.Locked;



        lineRenderer = GetComponent<LineRenderer>();
        playerController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        grappleAction.performed += _ => StartGrapple();
        grappleAction.canceled += _ => StopGrapple();
    }
    private void StartGrapple()
    {
        Debug.Log("Grapple Started");
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, grappleDistance, grappleLayerMask))
        {
            playerController.enabled = false;
            if (rigidbody == null)
                rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.mass = 10f;

            Debug.Log("Grapple hit " + hit.collider.name);
            grapplePoint = hit.point;
            springJoint = gameObject.AddComponent<SpringJoint>();

            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            // distance grapple will try to keep from grapple point
            springJoint.maxDistance = distanceFromPoint * 0.8f;
            springJoint.minDistance = distanceFromPoint * 0.25f;

            // spring values, change to fit feel of game
            springJoint.spring = 4.5f * 2;
            springJoint.damper = 7f * 2;
            springJoint.massScale = 4.5f;
        }
    }
    private void StopGrapple()
    {
        Destroy(springJoint);
        Destroy(rigidbody);
        playerController.enabled = true;
        Debug.Log("Grapple Stopped");
    }

    private void DrawRope()
    {
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }


    private void OnDisable()
    {
        grappleAction.performed -= _ => StartGrapple();
        grappleAction.canceled -= _ => StopGrapple();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }



        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Moves player relative to camera direction
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate towards camera direction
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        DrawRope();
    }
}
