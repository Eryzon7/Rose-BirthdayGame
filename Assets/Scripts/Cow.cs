using System.Collections;
using UnityEngine;

public class Cow : MonoBehaviour, IInteractable
{
    [Header("Grid")]
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float moveSpeed = 2f; // tiles per second

    [Header("Collision")]
    [SerializeField] private LayerMask obstacleLayer; // walls, player, other NPCs, items...

    [Header("Wandering")]
    [SerializeField] private Vector2 idleTimeRange = new Vector2(1f, 4f);
    [SerializeField] private int minSteps = 1;
    [SerializeField] private int maxSteps = 3;
    [Tooltip("Max tiles away from the start tile. 0 = unlimited.")]
    [SerializeField] private int roamRadius = 5;

    [SerializeField] private Inventory inventory;

    private SpriteRenderer spriteRenderer;


    private static readonly Vector2[] Directions =
        { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private ContactFilter2D filter;
    private readonly Collider2D[] hits = new Collider2D[4];
    private Vector3 homePosition;
    private Collider2D ownCollider;

    public Vector2 FacingDirection { get; private set; } = Vector2.down;
    public bool IsMoving { get; private set; }

    void Awake()
    {
        filter = new ContactFilter2D();
        filter.SetLayerMask(obstacleLayer);
        filter.useTriggers = false;
        ownCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        transform.position = SnapToGrid(transform.position);
        homePosition = transform.position;
        StartCoroutine(WanderRoutine());
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            // Stand around for a bit
            yield return new WaitForSeconds(Random.Range(idleTimeRange.x, idleTimeRange.y));

            // Pick a direction and walk a few tiles that way
            Vector2 dir = Directions[Random.Range(0, Directions.Length)];
            int steps = Random.Range(minSteps, maxSteps + 1);

            for (int i = 0; i < steps; i++)
            {
                if (!TryGetDestination(dir, out Vector3 destination))
                    break; // blocked, go back to idling

                yield return MoveTo(destination);
            }
        }
    }

    private bool TryGetDestination(Vector2 dir, out Vector3 destination)
    {
        FacingDirection = dir;
        if (dir.x != 0f)
            spriteRenderer.flipX = dir.x < 0f; // left = flipped, right = normal
        destination = SnapToGrid(transform.position + (Vector3)(dir * tileSize));

        // Stay inside the roaming area
        if (roamRadius > 0)
        {
            Vector3 offset = (destination - homePosition) / tileSize;
            if (Mathf.Abs(offset.x) > roamRadius || Mathf.Abs(offset.y) > roamRadius)
                return false;
        }

        // Temporarily ignore our own collider so we don't block ourselves
        int count = Physics2D.OverlapBox(destination, Vector2.one * tileSize * 0.5f, 0f, filter, hits);
        for (int i = 0; i < count; i++)
            if (hits[i] != ownCollider)
                return false;

        return true;
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        IsMoving = true;

        while ((transform.position - destination).sqrMagnitude > 0.0000001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, destination, moveSpeed * tileSize * Time.deltaTime);
            yield return null;
        }

        transform.position = destination; // exact, no drift
        IsMoving = false;
    }

    private Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(
            Mathf.Floor(pos.x / tileSize) * tileSize + tileSize * 0.5f,
            Mathf.Floor(pos.y / tileSize) * tileSize + tileSize * 0.5f,
            pos.z);
    }

    public void Interact(Vector2 facing)
    {
        inventory
    }
    public void SetPromptVisible(bool visible)
    {

    }
}