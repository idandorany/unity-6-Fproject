using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 10;
    public float cooldown = 1f;
    private bool canDamage = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                canDamage = false;
                Invoke(nameof(ResetDamage), cooldown);
            }
        }
    }

    void ResetDamage()
    {
        canDamage = true;
    }
}
