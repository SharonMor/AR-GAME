using UnityEngine;

public class StairSpeedController : MonoBehaviour
{
    public MoveStairs stairsScript; // Assign this in the Inspector

    public void SetSpeedSlow()
    {
        stairsScript.speed = 0.3f; // Example slow speed
    }

    public void SetSpeedMedium()
    {
        stairsScript.speed = 0.5f; // Example medium speed
    }

    public void SetSpeedFast()
    {
        stairsScript.speed = 1.6f; // Example fast speed
    }
}
