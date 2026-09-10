using UnityEngine;
using TMPro;
using System.Collections;

public class MapTitleUI : MonoBehaviour
{
    [SerializeField] private string mapName = "ĐỒNG CỎ ÁNH SÁNG";
    [SerializeField] private TMP_Text mapNameText;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float visibleDuration = 2f;
    [SerializeField] private float fadeOutDuration = 0.6f;

    private void Start()
    {
        mapNameText.text = mapName;
        StartCoroutine(ShowMapTitle());
    }

    private IEnumerator ShowMapTitle()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;

        float time = 0f;

        while (time < fadeInDuration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = time / fadeInDuration;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(visibleDuration);

        time = 0f;

        while (time < fadeOutDuration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = 1f - time / fadeOutDuration;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}