using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeInput : MonoBehaviour
{
    [SerializeField] private float minSwipeDistance = 50f;

    public event Action<Vector2> OnSwipe;

    private Vector2 startPos;
    private bool isDragging;

    private void Update()
    {
        Pointer pointer = Pointer.current;
        if (pointer == null) return;

        if (pointer.press.wasPressedThisFrame)
        {
            startPos = pointer.position.ReadValue();
            isDragging = true;
        }
        else if (isDragging && pointer.press.isPressed)
        {
            Vector2 currentPos = pointer.position.ReadValue();
            Vector2 delta = currentPos - startPos;

            if (delta.magnitude >= minSwipeDistance)
            {
                OnSwipe?.Invoke(GetCardinalDirection(delta));
                startPos = currentPos;
            }
        }
        else if (pointer.press.wasReleasedThisFrame)
        {
            isDragging = false;
        }
    }

    private static Vector2 GetCardinalDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            return delta.x > 0 ? Vector2.right : Vector2.left;

        return delta.y > 0 ? Vector2.up : Vector2.down;
    }
}
