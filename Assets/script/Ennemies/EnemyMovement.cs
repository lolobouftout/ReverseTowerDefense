using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float baseSpeed = 2f;
    private float currentSpeed;

    [Header("Waypoint System")]
    public Transform waypointPath;
    private Transform[] waypoints;
    private int waypointIndex = 0;

    [Header("Stats")]
    public float baseDamage = 10f;
    public float currentDamage;

    [Header("Target")]
    public EnemyBase enemyBase;

    [Header("References (Assignées automatiquement)")]
    public UpgradeManager upgradeManager;

    void Start()
    {
        // Attend que les références soient assignées
        StartCoroutine(Initialize());
    }

    System.Collections.IEnumerator Initialize()
    {
        // Attend 2 frames pour être sûr
        yield return null;
        yield return null;

        Debug.Log("Initialisation de " + gameObject.name + " : waypointPath=" + (waypointPath != null) + ", enemyBase=" + (enemyBase != null) + ", upgradeManager=" + (upgradeManager != null));

        // Récupère tous les waypoints depuis WaypointPath
        if (waypointPath != null)
        {
            waypoints = new Transform[waypointPath.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = waypointPath.GetChild(i);
            }
            Debug.Log("Waypoints trouvés : " + waypoints.Length);
        }
        else
        {
            Debug.LogError("WaypointPath TOUJOURS non assigné sur " + gameObject.name + " après attente !");
            yield break;
        }

        // Applique les upgrades au spawn
        if (upgradeManager != null)
        {
            currentSpeed = baseSpeed * upgradeManager.GetSpeedMultiplier();
            currentDamage = baseDamage * upgradeManager.GetDamageMultiplier();
        }
        else
        {
            currentSpeed = baseSpeed;
            currentDamage = baseDamage;
            Debug.LogWarning("UpgradeManager non assigné sur " + gameObject.name);
        }

        // Change la couleur selon le niveau de damage
        UpdateColor();
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Move();
    }

    void Move()
    {
        // Si on n'a pas atteint tous les waypoints
        if (waypointIndex < waypoints.Length)
        {
            // Déplace vers le waypoint actuel
            Vector2 targetPosition = waypoints[waypointIndex].position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

            // Si on est assez proche du waypoint
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                waypointIndex++;
            }
        }
        else
        {
            // Arrivé à la fin : attaquer la base
            ReachBase();
        }
    }

    void ReachBase()
    {
        // Attaque la base ennemie
        if (enemyBase != null)
        {
            enemyBase.TakeDamage(currentDamage);
        }
        else
        {
            Debug.LogWarning("EnemyBase non assignée sur " + gameObject.name);
        }

        // Se détruit après avoir attaqué
        Destroy(gameObject);
    }

    void UpdateColor()
    {
        // Plus l'ennemi est fort, plus il est rouge foncé
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null && upgradeManager != null)
        {
            float intensity = 1f - (upgradeManager.damageLevel * 0.08f);
            intensity = Mathf.Clamp(intensity, 0.3f, 1f);
            sr.color = new Color(1f, intensity, intensity);
        }
    }

    public float GetDamage()
    {
        return currentDamage;
    }
}