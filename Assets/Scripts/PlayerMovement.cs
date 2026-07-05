using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SwipeInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private float checkDistance = 0.3f;

    private Rigidbody2D rb;
    private SwipeInput swipeInput;
    private Transform colliderTransform;

    private Vector2 currentDirection = Vector2.zero;
    private Vector2 desiredDirection = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        swipeInput = GetComponent<SwipeInput>();
        colliderTransform = GetComponentInChildren<Collider2D>().transform;
    }

    private void OnEnable()
    {
        swipeInput.OnSwipe += HandleSwipe;
    }

    private void OnDisable()
    {
        swipeInput.OnSwipe -= HandleSwipe;
    }

    private void HandleSwipe(Vector2 direction)
    {
        desiredDirection = direction;
    }

    private void FixedUpdate()
    {
        if (desiredDirection != Vector2.zero && desiredDirection != currentDirection && !IsBlocked(desiredDirection))
        {
            currentDirection = desiredDirection;
        }

        rb.linearVelocity = !IsBlocked(currentDirection) ? currentDirection * moveSpeed : Vector2.zero;
    }

    private bool IsBlocked(Vector2 direction)
    {
        if (direction == Vector2.zero) return false;

        Vector2 checkPos = (Vector2)colliderTransform.position + direction * checkDistance;
        return Physics2D.OverlapCircle(checkPos, checkRadius, wallMask);
    }

    public void ResetState()
    {
        currentDirection = Vector2.zero;
        desiredDirection = Vector2.zero;
    }
}
