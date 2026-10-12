using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector2.zero;

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
    }
}