using System.Collections;
using UnityEngine; //test script

public class EnemyWeapon : MonoBehaviour
{
    private readonly Enemy enemy;

    public GameObject projectile;
    public Transform firePoint;
    public Enemy firingDirection; 

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
    private readonly Enemy e;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePoint = transform.GetChild(0);
        firingDirection = Enemy.main;
    }

    public void Fire()
    {
        if (canFire && !reloading && clip > 0)
        {
            GameObject e = Instantiate(projectile, firePoint.position, firePoint.rotation); 
            e.GetComponent<Rigidbody>().AddForce(firingDirection.transform.forward * projVelocity); 
            Destroy(e, projLifespan); 
            canFire = false;
            clip--;
            StartCoroutine(nameof(CooldownFire));
        }
    }

    public void Reload()
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

        StartCoroutine(nameof(ReloadingCooldown));
    }

    IEnumerator CooldownFire()
    {
        yield return new WaitForSeconds(rof);

        if (clip > 0)
            canFire = true;
    }

    IEnumerator ReloadingCooldown()
    {
        yield return new WaitForSeconds(reloadCooldown);

        reloading = false;
        canFire = true;
    }
}