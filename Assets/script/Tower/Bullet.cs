using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 8f;
    public float damage = 20f;

    [HideInInspector]
    public Transform target;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Auto-destruction apr�s 5 secondes (s�curit�)
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (target == null)
        {
            // Cible d�truite, continue en ligne droite
            rb.linearVelocity = transform.up * speed;
            return;
        }

        // Direction vers la cible
        Vector2 direction = (target.position - transform.position).normalized;

        // D�place le projectile
        rb.linearVelocity = direction * speed;

        // Rotation vers la cible
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si touche un ennemi
        if (other.CompareTag("Enemy"))
        {
            // Inflige des d�g�ts
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // D�truit le projectile
            Destroy(gameObject);
        }
    }
}