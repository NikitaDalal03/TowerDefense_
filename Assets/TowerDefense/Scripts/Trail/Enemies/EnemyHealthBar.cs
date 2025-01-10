using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarPrefab;  
    [SerializeField] private Vector3 offset;  

    private Slider healthBar;
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError("EnemyHealthBar: No Enemy component found on this GameObject.");
        }

        if (healthBarPrefab == null)
        {
            Debug.LogError("EnemyHealthBar: No health bar prefab assigned.");
        }
    }

    private void Start()
    {
        if (enemy != null && healthBarPrefab != null)
        {
            // Instantiate the health bar prefab
            GameObject healthBarObject = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);
            healthBar = healthBarObject.GetComponent<Slider>();

            // Ensure the health bar exists and is properly assigned
            if (healthBar == null)
            {
                Debug.LogError("EnemyHealthBar: Health bar prefab does not contain a Slider component.");
                return;
            }

            // Set the health bar's max value and initial value
            healthBar.maxValue = enemy.maxHealth;
            healthBar.value = enemy.health;

            // Subscribe to the health change event
            //enemy.OnHealthChanged += UpdateHealthBar;
        }
    }

    private void Update()
    {
        if (enemy != null && healthBar != null)
        {
            // Update the health bar position to follow the enemy
            UpdatePosition();
        }
    }

    private void UpdateHealthBar(float newHealth)
    {
        // Update the health bar slider value when the enemy's health changes
        if (healthBar != null)
        {
            healthBar.value = newHealth;
        }
    }

    private void UpdatePosition()
    {
        if (healthBar != null)
        {
            // Update the health bar's position to follow the enemy with an offset
            healthBar.transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position + offset);
        }
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            // Unsubscribe from the health change event when the script is destroyed
            //enemy.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
