using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; // Thêm thư viện Audio
using System.Collections; // Thêm thư viện Coroutine

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseOverlay;
    
    [Header("Audio Initialization")]
    [SerializeField] private AudioMixer audioMixer; // Thêm ô kéo Mixer vào đây

    private bool isPaused;

    private void Start()
    {
        // 1. Chạy tiến trình khởi tạo âm thanh đầu game
        StartCoroutine(InitializeAudioOnStart());

        // 2. Logic cũ: Tắt menu và reset thời gian
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseOverlay != null)
        {
            pauseOverlay.SetActive(false);
        }
    }

    private IEnumerator InitializeAudioOnStart()
    {
        // MẸO: Chờ 1 frame để đảm bảo AudioMixer đã khởi tạo xong hoàn toàn
        // (Nếu set âm lượng ngay lập tức ở frame 0, Unity AudioMixer thường sẽ phớt lờ lệnh này)
        yield return null; 
        
        if (audioMixer != null)
        {
            // Đọc và set BGM
            float bgmVol = PlayerPrefs.GetFloat("BGM_Volume", 1f);
            bool bgmMute = PlayerPrefs.GetInt("BGM_Mute", 0) == 1;
            audioMixer.SetFloat("BGM_Volume", bgmMute ? -80f : (Mathf.Log10(bgmVol) * 20f));

            // Đọc và set SFX
            float sfxVol = PlayerPrefs.GetFloat("SFX_Volume", 1f);
            bool sfxMute = PlayerPrefs.GetInt("SFX_Mute", 0) == 1;
            audioMixer.SetFloat("SFX_Volume", sfxMute ? -80f : (Mathf.Log10(sfxVol) * 20f));
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseOverlay.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Thoát game");
        Application.Quit();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}