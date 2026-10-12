using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject audioPanel;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle bgmMuteToggle;
    [SerializeField] private Toggle sfxMuteToggle;
    
    [Header("Toggle Background Images")]
    [SerializeField] private Image bgmBackgroundImage; // Thêm tham chiếu cho ảnh nền BGM
    [SerializeField] private Image sfxBackgroundImage; // Thêm tham chiếu cho ảnh nền SFX

    [Header("Audio Snapshots")]
    [SerializeField] private AudioMixerSnapshot normalSnapshot;
    [SerializeField] private AudioMixerSnapshot menuOpenSnapshot;

    private float previousBgmVolume = 1f;
    private float previousSfxVolume = 1f;
    private bool isUpdatingUI = false; 

    private const string BGM_VOL_KEY = "BGM_Volume";
    private const string SFX_VOL_KEY = "SFX_Volume";
    private const string BGM_MUTE_KEY = "BGM_Mute";
    private const string SFX_MUTE_KEY = "SFX_Mute";

    private void Start()
    {
        LoadAudioSettings();

        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        if (bgmMuteToggle != null) bgmMuteToggle.onValueChanged.AddListener(ToggleBGMMute);
        if (sfxMuteToggle != null) sfxMuteToggle.onValueChanged.AddListener(ToggleSFXMute);
    }

    private void OnEnable()
    {
        if (menuOpenSnapshot != null) menuOpenSnapshot.TransitionTo(0.5f);
        OpenPauseTab(); 
    }

    private void OnDisable()
    {
        if (normalSnapshot != null) normalSnapshot.TransitionTo(0.5f);
    }

    // ================= XỬ LÝ CHUYỂN TAB =================
    public void OpenPauseTab()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        if (audioPanel != null) audioPanel.SetActive(false);
    }

    public void OpenAudioTab()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (audioPanel != null) audioPanel.SetActive(true);
    }

    // Hàm phụ trợ để làm tàng hình/hiện hình Background
    private void SetBackgroundVisible(Image bgImage, bool isVisible)
    {
        if (bgImage == null) return;
        Color c = bgImage.color;
        c.a = isVisible ? 1f : 0f; // 1 = Hiện rõ, 0 = Tàng hình
        bgImage.color = c;
    }

    // ================= XỬ LÝ LƯU VÀ TẢI PLAYERPREFS =================
    private void LoadAudioSettings()
    {
        isUpdatingUI = true;

        // Load Nhạc nền
        bgmSlider.value = PlayerPrefs.GetFloat(BGM_VOL_KEY, 1f);
        bgmMuteToggle.isOn = PlayerPrefs.GetInt(BGM_MUTE_KEY, 0) == 1;
        previousBgmVolume = bgmSlider.value > bgmSlider.minValue ? bgmSlider.value : 1f;

        // Load Hiệu ứng
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOL_KEY, 1f);
        sfxMuteToggle.isOn = PlayerPrefs.GetInt(SFX_MUTE_KEY, 0) == 1;
        previousSfxVolume = sfxSlider.value > sfxSlider.minValue ? sfxSlider.value : 1f;

        // Cập nhật hiển thị tàng hình cho Background ngay khi mới load
        SetBackgroundVisible(bgmBackgroundImage, !bgmMuteToggle.isOn);
        SetBackgroundVisible(sfxBackgroundImage, !sfxMuteToggle.isOn);

        ApplyBGMToMixer(bgmSlider.value, bgmMuteToggle.isOn);
        ApplySFXToMixer(sfxSlider.value, sfxMuteToggle.isOn);

        isUpdatingUI = false;
    }

    // ================= XỬ LÝ NHẠC NỀN (BGM) =================
    private void SetBGMVolume(float value)
    {
        if (isUpdatingUI) return;

        if (value > bgmSlider.minValue && bgmMuteToggle.isOn)
        {
            isUpdatingUI = true;
            bgmMuteToggle.isOn = false;
            SetBackgroundVisible(bgmBackgroundImage, true); // Hiện lại nền
            PlayerPrefs.SetInt(BGM_MUTE_KEY, 0);
            isUpdatingUI = false;
        }

        if (value <= bgmSlider.minValue && !bgmMuteToggle.isOn)
        {
            isUpdatingUI = true;
            bgmMuteToggle.isOn = true;
            SetBackgroundVisible(bgmBackgroundImage, false); // Ẩn nền
            PlayerPrefs.SetInt(BGM_MUTE_KEY, 1);
            isUpdatingUI = false;
        }

        PlayerPrefs.SetFloat(BGM_VOL_KEY, value);
        PlayerPrefs.Save();
        ApplyBGMToMixer(value, bgmMuteToggle.isOn);
    }

    private void ToggleBGMMute(bool isMuted)
    {
        if (isUpdatingUI) return;

        isUpdatingUI = true;
        PlayerPrefs.SetInt(BGM_MUTE_KEY, isMuted ? 1 : 0);
        
        // Cập nhật tàng hình dựa theo trạng thái
        SetBackgroundVisible(bgmBackgroundImage, !isMuted);

        if (isMuted)
        {
            if (bgmSlider.value > bgmSlider.minValue) previousBgmVolume = bgmSlider.value;
            bgmSlider.value = bgmSlider.minValue;
        }
        else
        {
            bgmSlider.value = previousBgmVolume;
            PlayerPrefs.SetFloat(BGM_VOL_KEY, previousBgmVolume);
        }
        
        PlayerPrefs.Save();
        ApplyBGMToMixer(bgmSlider.value, isMuted);
        isUpdatingUI = false;
    }

    private void ApplyBGMToMixer(float value, bool isMuted)
    {
        float db = isMuted ? -80f : (Mathf.Log10(value) * 20f);
        audioMixer.SetFloat("BGM_Volume", db);
    }

    // ================= XỬ LÝ HIỆU ỨNG (SFX) =================
    private void SetSFXVolume(float value)
    {
        if (isUpdatingUI) return;

        if (value > sfxSlider.minValue && sfxMuteToggle.isOn)
        {
            isUpdatingUI = true;
            sfxMuteToggle.isOn = false;
            SetBackgroundVisible(sfxBackgroundImage, true);
            PlayerPrefs.SetInt(SFX_MUTE_KEY, 0);
            isUpdatingUI = false;
        }

        if (value <= sfxSlider.minValue && !sfxMuteToggle.isOn)
        {
            isUpdatingUI = true;
            sfxMuteToggle.isOn = true;
            SetBackgroundVisible(sfxBackgroundImage, false);
            PlayerPrefs.SetInt(SFX_MUTE_KEY, 1);
            isUpdatingUI = false;
        }

        PlayerPrefs.SetFloat(SFX_VOL_KEY, value);
        PlayerPrefs.Save();
        ApplySFXToMixer(value, sfxMuteToggle.isOn);
    }

    private void ToggleSFXMute(bool isMuted)
    {
        if (isUpdatingUI) return;

        isUpdatingUI = true;
        PlayerPrefs.SetInt(SFX_MUTE_KEY, isMuted ? 1 : 0);
        
        SetBackgroundVisible(sfxBackgroundImage, !isMuted);

        if (isMuted)
        {
            if (sfxSlider.value > sfxSlider.minValue) previousSfxVolume = sfxSlider.value;
            sfxSlider.value = sfxSlider.minValue;
        }
        else
        {
            sfxSlider.value = previousSfxVolume;
            PlayerPrefs.SetFloat(SFX_VOL_KEY, previousSfxVolume);
        }

        PlayerPrefs.Save();
        ApplySFXToMixer(sfxSlider.value, isMuted);
        isUpdatingUI = false;
    }

    private void ApplySFXToMixer(float value, bool isMuted)
    {
        float db = isMuted ? -80f : (Mathf.Log10(value) * 20f);
        audioMixer.SetFloat("SFX_Volume", db);
    }
}