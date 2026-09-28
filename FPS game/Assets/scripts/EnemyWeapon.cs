using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{/*
    PlayerController player;

    public GameObject projectile;
    public Transform firePoint;
    public Camera firingDirection; //change

    [Header("Meta Attributes")]
    public bool canFire = true;
    public bool holdToAttack = true; // fix
    public bool reloading = false;

    [Header("Weapon Stats")]
    public float projLifespan;
    public float projVelocity;
    public float reloadCooldown;
    public float rof;
    public int clip;
    public int clipSize;

    [Header("Ammo Stats")]
    public int ammo;
    public int maxAmmo;
    public int ammoRefill;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePoint = transform.GetChild(0);
        firingDirection = Camera.main; // fix
    }

    public void equip(PlayerController p) // fix
    {
        player = p;

        player.currentWeapon = this; //fix

        transform.SetPositionAndRotation(player.weaponSlot.position, player.weaponSlot.rotation); // fix
        transform.SetParent(player.weaponSlot); //fix

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    public void unequip()
    {
        player.currentWeapon = null; //fix

        transform.SetParent(null);

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().isTrigger = false;

        this.player = null; //fix
    }

    public void fire()
    {
        if (canFire && !reloading && clip > 0)
        {
            GameObject p = Instantiate(projectile, firePoint.position, firePoint.rotation); //fix
            p.GetComponent<Rigidbody>().AddForce(firingDirection.transform.forward * projVelocity); //fix
            Destroy(p, projLifespan); //fix
            canFire = false;
            clip--;
            StartCoroutine("cooldownFire");
        }
    }

    public void reload()
    {
        if (clip >= clipSize)
            return;

        reloading = true;
        canFire = false;

        int reloadCount = clipSize - clip;

        if (ammo < reloadCount)
        {
            clip += ammo;
            ammo = 0;
        }

        else
        {
            clip += reloadCount;
            ammo -= reloadCount;
        }

        StartCoroutine("reloadingCooldown");
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(rof);

        if (clip > 0)
            canFire = true;
    }

    IEnumerator reloadingCooldown()
    {
        yield return new WaitForSeconds(reloadCooldown);

        reloading = false;
        canFire = true;
    }*/
}