using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Combat Settings")]
    public float shootingRange = 10f;
    public float fireRate = 0.5f;       
    public int maxAmmo = 10;            
    public float reloadTime = 2.0f;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading = false;

    public bool isFollowing = false;

    public float detectionRange = 50;

    public NavMeshAgent agent;

    public int health = 100;
    public int maxHealth = 100;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();

        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        transform.LookAt(player);

        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (distanceToPlayer <= shootingRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        float targetDistance = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

        isFollowing = targetDistance <= detectionRange;

        if (isFollowing)
        {
            agent.destination = player.transform.position;
        }
    }

    void Shoot()
    {
        currentAmmo--;

        GameObject ARBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = ARBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * 20f; 
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ARBullet")
        {
            health -= 16;
        }

        if (collision.gameObject.tag == "PistolBullet")
        {
            health -= 13;
        }

        if (collision.gameObject.tag == "BossBullet")
        {
            health -= 1;
        }

        if (collision.gameObject.tag == "CannonBullet")
        {
            health -= 20;
        }

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void Destroy(bool v) // aa
    {
        throw new NotImplementedException();
    }
}