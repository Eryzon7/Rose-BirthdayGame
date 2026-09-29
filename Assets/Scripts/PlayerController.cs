using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface implemented by anything that can be "followed" in a trail
/// (the player, or another follower further up the chain).
/// </summary>
public interface IGridTrailable
{
    /// <summary>
    /// A history of grid tile positions, newest first. Entries are only
    /// added when a tile-step actually happens (not every frame).
    /// Index 0 = the tile just left. Index 1 = the tile before that, etc.
    /// </summary>
    List<Vector3> TileHistory { get; }
}

/// <summary>
/// Moves the player one tile at a time, Omori/Pokemon-style.
/// Attach to the player object. Requires a Collider2D if using obstacle checks.
/// </summary>
public class PlayerController : MonoBehaviour, IGridTrailable
{
    [Header("Grid Settings")]
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float moveSpeed = 5f; // tiles per second

    [Header("Collision (optional)")]
    [SerializeField] private bool checkObstacles = true;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float obstacleCheckRadius = 0.4f;

    [Header("Trail History")]
    [Tooltip("How many past tiles to remember. Needs to be at least as many as you have followers.")]
    [SerializeField] private int maxHistoryLength = 4;

    public List<Vector3> TileHistory { get; } = new List<Vector3>();
    public bool IsMoving { get; private set; }
    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    private Vector3 targetPosition;
    private Vector2 pendingInput;

    void Start()
    {
        // Snap to grid on spawn just in case
        transform.position = SnapToGrid(transform.position);
        targetPosition = transform.position;

        // Prefill history so followers have somewhere valid to sit before the first move
        for (int i = 0; i < maxHistoryLength; i++)
            TileHistory.Add(transform.position);
    }

    void Update()
    {
        if (!IsMoving)
        {
            ReadInput();
            if (pendingInput != Vector2.zero)
                TryStartMove(pendingInput);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //open inventory
            
        }
    }

    void FixedUpdate()
    {
        if (IsMoving)
            StepMove();
    }

    private void ReadInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Prioritize one axis at a time so movement stays grid-locked (no diagonals)
        if (Mathf.Abs(h) > Mathf.Abs(v))
            pendingInput = new Vector2(Mathf.Sign(h), 0f);
        else if (v != 0f)
            pendingInput = new Vector2(0f, Mathf.Sign(v));
        else
            pendingInput = Vector2.zero;
    }

    private void TryStartMove(Vector2 direction)
    {
        FacingDirection = direction;

        Vector3 destination = transform.position + (Vector3)(direction * tileSize);

        if (checkObstacles && Physics2D.OverlapCircle(destination, obstacleCheckRadius, obstacleLayer))
            return; // blocked, stay put (but still faces that direction)

        // Record the tile we're about to leave BEFORE moving, so followers
        // can move onto it right away.
        TileHistory.Insert(0, transform.position);
        if (TileHistory.Count > maxHistoryLength)
            TileHistory.RemoveAt(TileHistory.Count - 1);

        targetPosition = destination;
        IsMoving = true;
    }

    private void StepMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * tileSize * Time.fixedDeltaTime);

        if (transform.position == targetPosition)
            IsMoving = false;
    }

    private Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(
            Mathf.Round(pos.x / tileSize) * tileSize,
            Mathf.Round(pos.y / tileSize) * tileSize,
            pos.z
        );
    }
}