using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseOverlay;

    private bool isPaused;

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseOverlay != null)
        {
            pauseOverlay.SetActive(false);
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