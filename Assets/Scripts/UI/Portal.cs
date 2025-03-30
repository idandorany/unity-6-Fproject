using UnityEngine;

public class Portal : MonoBehaviour
{
    public string nextLevelName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager manager = FindFirstObjectByType<LevelManager>();
            if (manager != null)
            {
                manager.LoadNextLevel(nextLevelName);
            }
            else
            {
                Debug.LogWarning("LevelManager not found!");
            }
        }
    }
}
