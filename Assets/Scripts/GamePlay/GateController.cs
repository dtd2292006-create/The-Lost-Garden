using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GateController : MonoBehaviour
{
    [Header("Gate Sprites")]
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    [Header("Gate Colliders")]
    [SerializeField] private Collider2D blockerCollider;
    [SerializeField] private Collider2D entranceTrigger;

    [Header("Gate Audio & Effects")]
    [SerializeField] private AudioSource gateAudioSource;
    [SerializeField] private AudioClip activationSound;
    // Thêm 2 thông số để bạn tùy chỉnh trong Inspector
    [SerializeField] [Tooltip("Thời gian rung màn hình (giây)")] private float cameraShakeDuration = 0.5f;
    [SerializeField] [Tooltip("Độ trễ trước khi đổi hình ảnh cổng (giây)")] private float visualDelay = 0.5f;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName;

    private SpriteRenderer spriteRenderer;

    private bool isActive;
    private bool isLoadingScene;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("GateController cần SpriteRenderer trên cùng GameObject!", this);
        }
    }

    private void Start()
    {
        SetGateInactive();

        if (GameManager.Instance == null)
        {
            Debug.LogError("Không tìm thấy GameManager trong Scene!", this);
            return;
        }

        GameManager.Instance.OnGateUnlocked += HandleGateUnlocked;

        if (GameManager.Instance.GateUnlocked)
        {
            ActivateGateVisuals();
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGateUnlocked -= HandleGateUnlocked;
        }
    }

    private void SetGateInactive()
    {
        isActive = false;
        isLoadingScene = false;

        if (spriteRenderer != null && inactiveSprite != null)
            spriteRenderer.sprite = inactiveSprite;

        if (blockerCollider != null) blockerCollider.enabled = true;
        if (entranceTrigger != null) entranceTrigger.enabled = false;
    }

    private void HandleGateUnlocked()
    {
        if (isActive) return;
        isActive = true; 

        PlayActivationSound();

        // Sử dụng biến cameraShakeDuration
        if (CameraFollow.Instance != null)
        {
            CameraFollow.Instance.TriggerShake(cameraShakeDuration, 0.15f);
        }

        // Sử dụng biến visualDelay
        StartCoroutine(ActivateVisualsWithDelay(visualDelay));
    }

    private IEnumerator ActivateVisualsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ActivateGateVisuals();
    }

    private void ActivateGateVisuals()
    {
        if (spriteRenderer != null && activeSprite != null)
            spriteRenderer.sprite = activeSprite;

        if (blockerCollider != null) blockerCollider.enabled = false;
        if (entranceTrigger != null) entranceTrigger.enabled = true;
    }

    private void PlayActivationSound()
    {
        if (gateAudioSource == null || activationSound == null) return;
        gateAudioSource.PlayOneShot(activationSound);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive || isLoadingScene || !other.CompareTag("Player")) return;

        if (string.IsNullOrWhiteSpace(nextSceneName)) return;

        isLoadingScene = true;
        Time.timeScale = 1f;

        SceneManager.LoadScene(nextSceneName);
    }
}