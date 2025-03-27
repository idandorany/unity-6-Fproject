using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 10;
    public float cooldown = 0.5f;
    private bool canAttack = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canAttack) return;

        Debug.Log("Weapon triggered with: " + other.name);

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy!");

            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                canAttack = false;
                Invoke(nameof(ResetAttack), cooldown);
            }
        }
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}
