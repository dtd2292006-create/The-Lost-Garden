using UnityEngine;

public class SeedFloating : MonoBehaviour
{
    [SerializeField] private float floatingSpeed = 2f;
    [SerializeField] private float floatingHeight = 0.15f;

    private Vector3 startLocalPosition;
    private float phaseOffset;

    private void Awake()
    {
        startLocalPosition = transform.localPosition;
        phaseOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        Vector3 newPosition = startLocalPosition;

        newPosition.y += Mathf.Sin(
            Time.time * floatingSpeed + phaseOffset
        ) * floatingHeight;

        transform.localPosition = newPosition;
    }
}