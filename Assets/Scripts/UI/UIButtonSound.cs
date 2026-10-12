using UnityEngine;
using UnityEngine.EventSystems; // Cần thiết để nhận diện chuột

[RequireComponent(typeof(AudioSource))]
public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Cấu hình cơ bản cho UI Audio
        audioSource.playOnAwake = false;
        
        // RẤT QUAN TRỌNG: Cho phép âm thanh kêu ngay cả khi Time.timeScale = 0 (khi đang Pause)
        audioSource.ignoreListenerPause = true; 
    }

    // Chạy khi người chơi đưa chuột vào nút
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    // Chạy khi người chơi click chuột xuống
    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}