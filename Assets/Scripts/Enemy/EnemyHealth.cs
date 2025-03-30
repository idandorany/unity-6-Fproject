using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int health = 30;
    private int currentHealth;

    public Slider healthBar; 

    void Start()
    {
        currentHealth = health;

        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }

        Debug.Log($"{gameObject.name} spawned with {currentHealth} HP.");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);

        if (healthBar != null)
            healthBar.value = currentHealth;

        Debug.Log($"{gameObject.name} took {amount} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        LevelManager manager = FindFirstObjectByType<LevelManager>();
        if (manager != null)
        {
            manager.OnEnemyKilled();
        }
        else
        {
            Debug.LogWarning("LevelManager not found!");
        }
        Destroy(gameObject);
    }
}
