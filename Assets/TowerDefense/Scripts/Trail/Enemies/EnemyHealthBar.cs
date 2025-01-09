using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar; 
    [SerializeField] private Vector3 offset;  

    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError("EnemyHealthBar: No Enemy component found on this GameObject.");
        }

        if (healthBar == null)
        {
            Debug.LogError("EnemyHealthBar: No health bar (Slider) assigned.");
        }
    }

    private void Start()
    {
        if (enemy != null)
        {
            healthBar.maxValue = enemy.maxHealth;
            healthBar.value = enemy.maxHealth;
        }
    }

    public void UpdateHealthBar(float newHealth)
    {
        healthBar.value = newHealth;
    }

    public void UpdatePosition()
    {
        if (healthBar != null)
        {
            healthBar.transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position + offset);
        }
    }
}
