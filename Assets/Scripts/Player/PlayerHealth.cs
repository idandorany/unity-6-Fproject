using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar; 

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.maxValue = maxHealth;

        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        Debug.Log("Player took " + amount + " damage. HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    void Die()
    {
        Debug.Log("Player died.");

        GameOverManager gm = FindFirstObjectByType<GameOverManager>();
        if (gm != null)
        {
            gm.ShowGameOver();
        }

        // Optional: Disable movement or other systems
        gameObject.SetActive(false);
    }

}
