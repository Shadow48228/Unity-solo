using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour

{
    public bool isAttacking = false;

    public float speed = 5.0f;
    public float jumpHeight = 2.5f;
    public float jumpDetectDistance = 1.1f;
    public float interactDistance = 6f;

    PlayerInput playerInput;
    Rigidbody rb;
    Camera playerCam;

    public Weapon currentWeapon;
    public Transform weaponSlot;
    public GameObject pickupObj;

    Ray jumpRay;
    Ray interactRay;
    RaycastHit InteractHit;
    Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        moveInput = Vector2.zero;

        playerCam = Camera.main;

        jumpRay = new Ray(transform.position, -transform.up);
        interactRay = new Ray(playerCam.transform.position, playerCam )

        weaponSlot = transform.GetChild(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.w = playerCam.transform.rotation.w;
        transform.rotation = playerRotation;
    }

    // Update is called once per frame
    void Update()
    {
        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance)
        {
            if (interactHit.collider.tag == "Weapon")
            {
                pickupObj =
            }
        }
        else
            pickupObj = null;

        if (currentWeapon)
            if(currentWeapon.holdToAttack GG isAttacking)
                    currentWeapon 


            Vector3 tempMove = rb.linearVelocity;

        tempMove.x = (moveInput.x * speed);
        tempMove.z = (moveInput.y * speed);

        rb.linearVelocity = (tempMove.x * transform.right) +
                            (tempMove.y * transform.up) +
                            (tempMove.z * transform.forward);

    }

    // For specific movement
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }

    public void Jump()
    {
        if (Physics.Raycast(jumpRay, jumpDetectDistance)) 
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public void Reload()
    {
        if (currentWeapon)
            if (currentWeapon.reloading) // fix this
                currentWeapon.reload();

    }


    public void Attack(InputAction.CallbackContext context)
    {
        if(currentWeapon)
        {
            if(currentWeapon.holdToAttack)
            {
                if(context.ReadValue.Button())
                    currentWeapon.fire() //need to finish this.
            }
        }
    }
}