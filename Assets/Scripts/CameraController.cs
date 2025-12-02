using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public BoxCollider2D mapBounds;
    public float smoothSpeed = 0.125f;
    private Vector3 offset;
    private float camHalfWidth;
    private float camHalfHeight;

    void Start()
    {
        offset = transform.position - playerTransform.position;
        camHalfHeight = Camera.main.orthographicSize;
        camHalfWidth = camHalfHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        Vector3 desiredPoistion = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPoistion, smoothSpeed);
        transform.position = smoothedPosition;

        float minX = mapBounds.bounds.min.x + camHalfWidth;
        float maxX = mapBounds.bounds.max.x - camHalfWidth;
        float minY = mapBounds.bounds.min.y + camHalfHeight;
        float maxY = mapBounds.bounds.max.y - camHalfHeight;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX), 
            Mathf.Clamp(transform.position.y, minY, maxY), 
            transform.position.z);
    }
}
