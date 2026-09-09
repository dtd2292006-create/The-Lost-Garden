using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Collider2D mapBounds;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (player == null || mapBounds == null)
            return;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        Bounds bounds = mapBounds.bounds;

        float minX = bounds.min.x + halfWidth;
        float maxX = bounds.max.x - halfWidth;

        float minY = bounds.min.y + halfHeight;
        float maxY = bounds.max.y - halfHeight;

        float cameraX;
        float cameraY;

        if (minX <= maxX)
        {
            cameraX = Mathf.Clamp(
                player.position.x,
                minX,
                maxX
            );
        }
        else
        {
            cameraX = bounds.center.x;
        }

        if (minY <= maxY)
        {
            cameraY = Mathf.Clamp(
                player.position.y,
                minY,
                maxY
            );
        }
        else
        {
            cameraY = bounds.center.y;
        }

        transform.position = new Vector3(
            cameraX,
            cameraY,
            -10f
        );
    }
}