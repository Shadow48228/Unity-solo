using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Combat Settings")]
    public float shootingRange = 100f;
    public float fireRate = 1f;
    public int maxAmmo = 100;
    public float reloadTime = 2f;

    [Header("Enemy Spawner Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 30f;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading = false;

    public bool isFollowing = false;

    public float detectionRange = 100;

    public NavMeshAgent agent;

    public int health = 500;
    public int maxHealth = 500;

    private Coroutine SpawnCoroutine;
    private bool isCoroutineRunning = false;

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


        if (targetDistance <= shootingRange)
        {
            if (!isCoroutineRunning)
            {
                SpawnCoroutine = StartCoroutine(SpawnEnemiesRoutine());
            }
        }
        else
        {
            if (isCoroutineRunning)
            {
                StopCoroutine(SpawnCoroutine);
                isCoroutineRunning = false;
            }
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        Vector3 spawnPos = transform.position + UnityEngine.Random.insideUnitSphere * 3f;
        spawnPos.y = transform.position.y;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            spawnPos = sp.position;
        }

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
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
            Destroy(gameObject.tag == "ArBullet"); // aa
        }

        if (collision.gameObject.tag == "PistolBullet")
        {
            health -= 13;
            Destroy(gameObject.tag == "PistolBullet"); // aa
        }

        if (collision.gameObject.tag == "BossBullet")
        {
            health -= 1;
            Destroy(gameObject.tag == "BossBullet"); // aa
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