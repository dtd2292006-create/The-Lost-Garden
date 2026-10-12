using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    // Cấp quyền truy cập toàn cục (Singleton) để dễ dàng gọi hiệu ứng rung từ script khác
    public static CameraFollow Instance { get; private set; } 

    [SerializeField] private Transform player;
    [SerializeField] private Collider2D mapBounds;

    private Camera cam;
    
    // Các biến xử lý hiệu ứng Rung
    private float shakeTimer;
    private float currentShakeMagnitude;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        cam = GetComponent<Camera>();
    }

    // Gọi hàm này để kích hoạt rung màn hình
    public void TriggerShake(float duration, float magnitude)
    {
        shakeTimer = duration;
        currentShakeMagnitude = magnitude;
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

        float cameraX = (minX <= maxX) ? Mathf.Clamp(player.position.x, minX, maxX) : bounds.center.x;
        float cameraY = (minY <= maxY) ? Mathf.Clamp(player.position.y, minY, maxY) : bounds.center.y;

        Vector3 targetPos = new Vector3(cameraX, cameraY, -10f);

        // Áp dụng độ lệch (Offset) ngẫu nhiên nếu hệ thống đang đếm ngược thời gian rung
        if (shakeTimer > 0)
        {
            Vector2 shakeOffset = Random.insideUnitCircle * currentShakeMagnitude;
            targetPos += new Vector3(shakeOffset.x, shakeOffset.y, 0f);
            
            shakeTimer -= Time.deltaTime;
        }

        transform.position = targetPos;
    }
}