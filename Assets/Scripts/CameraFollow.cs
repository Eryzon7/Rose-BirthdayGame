using UnityEngine;

/// <summary>
/// Smoothly makes a 2D camera follow a target (e.g. the player).
/// Attach this to your Main Camera and drag the Player into the "Target" slot.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [Tooltip("Higher = snappier, lower = smoother/laggier.")]
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private Vector2 offset = Vector2.zero;

    [Header("Optional World Bounds")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds = new Vector2(-50f, -50f);
    [SerializeField] private Vector2 maxBounds = new Vector2(50f, 50f);

    private float zPos;

    private void Awake()
    {
        // Keep the camera's original Z so the 2D scene stays visible.
        zPos = transform.position.z;
    }

    // LateUpdate runs after the player has moved this frame, which avoids jitter.
    private void LateUpdate()
    {
        if (target == null) return;

        Vector2 desired = (Vector2)target.position + offset;

        if (useBounds)
        {
            desired.x = Mathf.Clamp(desired.x, minBounds.x, maxBounds.x);
            desired.y = Mathf.Clamp(desired.y, minBounds.y, maxBounds.y);
        }

        // Frame-rate independent smoothing.
        float t = 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime);
        Vector2 smoothed = Vector2.Lerp(transform.position, desired, t);

        transform.position = new Vector3(smoothed.x, smoothed.y, zPos);
    }
}