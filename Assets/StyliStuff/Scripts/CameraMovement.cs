using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player or object to follow
    public Vector3 offset = new Vector3(0, 5, -10); // Offset from the target
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement
    void LateUpdate()
    {
        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;
        // Smoothly interpolate between current and desired positions
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Update the camera's position
        transform.position = smoothedPosition;
        // Optionally, make the camera look at the target
        transform.LookAt(target);
    }
}