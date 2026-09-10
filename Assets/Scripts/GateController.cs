using UnityEngine;
using UnityEngine.SceneManagement;

public class GateController : MonoBehaviour
{
    [Header("Gate Sprites")]
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    [Header("Gate Colliders")]
    [SerializeField] private Collider2D blockerCollider;
    [SerializeField] private Collider2D entranceTrigger;

    [Header("Gate Audio")]
    [SerializeField] private AudioSource gateAudioSource;
    [SerializeField] private AudioClip activationSound;

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
            Debug.LogError(
                "GateController cần SpriteRenderer trên cùng GameObject!",
                this
            );
        }
    }

    private void Start()
    {
        SetGateInactive();

        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "Không tìm thấy GameManager trong Scene!",
                this
            );

            return;
        }

        GameManager.Instance.OnGateUnlocked += ActivateGate;

        if (GameManager.Instance.GateUnlocked)
        {
            ActivateGate();
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGateUnlocked -= ActivateGate;
        }
    }

    private void SetGateInactive()
    {
        isActive = false;
        isLoadingScene = false;

        if (spriteRenderer != null &&
            inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite;
        }

        if (blockerCollider != null)
        {
            blockerCollider.enabled = true;
        }

        if (entranceTrigger != null)
        {
            entranceTrigger.enabled = false;
        }
    }

    private void ActivateGate()
    {
        if (isActive)
            return;

        isActive = true;

        if (spriteRenderer != null &&
            activeSprite != null)
        {
            spriteRenderer.sprite = activeSprite;
        }

        if (blockerCollider != null)
        {
            blockerCollider.enabled = false;
        }

        if (entranceTrigger != null)
        {
            entranceTrigger.enabled = true;
        }

        PlayActivationSound();

        Debug.Log("Cổng đã được kích hoạt!");
    }

    private void PlayActivationSound()
    {
        if (gateAudioSource == null)
        {
            Debug.LogWarning(
                "GateController chưa được gắn AudioSource!",
                this
            );

            return;
        }

        if (activationSound == null)
        {
            Debug.LogWarning(
                "GateController chưa được gắn âm thanh kích hoạt!",
                this
            );

            return;
        }

        gateAudioSource.PlayOneShot(activationSound);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive || isLoadingScene)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogError(
                "Chưa nhập tên Scene tiếp theo!",
                this
            );

            return;
        }

        isLoadingScene = true;
        Time.timeScale = 1f;

        SceneManager.LoadScene(nextSceneName);
    }
}