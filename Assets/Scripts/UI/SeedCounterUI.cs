using UnityEngine;
using TMPro;

public class SeedCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text seedCountText;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("Không tìm thấy GameManager!");
            return;
        }

        GameManager.Instance.OnSeedCountChanged += UpdateCounter;

        UpdateCounter(
            GameManager.Instance.CollectedSeeds,
            GameManager.Instance.TotalSeeds
        );
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSeedCountChanged -= UpdateCounter;
        }
    }

    private void UpdateCounter(int currentSeeds, int totalSeeds)
    {
        seedCountText.text =
            currentSeeds + " / " + totalSeeds;
    }
}