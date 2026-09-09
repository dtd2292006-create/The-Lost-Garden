using UnityEngine;

public class LightSeed : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectSeed(value);
        }

        Destroy(gameObject);
    }
}