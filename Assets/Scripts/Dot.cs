using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Dot : MonoBehaviour
{
    [SerializeField] private float triggerRadius = 0.4f;

    private static readonly Color EatenColor = Color.black;

    private SpriteRenderer spriteRenderer;
    private Collider2D dotCollider;
    private Color originalColor;
    private bool eaten;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dotCollider = GetComponent<Collider2D>();
        originalColor = spriteRenderer.color;

        if (dotCollider is CircleCollider2D circleCollider)
        {
            circleCollider.radius = triggerRadius;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (eaten || !other.CompareTag("Player")) return;

        eaten = true;
        spriteRenderer.color = EatenColor;
        dotCollider.enabled = false;
        ScoreManager.Instance.AddScore();
    }

    public void ResetDot()
    {
        eaten = false;
        spriteRenderer.color = originalColor;
        dotCollider.enabled = true;
    }
}
