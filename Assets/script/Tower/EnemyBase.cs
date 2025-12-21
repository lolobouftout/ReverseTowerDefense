using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    [Header("Base Settings")]
    public float maxHealth = 1000f;
    private float currentHealth;

    [Header("UI References")]
    public TMP_Text healthText;
    public GameObject victoryPanel;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        // Cache le panel de victoire au départ
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // Ne descend pas sous 0

        UpdateUI();

        // Flash rouge
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            BaseDestroyed();
        }
    }

    void BaseDestroyed()
    {
        Debug.Log("VICTOIRE ! La base ennemie est détruite !");

        // Affiche le panel de victoire
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        // Arrête le temps (optionnel)
        Time.timeScale = 0f;
    }

    void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = "Enemy Base HP: " + Mathf.RoundToInt(currentHealth) + " / " + maxHealth;

            // Change la couleur selon la vie restante
            float healthPercent = currentHealth / maxHealth;
            if (healthPercent > 0.5f)
                healthText.color = Color.green;
            else if (healthPercent > 0.2f)
                healthText.color = Color.yellow;
            else
                healthText.color = Color.red;
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            sr.color = originalColor;
        }
    }

    // Pour restart (appelé par un bouton)
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}