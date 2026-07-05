using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private float checkDistance = 0.42f;
    [SerializeField] private float wallPushDistance = 0.08f;
    [SerializeField] private Vector2 initialDirection = Vector2.left;
    [SerializeField] private float startDelay = 0f;

    private static readonly Vector2[] Directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private Rigidbody2D rb;
    private Vector2 currentDirection = Vector2.zero;
    private bool hasStarted;
    private Coroutine startRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        BeginAfterDelay();
    }

    private void FixedUpdate()
    {
        if (!hasStarted) return;
        rb.linearVelocity = currentDirection * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage();
            return;
        }

        if (!hasStarted) return;

        Vector2 normal = collision.GetContact(0).normal;
        rb.position += normal * wallPushDistance;
        currentDirection = PickRandomOpenDirection();
    }

    private Vector2 PickRandomOpenDirection()
    {
        List<Vector2> open = new List<Vector2>();
        foreach (Vector2 dir in Directions)
        {
            if (!IsBlocked(dir)) open.Add(dir);
        }

        return open.Count > 0 ? open[Random.Range(0, open.Count)] : -currentDirection;
    }

    private bool IsBlocked(Vector2 direction)
    {
        Vector2 checkPos = rb.position + direction * checkDistance;
        return Physics2D.OverlapCircle(checkPos, checkRadius, wallMask);
    }

    public void ResetGhost(Vector2 startPosition)
    {
        rb.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        BeginAfterDelay();
    }

    private void BeginAfterDelay()
    {
        if (startRoutine != null) StopCoroutine(startRoutine);
        hasStarted = false;
        currentDirection = Vector2.zero;
        startRoutine = StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(startDelay);
        currentDirection = initialDirection;
        hasStarted = true;
    }
}
