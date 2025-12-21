using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Levels")]
    public int damageLevel = 1;
    public int speedLevel = 1;
    public int healthLevel = 1;

    [Header("Upgrade Multipliers")]
    public float damagePerLevel = 0.25f; // +25% par niveau
    public float speedPerLevel = 0.2f;   // +20% par niveau
    public float healthPerLevel = 0.3f;  // +30% par niveau

    [Header("UI References")]
    public TMP_Text damageText;
    public TMP_Text speedText;
    public TMP_Text healthText;
    public TMP_Text feedbackText;

    [Header("Buttons")]
    public Button damageButton;
    public Button speedButton;
    public Button healthButton;

    void Start()
    {
        // Configure les boutons
        if (damageButton != null)
            damageButton.onClick.AddListener(UpgradeDamage);

        if (speedButton != null)
            speedButton.onClick.AddListener(UpgradeSpeed);

        if (healthButton != null)
            healthButton.onClick.AddListener(UpgradeHealth);

        UpdateUI();
    }

    public void UpgradeDamage()
    {
        damageLevel++;
        ShowFeedback("DAMAGE UPGRADED! Level " + damageLevel);
        UpdateUI();
    }

    public void UpgradeSpeed()
    {
        speedLevel++;
        ShowFeedback("SPEED UPGRADED! Level " + speedLevel);
        UpdateUI();
    }

    public void UpgradeHealth()
    {
        healthLevel++;
        ShowFeedback("HEALTH UPGRADED! Level " + healthLevel);
        UpdateUI();
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

    void UpdateUI()
    {
        if (damageText != null)
        {
            damageText.text = "Damage: Lv " + damageLevel + " (+" + ((damageLevel - 1) * damagePerLevel * 100) + "%)";
        }

        if (speedText != null)
        {
            speedText.text = "Speed: Lv " + speedLevel + " (+" + ((speedLevel - 1) * speedPerLevel * 100) + "%)";
        }

        if (healthText != null)
        {
            healthText.text = "Health: Lv " + healthLevel + " (+" + ((healthLevel - 1) * healthPerLevel * 100) + "%)";
        }
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