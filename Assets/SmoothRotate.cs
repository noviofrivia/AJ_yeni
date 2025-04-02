using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f; // Degrees per second
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Rotate around Y-axis

    void Update()
    {
        // Rotate around the specified axis at a constant speed
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
