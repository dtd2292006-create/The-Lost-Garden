using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Tự động thêm AudioSource khi gắn script
public class LightSeed : MonoBehaviour
{
    [SerializeField] private int value = 1;
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject collectParticlePrefab;

    [Header("Audio Effects")]
    [SerializeField] private AudioClip collectSound; // Kéo file âm thanh nhặt hạt vào đây

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D seedCollider;
    private bool isCollected = false; // Cờ chặn việc ăn 2 lần

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        seedCollider = GetComponent<Collider2D>();

        // Cấu hình loa mặc định
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu đã ăn rồi hoặc người chạm không phải Player thì bỏ qua
        if (isCollected || !other.CompareTag("Player"))
            return;

        isCollected = true; // Đánh dấu đã bị nhặt

        // 1. Sinh ra hiệu ứng nổ hạt
        if (collectParticlePrefab != null)
        {
            Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);
        }

        // 2. Cộng điểm
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectSeed(value);
        }

        // 3. Phát âm thanh nhặt
        if (collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        // 4. Ẩn hạt giống đi (Tắt hình ảnh và va chạm)
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (seedCollider != null) seedCollider.enabled = false;

        // 5. Hẹn giờ Destroy chính xác bằng độ dài của file âm thanh
        float destroyDelay = (collectSound != null) ? collectSound.length : 0.5f;
        Destroy(gameObject, destroyDelay);
    }
}