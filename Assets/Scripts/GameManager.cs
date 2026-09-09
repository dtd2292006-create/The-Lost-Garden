using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int totalSeeds = 1;
    [SerializeField] private TMP_Text statusText;

    private int collectedSeeds;
    private bool levelCompleted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        collectedSeeds = 0;
        levelCompleted = false;

        SetupStatusText();
        UpdateUI();
    }

    private void SetupStatusText()
    {
        if (statusText == null)
        {
            Debug.LogError("Chưa gắn StatusText vào GameManager!");
            return;
        }

        statusText.gameObject.SetActive(true);

        RectTransform rect = statusText.rectTransform;

        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);

        rect.anchoredPosition = new Vector2(30f, -30f);
        rect.sizeDelta = new Vector2(700f, 150f);
        rect.localScale = Vector3.one;

        statusText.fontSize = 32f;
        statusText.color = Color.black;
        statusText.alignment = TextAlignmentOptions.TopLeft;
        statusText.enableWordWrapping = false;
    }

    public void CollectSeed(int amount)
    {
        if (levelCompleted)
            return;

        collectedSeeds += amount;
        collectedSeeds = Mathf.Min(collectedSeeds, totalSeeds);

        if (collectedSeeds >= totalSeeds)
        {
            levelCompleted = true;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (statusText == null)
            return;

        if (levelCompleted)
        {
            statusText.text =
                "Hạt sáng: " +
                collectedSeeds +
                " / " +
                totalSeeds +
                "\nĐã thu thập đủ hạt sáng!";
        }
        else
        {
            statusText.text =
                "Hạt sáng: " +
                collectedSeeds +
                " / " +
                totalSeeds;
        }
    }
}