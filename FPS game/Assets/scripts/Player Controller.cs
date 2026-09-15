using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour

{
    public float speed = 5.0f;
    public float jumpHeight = 2.5f;
    public float jumpDetectDistance = 1.1f;

    PlayerInput playerInput;
    Rigidbody rb;
    Camera playerCam;

    Ray jumpRay;
    Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        moveInput = Vector2.zero;

        playerCam = Camera.main;

        jumpRay = new Ray(transform.position, -transform.up);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        Vector3 tempMove = rb.linearVelocity;

        tempMove.x = (moveInput.x * speed) * transform.right.x;
        tempMove.z = (moveInput.y * speed) * transform.forward.z;

        rb.linearVelocity = tempMove;

    }

    // For specific movement
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }

    public void Jump()
    {
        if (Physics.Raycast(jumpRay, jumpDetectDistance)) ;
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    }

}