using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float height = 10f;
    public float rotationSpeed = 100f;

    private float currentAngle = 0f;

    void Update()
    {
        // Rotate with Q and E keys (or customize)
        if (Input.GetKey(KeyCode.Q))
            currentAngle -= rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
            currentAngle += rotationSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate rotated offset
        Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        // Set camera position & height
        transform.position = target.position + offset + Vector3.up * height;

        // Look at player
        transform.LookAt(target.position);
    }
}
