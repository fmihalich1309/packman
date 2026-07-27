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
    public int randomNumber;
    public int points;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dotCollider = GetComponent<Collider2D>();

        randomNumber = Random.Range(1, 3);

        if (randomNumber == 1)
        {
            spriteRenderer.color = Color.yellow;
            points = 3;
        }

        else if (randomNumber == 2)
        {
            spriteRenderer.color = Color.blue;
            points = 2;

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

        if (randomNumber == 1)
        {
            ScoreManager3p.Instance.AddScore();
        }
        else
        {
            ScoreManager2p.Instance.AddScore();
        }
    }

    public void ResetDot()
    {
        eaten = false;
        spriteRenderer.color = originalColor;
        dotCollider.enabled = true;
    }
}
