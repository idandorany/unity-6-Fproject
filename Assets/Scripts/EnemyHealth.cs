using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 30;

    void Start()
    {
        Debug.Log($"{gameObject.name} has spawned with {health} HP.");
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining HP: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}
