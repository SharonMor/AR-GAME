using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public Transform laserOrigin; // The starting point of the first laser beam
    public LineRenderer firstLaserLineRenderer; // The LineRenderer component to visualize the first laser beam
    public LineRenderer secondLaserLineRenderer; // The LineRenderer component to visualize the second laser beam
    public Transform smallBlackDot; // The fixed end point for the second laser
    public TextMesh laserLengthText;
    public GameObject pillar; // The pillar object to change color
    public Color[] laserLengthColors; // Array of colors for different laser lengths
    public AudioClip successSound; // The sound to play on success
    public MoveStairs moveStairs;

    private AudioSource audioSource; // AudioSource component for playing sounds
    private Vector3 hitPoint; // To store the hit point of the first laser
    private float laserLength;
    private Renderer pillarRenderer; // Renderer for the pillar
    private bool allowSound = true;
    void Start()
    {
        if (firstLaserLineRenderer == null)
            firstLaserLineRenderer = GetComponent<LineRenderer>();

        pillarRenderer = pillar.GetComponent<Renderer>(); // Get the Renderer component of the pillar

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component if it doesn't exist
        }
    }

    void Update()
    {
        ShootFirstLaserFromPoint(laserOrigin.position, laserOrigin.forward);
        if (hitPoint != Vector3.zero) // Check if the first laser hit something
        {
            ShootSecondLaserFromPoint(hitPoint, smallBlackDot.position);
            UpdatePillarColorBasedOnLaserLength(); // Update the pillar color based on laser length
        }
    }

    void ShootFirstLaserFromPoint(Vector3 start, Vector3 direction)
    {
        Ray ray = new Ray(start, direction);
        RaycastHit hit;

        firstLaserLineRenderer.SetPosition(0, start); // Set the start position of the first laser

        if (Physics.Raycast(ray, out hit))
        {
            hitPoint = hit.point; // Store the hit point
            firstLaserLineRenderer.SetPosition(1, hit.point); // First laser hit something, end the line here
            laserLength = Vector3.Distance(start, hit.point);
            laserLengthText.text = "Distance: " + laserLength.ToString("F2");
        }
        else
        {
            firstLaserLineRenderer.SetPosition(1, start + direction * 100); // No hit, draw the line long enough from the starting point
            hitPoint = Vector3.zero; // Reset hit point
        }
    }

    void ShootSecondLaserFromPoint(Vector3 start, Vector3 end)
    {
        secondLaserLineRenderer.SetPosition(0, start); // Set the start position of the second laser
        secondLaserLineRenderer.SetPosition(1, end); // Set the end position of the second laser
    }

    void UpdatePillarColorBasedOnLaserLength()
    {
        // Ensure there are exactly 4 colors for the 4 specified lengths
        if (laserLengthColors == null || laserLengthColors.Length != 4)
        {
            Debug.LogWarning("The laserLengthColors array must contain exactly 4 colors.");
            return; // Early return to avoid errors if the colors array is not set up correctly
        }

        // Round the laser length to two decimal places
        float roundedLaserLength = Mathf.Round(laserLength * 100f) / 100f;

        // Determine the color based on the specific lengths
        if (roundedLaserLength <= 0.33f)
        {
            pillarRenderer.material.color = laserLengthColors[0];
            if (!moveStairs.isMoving && allowSound)
            {
                audioSource.PlayOneShot(successSound); // Play the success sound only when the stairs are stopped
                allowSound = false;

            }
        }
        else if (roundedLaserLength <= 0.42f)
        {
            pillarRenderer.material.color = laserLengthColors[1];
            allowSound = true;
        }
        else if (roundedLaserLength <= 0.50f)
        {
            pillarRenderer.material.color = laserLengthColors[2];
        }
        else if (roundedLaserLength <= 0.56f)
        {
            pillarRenderer.material.color = laserLengthColors[3];
        }
        else
        {
            // Optional: Handle cases where the laser length is beyond the specified ranges
            Debug.Log("Laser length is beyond the specified ranges. Color will not change.");
        }
    }

}
