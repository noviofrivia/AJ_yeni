using UnityEngine;

public class Knockback2D : MonoBehaviour
{
    [Header("Arena Collision")]
    [SerializeField] private float arenaKnockbackForce = 15f;
    

    [Header("Ball Collision")]
    [SerializeField] private float minBallKnockback = 10f;
    [SerializeField] private float maxBallKnockback = 30f;
    [SerializeField] private float velocityMultiplier = 1f;

    [Header("Collision Effects")]
    [SerializeField] private GameObject collisionEffectPrefab;
    [SerializeField] private float effectLifetime = 1f;
    [SerializeField] private bool isEffectSpawner = true;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
   

    [SerializeField] private SequentialSoundPlayer soundPlayer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "ArenaCollider")
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            rb.AddForce(knockbackDirection * arenaKnockbackForce, ForceMode2D.Impulse);
        }

        else if(collision.gameObject.tag == "goalpost_colliders")
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            rb.AddForce(knockbackDirection * arenaKnockbackForce, ForceMode2D.Impulse);
        }

        else if (collision.gameObject.CompareTag("ball"))
        {
            // Dynamic knockback (runs on both balls)
            Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (otherRb != null)
            {
                float impactSpeed = (rb.velocity - otherRb.velocity).magnitude;
                float dynamicForce = Mathf.Clamp(
                    impactSpeed * velocityMultiplier,
                    minBallKnockback,
                    maxBallKnockback
                );
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                otherRb.AddForce(knockbackDirection * dynamicForce, ForceMode2D.Impulse);
            }

            // Only the effect spawner handles sound and effects for ball-to-ball collisions
            if (isEffectSpawner)
            {
                if (collisionEffectPrefab != null)
                {
                    ContactPoint2D contact = collision.contacts[0];
                    GameObject effect = Instantiate(collisionEffectPrefab, contact.point, Quaternion.identity);
                    Destroy(effect, effectLifetime);
                }

            }
        }

        if (soundPlayer != null)
        {
            soundPlayer.HandleCollision(collision);
        }
    }
}
