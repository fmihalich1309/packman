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
        int randomcolor = Random.Range(1, 3);

        spriteRenderer = GetComponent<SpriteRenderer>();
        dotCollider = GetComponent<Collider2D>();

        bool TheDotIsYellow;

        if (randomcolor == 1)
        {
            spriteRenderer.color = Color.yellow;
            TheDotIsYellow = true;
        }

        else if (randomcolor == 2)
        {
            spriteRenderer.color = Color.blue;
            TheDotIsYellow = false;
        }
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
