using UnityEngine;
using UnityEngine.AI;
//using static NewMonoBehaviourScript;
public class Enemy : MonoBehaviour
{
    public bool isFollowing = false;

    public int health = 100;
    public int maxHealth = 100;

    public float detectionRange = 50;

    public PlayerController player;
    public NavMeshAgent agent;
    internal EnemyWeapon currentWeapon; // part of test
    internal object weaponSlot; // part of test

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

        isFollowing = targetDistance <= detectionRange;

        if (isFollowing)
        {
            agent.destination = player.transform.position;
        }
    }
    // TO DO: Make enemy take damage and give damage to player
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

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}