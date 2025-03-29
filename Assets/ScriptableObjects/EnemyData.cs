using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int maxHealth = 30;
    public float moveSpeed = 3.5f;
    public int damage = 10;
}
