using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player character
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    public Vector3 offset; // Offset position from the player

    void Start()
    {

    } 

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
