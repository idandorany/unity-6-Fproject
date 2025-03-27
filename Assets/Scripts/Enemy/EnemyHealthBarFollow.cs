using UnityEngine;

public class EnemyHealthBarFollow : MonoBehaviour
{
    public Transform target;       
    public Vector3 offset = new Vector3(0, 2f, 0); // Adjust Y as needed

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 fixedPos = target.position + offset;
            transform.position = new Vector3(fixedPos.x, offset.y, fixedPos.z); // lock Y
        }

        // Always face the camera
        transform.forward = Camera.main.transform.forward;
    }
}
