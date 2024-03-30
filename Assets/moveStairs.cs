using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStairs : MonoBehaviour
{
    public float leftDistance = 0.5f; // The maximum distance the stairs move to the left from the starting point
    public float rightDistance = 0.5f; // The maximum distance the stairs move to the right from the starting point
    public float speed = 2.0f; // Speed at which the stairs will move

    private Vector3 startingPosition;
    private bool movingLeft = true;
    private bool isMoving = true;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isMoving = !isMoving;
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            isMoving = !isMoving;
        }

        if (!isMoving) return;

        DetermineMovementDirection();
        MoveStairsLeftOrRight();
    }

    void DetermineMovementDirection()
    {
        if (movingLeft && transform.position.x <= startingPosition.x - leftDistance)
        {
            movingLeft = false;
        }
        else if (!movingLeft && transform.position.x >= startingPosition.x + rightDistance)
        {
            movingLeft = true;
        }
    }

    void MoveStairsLeftOrRight()
    {
        float step = speed * Time.deltaTime * (movingLeft ? -1 : 1);
        transform.Translate(step, 0, 0);

        // Clamp the position to ensure it stays within the desired range for both left and right movements
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, startingPosition.x - leftDistance, startingPosition.x + rightDistance);
        transform.position = clampedPosition;
    }
}
