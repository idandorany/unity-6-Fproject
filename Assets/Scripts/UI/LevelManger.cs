using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject checkpointPortal;
    private int totalEnemies;

    void Start()
    {
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        totalEnemies = enemies.Length;

        checkpointPortal.SetActive(false);
    }

    public void OnEnemyKilled()
    {
        totalEnemies--;

        if (totalEnemies <= 0)
        {
            checkpointPortal.SetActive(true);
        }
    }

    public void LoadNextLevel(string nextLevelName)
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
