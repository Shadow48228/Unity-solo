using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour

{
    public bool sprinting = false;
    public bool isAttacking = false;
    public bool hazardDamage = false;
    public bool canSprint = true;
    public bool toggleSprint = true;
    public bool sprintStop = false;
    public bool staminaStop = false;
    public bool regenStamina = false;

    public int health = 100;
    public int maxHealth = 100;

    public float speed = 10;
    public float jumpHeight = 2.5f;
    public float jumpDetectDistance = 1.1f;
    public float interactDistance = 10f;
    public float hazardCooldown = 3f;
    public float sprintBoost = 2.0f;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float sprintCost = .1f;
    public float sprintCooldown = 2;
    public float staminaRegen = 5;
    public float staminaCooldown = 2;

    PlayerInput playerInput;
    Rigidbody rb;
    Camera playerCam;

    public Weapon currentWeapon;
    public Transform weaponSlot;
    public GameObject pickupObj;

    Ray jumpRay;
    Ray interactRay;
    RaycastHit interactHit;
    Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        moveInput = Vector2.zero;

        playerCam = Camera.main;

        jumpRay = new Ray(transform.position, -transform.up);
        interactRay = new Ray(playerCam.transform.position, playerCam.transform.forward);

        weaponSlot = playerCam.transform.GetChild(0);

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

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.tag == "Weapon")
            {
                pickupObj = interactHit.collider.gameObject;
            }
            else
                pickupObj = null;
        }
        else
            pickupObj = null;

        if (currentWeapon)
            if (currentWeapon.holdToAttack && isAttacking)
                currentWeapon.fire();


        Vector3 tempMove = rb.linearVelocity;

        tempMove.x = (moveInput.x * speed);
        tempMove.z = (moveInput.y * speed);

        if (sprinting)
        {
            if (moveInput.y == 1)
            {
                tempMove.z = (moveInput.y += sprintBoost);

                stamina -= sprintCost * Time.deltaTime;

                if (stamina < 0)
                    stamina = 0;

                StopCoroutine("staminaReset");
            }
            else
            {
                canSprint = false;
                sprinting = false;

            }
        }

        if (!sprinting)
        {
            if (!regenStamina && !staminaStop && stamina < maxStamina)
            {
                StartCoroutine("staminaReset");
            }
            if (!canSprint && !sprintStop)
            {
                StartCoroutine("sprintReset");
            }
            if (regenStamina)
            {
                stamina += staminaRegen * Time.deltaTime;

                if (stamina >= maxStamina)
                {
                    stamina = maxStamina;
                    regenStamina = false;
                }
            }
        }

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
            if (!currentWeapon.reloading)
                currentWeapon.reload();

    }


    public void Attack(InputAction.CallbackContext context)
    {
        if (currentWeapon)
        {
            if (currentWeapon.holdToAttack)
            {
                if (context.ReadValueAsButton())
                    isAttacking = true;
                else
                    isAttacking = false;
            }

            else if (context.ReadValueAsButton())
                currentWeapon.fire();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (pickupObj)
            {
                if (pickupObj.tag == "Weapon")
                {
                    pickupObj.GetComponent<Weapon>().equip(this);
                }
            }
            else if (currentWeapon)
                Reload();
        }
    }

    public void DropWeapon()
    {
        if (currentWeapon)
            currentWeapon.unequip();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ammo")
        {
            if (currentWeapon && currentWeapon.ammo < currentWeapon.maxAmmo)
            {
                int ammoFill = currentWeapon.maxAmmo - currentWeapon.ammo;

                if (ammoFill < currentWeapon.ammoRefill)
                {
                    currentWeapon.ammo += ammoFill;
                }
                else
                {
                    currentWeapon.ammo += currentWeapon.ammoRefill;
                }

                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "Hazard")
        {
            if (hazardDamage)
                StartCoroutine("damageCooldown");
        }

        if (collision.gameObject.tag == "ARBullet")
        {
            health -= 16;
        }

        if (collision.gameObject.tag == "MinigunBullet")
        {
            health -= 5;
        }

        if (collision.gameObject.tag == "BossBullet")
        {
            health -= 25;
        }

        if (health <= 0f)
        {
            Destroy(gameObject);
        }

    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            if (hazardDamage)
            {
                StopCoroutine("damageCooldown");
                hazardDamage = false;
            }
        }
    }

    IEnumerator damageCooldown()
    {
        hazardDamage = true;

        yield return new WaitForSeconds(hazardCooldown);

        health--;
        hazardDamage = false;

    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (canSprint)
        {
            if (toggleSprint)
            {
                sprinting = !sprinting;
            }
            else if (!toggleSprint)
            {
                sprinting = context.ReadValueAsButton();

                if (!sprinting)
                    canSprint = false;
            }
        }
    }

    IEnumerator sprintReset()
    {
        sprintStop = true;

        yield return new WaitForSeconds(sprintCooldown);

        canSprint = true;
        sprintStop = false;
    }

    IEnumerator staminaReset()
    {
        staminaStop = true;

        yield return new WaitForSeconds(staminaCooldown);

        regenStamina = true;
        staminaStop = false;
    }
}