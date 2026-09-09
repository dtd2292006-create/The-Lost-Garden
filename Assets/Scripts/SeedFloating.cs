using UnityEngine;

public class SeedFloating : MonoBehaviour
{
    [SerializeField] private float floatingSpeed = 2f;
    [SerializeField] private float floatingHeight = 0.15f;
    [SerializeField] private float rotationSpeed = 80f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newY =
            startPosition.y +
            Mathf.Sin(Time.time * floatingSpeed) * floatingHeight;

        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );

        transform.Rotate(
            0f,
            0f,
            rotationSpeed * Time.deltaTime
        );
    }
}