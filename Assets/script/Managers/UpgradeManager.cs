using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Points")]
    public int upgradePoints = 5; // Points de départ

    [Header("Upgrade Levels")]
    public int damageLevel = 1;
    public int speedLevel = 1;
    public int healthLevel = 1;
    public int spawnSpeedLevel = 1;
    public int enemyCountLevel = 1;

    [Header("Upgrade Costs")]
    public int damageCost = 1;
    public int speedCost = 1;
    public int healthCost = 1;
    public int spawnSpeedCost = 2;
    public int enemyCountCost = 3;

    [Header("Upgrade Multipliers")]
    public float damagePerLevel = 0.25f; // +25% par niveau
    public float speedPerLevel = 0.2f;   // +20% par niveau
    public float healthPerLevel = 0.3f;  // +30% par niveau
    public float spawnSpeedPerLevel = 0.15f; // -15% délai par niveau
    public float enemyCountPerLevel = 0.2f; // +20% ennemis par niveau

    [Header("UI References")]
    public TMP_Text pointsText;
    public TMP_Text damageText;
    public TMP_Text speedText;
    public TMP_Text healthText;
    public TMP_Text spawnSpeedText;
    public TMP_Text enemyCountText;
    public TMP_Text feedbackText;

    [Header("Buttons")]
    public Button damageButton;
    public Button speedButton;
    public Button healthButton;
    public Button spawnSpeedButton;
    public Button enemyCountButton;

    void Start()
    {
        // Configure les boutons
        if (damageButton != null)
            damageButton.onClick.AddListener(UpgradeDamage);

        if (speedButton != null)
            speedButton.onClick.AddListener(UpgradeSpeed);

        if (healthButton != null)
            healthButton.onClick.AddListener(UpgradeHealth);

        if (spawnSpeedButton != null)
            spawnSpeedButton.onClick.AddListener(UpgradeSpawnSpeed);

        if (enemyCountButton != null)
            enemyCountButton.onClick.AddListener(UpgradeEnemyCount);

        UpdateUI();
    }

    public void AddUpgradePoints(int waveNumber)
    {
        if (waveNumber == 1)
        {
            upgradePoints += 5;
        }
        else
        {
            upgradePoints += 5 + (2 * (waveNumber - 1));
        }

        ShowFeedback("+" + (waveNumber == 1 ? 5 : 5 + (2 * (waveNumber - 1))) + " Upgrade Points!");
        UpdateUI();
    }

    public void UpgradeDamage()
    {
        if (upgradePoints >= damageCost)
        {
            upgradePoints -= damageCost;
            damageLevel++;
            ShowFeedback("DAMAGE UPGRADED! Level " + damageLevel);
            UpdateUI();
        }
        else
        {
            ShowFeedback("Not enough points! Need " + damageCost);
        }
    }

    public void UpgradeSpeed()
    {
        if (upgradePoints >= speedCost)
        {
            upgradePoints -= speedCost;
            speedLevel++;
            ShowFeedback("SPEED UPGRADED! Level " + speedLevel);
            UpdateUI();
        }
        else
        {
            ShowFeedback("Not enough points! Need " + speedCost);
        }
    }

    public void UpgradeHealth()
    {
        if (upgradePoints >= healthCost)
        {
            upgradePoints -= healthCost;
            healthLevel++;
            ShowFeedback("HEALTH UPGRADED! Level " + healthLevel);
            UpdateUI();
        }
        else
        {
            ShowFeedback("Not enough points! Need " + healthCost);
        }
    }

    public void UpgradeSpawnSpeed()
    {
        if (upgradePoints >= spawnSpeedCost)
        {
            upgradePoints -= spawnSpeedCost;
            spawnSpeedLevel++;
            ShowFeedback("SPAWN SPEED UPGRADED! Level " + spawnSpeedLevel);
            UpdateUI();
        }
        else
        {
            ShowFeedback("Not enough points! Need " + spawnSpeedCost);
        }
    }

    public void UpgradeEnemyCount()
    {
        if (upgradePoints >= enemyCountCost)
        {
            upgradePoints -= enemyCountCost;
            enemyCountLevel++;
            ShowFeedback("ENEMY COUNT UPGRADED! Level " + enemyCountLevel);
            UpdateUI();
        }
        else
        {
            ShowFeedback("Not enough points! Need " + enemyCountCost);
        }
    }

    public float GetDamageMultiplier()
    {
        return 1f + (damageLevel - 1) * damagePerLevel;
    }

    public float GetSpeedMultiplier()
    {
        return 1f + (speedLevel - 1) * speedPerLevel;
    }

    public float GetHealthMultiplier()
    {
        return 1f + (healthLevel - 1) * healthPerLevel;
    }

    public float GetSpawnSpeedMultiplier()
    {
        // Réduit le délai entre spawns
        return 1f - (spawnSpeedLevel - 1) * spawnSpeedPerLevel;
    }

    public float GetEnemyCountMultiplier()
    {
        return 1f + (enemyCountLevel - 1) * enemyCountPerLevel;
    }

    void UpdateUI()
    {
        if (pointsText != null)
        {
            pointsText.text = "Upgrade Points: " + upgradePoints;
        }

        if (damageText != null)
        {
            damageText.text = "Damage Lv" + damageLevel + " [" + damageCost + "pts] (+" + ((damageLevel - 1) * damagePerLevel * 100) + "%)";
        }

        if (speedText != null)
        {
            speedText.text = "Speed Lv" + speedLevel + " [" + speedCost + "pts] (+" + ((speedLevel - 1) * speedPerLevel * 100) + "%)";
        }

        if (healthText != null)
        {
            healthText.text = "Health Lv" + healthLevel + " [" + healthCost + "pts] (+" + ((healthLevel - 1) * healthPerLevel * 100) + "%)";
        }

        if (spawnSpeedText != null)
        {
            spawnSpeedText.text = "Spawn Speed Lv" + spawnSpeedLevel + " [" + spawnSpeedCost + "pts] (-" + ((spawnSpeedLevel - 1) * spawnSpeedPerLevel * 100) + "% delay)";
        }

        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemy Count Lv" + enemyCountLevel + " [" + enemyCountCost + "pts] (+" + ((enemyCountLevel - 1) * enemyCountPerLevel * 100) + "%)";
        }

        // Active/désactive les boutons selon les points disponibles
        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        if (damageButton != null)
            damageButton.interactable = (upgradePoints >= damageCost);

        if (speedButton != null)
            speedButton.interactable = (upgradePoints >= speedCost);

        if (healthButton != null)
            healthButton.interactable = (upgradePoints >= healthCost);

        if (spawnSpeedButton != null)
            spawnSpeedButton.interactable = (upgradePoints >= spawnSpeedCost);

        if (enemyCountButton != null)
            enemyCountButton.interactable = (upgradePoints >= enemyCountCost);
    }

    void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.enabled = true;

            // Cache le texte après 1.5 secondes
            CancelInvoke("HideFeedback");
            Invoke("HideFeedback", 1.5f);
        }
    }

    void HideFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.enabled = false;
        }
    }
}