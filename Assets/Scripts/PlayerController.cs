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
    //[SerializeField] private float obstacleCheckRadius = 0.4f;

    [Header("Trail History")]
    [Tooltip("How many past tiles to remember. Needs to be at least as many as you have followers.")]
    [SerializeField] private int maxHistoryLength = 4;

    [Header("Interaction")]
    [SerializeField] private LayerMask interactableLayer;
    private IInteractable currentInteractable;

    [SerializeField] private GameObject Inventory;
    private bool InventoryOpen;

    public List<Vector3> TileHistory { get; } = new List<Vector3>();
    public bool IsMoving { get; private set; }
    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    private Vector3 targetPosition;
    private Vector2 pendingInput;

    private ContactFilter2D obstacleFilter;
    private readonly Collider2D[] obstacleHits = new Collider2D[4];

    void Awake()
    {
        obstacleFilter = new ContactFilter2D();
        obstacleFilter.SetLayerMask(obstacleLayer);
        obstacleFilter.useTriggers = false; // triggers no longer block movement
    }

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

        UpdateInteractable();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!IsMoving && currentInteractable != null)
                currentInteractable.Interact();
            else
            {
                if (InventoryOpen)
                {
                    Inventory.SetActive(false);
                    InventoryOpen = false;
                }
                else
                {
                    Inventory.SetActive(true);
                    InventoryOpen = true;
                }
            }
        }
    }

    private void UpdateInteractable()
    {
        IInteractable found = null;

        // Only look for interactables while standing still
        if (!IsMoving)
        {
            Vector3 front = SnapToGrid(transform.position) + (Vector3)(FacingDirection * tileSize);
            Collider2D hit = Physics2D.OverlapBox(front, Vector2.one * tileSize * 0.5f, 0f, interactableLayer);

            if (hit != null)
                hit.TryGetComponent(out found);
        }

        if (found == currentInteractable) return;

        currentInteractable?.SetPromptVisible(false);
        currentInteractable = found;
        currentInteractable?.SetPromptVisible(true);
    }

    void FixedUpdate()
    {
        if (IsMoving)
            StepMove();
        if (Input.GetKeyDown(KeyCode.E))
            TryInteract();
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

        // Build the destination from a snapped position so errors never accumulate
        Vector3 start = SnapToGrid(transform.position);
        Vector3 destination = SnapToGrid(start + (Vector3)(direction * tileSize));

        if (checkObstacles)
        {
            int count = Physics2D.OverlapBox(
                destination, Vector2.one * tileSize * 0.5f, 0f, obstacleFilter, obstacleHits);

            if (count > 0)
            {
                Debug.Log($"Blocked at {destination} by '{obstacleHits[0].name}', bounds: {obstacleHits[0].bounds}");
                return; // blocked by a solid collider
            }
        }

        TileHistory.Insert(0, start);
        if (TileHistory.Count > maxHistoryLength)
            TileHistory.RemoveAt(TileHistory.Count - 1);

        targetPosition = destination;
        IsMoving = true;
    }

    private void StepMove()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, targetPosition, moveSpeed * tileSize * Time.deltaTime);

        // Vector3 == is approximate, so compare distance and snap exactly
        if ((transform.position - targetPosition).sqrMagnitude < 0.0000001f)
        {
            transform.position = targetPosition; // exact, no leftover error
            IsMoving = false;
        }
    }

    private Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(
            Mathf.Floor(pos.x / tileSize) * tileSize + tileSize * 0.5f,
            Mathf.Floor(pos.y / tileSize) * tileSize + tileSize * 0.5f,
            pos.z
        );
    }

    private void TryInteract()
    {
        Vector3 front = SnapToGrid(transform.position + (Vector3)(FacingDirection * tileSize));
        Collider2D hit = Physics2D.OverlapBox(front, Vector2.one * tileSize * 0.5f, 0f, interactableLayer);

        if (hit != null && hit.TryGetComponent(out IInteractable interactable))
            interactable.Interact();
    }

  
}