using UnityEngine;
using System.Collections.Generic;

public class TowerDefense : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float baseDamage = 20f;
    public float baseFireRate = 1f; // Tirs par seconde

    private float currentDamage;
    private float currentFireRate;
    private float nextFireTime = 0f;

    [Header("Detection")]
    public float range = 3f;
    private List<Transform> enemiesInRange = new List<Transform>();
    private Transform currentTarget;

    [Header("References")]
    public WaveManager waveManager;

    void Start()
    {
        // S'améliore avec les vagues
        UpdateStats();

        // Si pas de firePoint, crée-en un
        if (firePoint == null)
        {
            GameObject fp = new GameObject("FirePoint");
            fp.transform.parent = transform;
            fp.transform.localPosition = Vector3.zero;
            firePoint = fp.transform;
        }
    }

    void Update()
    {
        // Mise à jour de la cible
        UpdateTarget();

        // Vise la cible
        if (currentTarget != null)
        {
            LookAtTarget();

            // Tire si le cooldown est passé
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / currentFireRate;
            }
        }
    }

    public void UpdateStats()
    {
        // Les tours deviennent plus fortes avec les vagues
        int wave = 1;

        if (waveManager != null)
        {
            wave = waveManager.currentWave;
            if (wave < 1) wave = 1; // Minimum vague 1
        }

        currentDamage = baseDamage * (1 + (wave - 1) * 0.15f); // +15% par vague
        currentFireRate = baseFireRate * (1 + (wave - 1) * 0.1f); // +10% cadence par vague
    }

    void UpdateTarget()
    {
        // Nettoie la liste des ennemis détruits
        enemiesInRange.RemoveAll(e => e == null);

        // Trouve l'ennemi le plus proche
        if (enemiesInRange.Count > 0)
        {
            currentTarget = GetClosestEnemy();
        }
        else
        {
            currentTarget = null;
        }
    }

    Transform GetClosestEnemy()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(transform.position, enemy.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    void LookAtTarget()
    {
        if (currentTarget == null) return;

        // Rotation vers la cible (2D)
        Vector2 direction = currentTarget.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet Prefab non assigné sur " + gameObject.name);
            return;
        }

        // Crée le projectile
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.damage = currentDamage;
            bulletScript.target = currentTarget;
        }
    }

    // Détection des ennemis qui entrent dans la zone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!enemiesInRange.Contains(other.transform))
            {
                enemiesInRange.Add(other.transform);
            }
        }
    }

    // Détection des ennemis qui sortent de la zone
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
        }
    }

    // Pour visualiser la portée dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}