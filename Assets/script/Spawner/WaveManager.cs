using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public int currentWave = 0;
    public int baseEnemiesPerWave = 5;
    public float timeBetweenSpawns = 1.5f;

    private int enemiesToSpawn;
    private bool isSpawning = false;

    [Header("References")]
    public UpgradeManager upgradeManager;
    public TowerDefense[] towers;
    public Transform waypointPath;
    public EnemyBase enemyBase;

    [Header("UI References")]
    public Button startWaveButton;
    public TMP_Text waveText;
    public TMP_Text enemiesLeftText;

    private int enemiesAlive = 0;

    void Start()
    {
        // Configure le bouton
        if (startWaveButton != null)
        {
            startWaveButton.onClick.AddListener(StartNextWave);
        }

        UpdateUI();
    }

    public void StartNextWave()
    {
        if (isSpawning) return;

        currentWave++;

        // Donne des points d'upgrade à la fin de la vague précédente
        if (upgradeManager != null)
        {
            upgradeManager.AddUpgradePoints(currentWave);
        }

        // Calcule le nombre d'ennemis avec le multiplicateur
        float baseEnemies = baseEnemiesPerWave + (currentWave - 1) * 3;
        if (upgradeManager != null)
        {
            enemiesToSpawn = Mathf.RoundToInt(baseEnemies * upgradeManager.GetEnemyCountMultiplier());
        }
        else
        {
            enemiesToSpawn = Mathf.RoundToInt(baseEnemies);
        }

        // Désactive le bouton pendant le spawn
        if (startWaveButton != null)
        {
            startWaveButton.interactable = false;
        }

        // Met à jour les stats des tours
        UpdateTowerStats();

        StartCoroutine(SpawnWave());
        UpdateUI();
    }

    IEnumerator SpawnWave()
    {
        isSpawning = true;

        // Calcule le délai entre spawns avec le multiplicateur
        float currentSpawnDelay = timeBetweenSpawns;
        if (upgradeManager != null)
        {
            currentSpawnDelay = timeBetweenSpawns * upgradeManager.GetSpawnSpeedMultiplier();
        }

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            UpdateUI();
            yield return new WaitForSeconds(currentSpawnDelay);
        }

        isSpawning = false;

        // Réactive le bouton après le spawn
        yield return new WaitForSeconds(2f);
        if (startWaveButton != null && enemiesAlive == 0)
        {
            startWaveButton.interactable = true;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Enemy Prefab ou Spawn Point non assigné !");
            return;
        }

        if (waypointPath == null)
        {
            Debug.LogError("WaypointPath non assigné sur WaveManager !");
            return;
        }

        if (enemyBase == null)
        {
            Debug.LogError("EnemyBase non assigné sur WaveManager !");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemiesAlive++;

        // Passe les références nécessaires à l'ennemi AVANT qu'il s'initialise
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.upgradeManager = upgradeManager;
            movement.waypointPath = waypointPath;
            movement.enemyBase = enemyBase;
            Debug.Log("Références assignées à " + enemy.name + " : WaypointPath=" + (waypointPath != null));
        }
        else
        {
            Debug.LogError("EnemyMovement non trouvé sur le prefab Enemy !");
        }

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.upgradeManager = upgradeManager;
            health.waveManager = this;
        }
        else
        {
            Debug.LogError("EnemyHealth non trouvé sur le prefab Enemy !");
        }
    }

    // Appelé quand un ennemi meurt ou atteint la base
    public void EnemyDestroyed()
    {
        enemiesAlive--;
        UpdateUI();

        // Si plus d'ennemis et spawn terminé, réactive le bouton
        if (enemiesAlive <= 0 && !isSpawning && startWaveButton != null)
        {
            startWaveButton.interactable = true;
        }
    }

    void UpdateTowerStats()
    {
        // Met à jour toutes les tours référencées
        if (towers != null)
        {
            foreach (TowerDefense tower in towers)
            {
                if (tower != null)
                {
                    tower.UpdateStats();
                }
            }
        }
    }

    void UpdateUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }

        if (enemiesLeftText != null)
        {
            enemiesLeftText.text = "Enemies: " + enemiesAlive;
        }
    }

    void Update()
    {
        // Compte les ennemis vivants (sécurité)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesAlive = enemies.Length;
        UpdateUI();
    }
}