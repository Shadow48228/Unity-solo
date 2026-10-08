using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private float damage = 25;

    void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        Destroy(gameObject, 30f);
    }
}
