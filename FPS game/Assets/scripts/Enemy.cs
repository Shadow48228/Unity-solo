using Unity.Hierarchy;
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
          //health -= 13health;
        }


        if (collision.gameObject.tag == "PistolBullet")
        {

        }
    }

  /*public class HealthController : MonoBehaviour, IDamageable
    {
        public float health = 100f;

        public void TakeDamage(float damageAmount)
        {
            health -= damageAmount;

            if (health <= 0f)
            {
                Die();
            }
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }*/
}