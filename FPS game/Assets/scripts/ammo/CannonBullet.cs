using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public float damage = 20f;

    void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}