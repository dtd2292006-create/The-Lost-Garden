using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerFootstepAudio : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private float movementThreshold = 0.05f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool isMoving =
            Time.timeScale > 0f &&
            rb.linearVelocity.sqrMagnitude >
            movementThreshold * movementThreshold;

        if (isMoving && !footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
        else if (!isMoving && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }
}