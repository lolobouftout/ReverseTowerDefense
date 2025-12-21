using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float baseHealth = 100f;
    private float currentHealth;

    [Header("References (Assignées automatiquement)")]
    public UpgradeManager upgradeManager;
    public WaveManager waveManager;

    void Start()
    {
        // Applique l'upgrade de santé
        if (upgradeManager != null)
        {
            currentHealth = baseHealth * upgradeManager.GetHealthMultiplier();
        }
        else
        {
            currentHealth = baseHealth;
            Debug.LogWarning("UpgradeManager non assigné sur " + gameObject.name);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Feedback visuel : flash blanc
        StartCoroutine(FlashWhite());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Informe le WaveManager qu'un ennemi est mort
        if (waveManager != null)
        {
            waveManager.EnemyDestroyed();
        }

        // Ajoute des effets ici si tu veux (particles, son, etc.)
        Destroy(gameObject);
    }

    // Effet visuel de dégâts
    System.Collections.IEnumerator FlashWhite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            sr.color = originalColor;
        }
    }

    // Pour afficher la vie (optionnel)
    public float GetHealthPercentage()
    {
        float maxHealth = baseHealth;
        if (upgradeManager != null)
        {
            maxHealth = baseHealth * upgradeManager.GetHealthMultiplier();
        }
        return currentHealth / maxHealth;
    }
}