using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes this object trail behind a leader (the player or another follower),
/// stepping onto the exact tile the leader just left. Chain multiple of these
/// together for a party line, like Omori/Pokemon follower mechanics.
/// </summary>
public class FollowerMovement : MonoBehaviour, IGridTrailable
{
    [Header("Who to follow")]
    [SerializeField] private MonoBehaviour leaderSource; // must implement IGridTrailable
    private IGridTrailable leader;

    [Header("Follow Settings")]
    [Tooltip("How many whole tiles behind the leader this follower should stay. 1 = right behind it.")]
    [SerializeField] private int tilesBehind = 1;

    [Tooltip("If true, snaps instantly to its tile. If false, slides there over moveSpeed. Turn this OFF if you want walking animations.")]
    [SerializeField] private bool snapInstantly = true;
    [Tooltip("Only used if snapInstantly is false. Set this to the player's moveSpeed * tileSize so the follower keeps up.")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Trail History (for chaining more followers)")]
    [Tooltip("How many past tiles to remember. Needs to be at least as many followers as will chain off this one.")]
    [SerializeField] private int maxHistoryLength = 4;

    public List<Vector3> TileHistory { get; } = new List<Vector3>();

    // Read by FollowerAnimator
    public bool IsMoving { get; private set; }
    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    private Vector3? lastTarget;
    private bool initialized;

    void Awake()
    {
        if (leaderSource is IGridTrailable trailable)
            leader = trailable;
        else
            Debug.LogError($"{nameof(FollowerMovement)} on {name}: leaderSource must implement IGridTrailable.");
    }

    void FixedUpdate()
    {
        if (leader == null || leader.TileHistory.Count == 0) return;

        int index = Mathf.Min(tilesBehind - 1, leader.TileHistory.Count - 1);
        Vector3 target = leader.TileHistory[index];

        // On the very first tick, snap straight to the correct tile instead of
        // trusting wherever this object happened to be placed in the editor.
        // This guarantees no overlap regardless of starting position.
        if (!initialized)
        {
            transform.position = target;
            for (int i = 0; i < maxHistoryLength; i++)
                TileHistory.Add(target);

            lastTarget = target;
            initialized = true;
            return;
        }

        // Only act when the target tile actually changes (i.e. the leader took a step)
        if (target != lastTarget.Value)
        {
            lastTarget = target;

            // Face the direction we are about to walk (before we move)
            UpdateFacing(target - transform.position);

            // Record the tile we're leaving before moving, same as the leader does
            TileHistory.Insert(0, transform.position);
            if (TileHistory.Count > maxHistoryLength)
                TileHistory.RemoveAt(TileHistory.Count - 1);
        }

        if (snapInstantly)
        {
            transform.position = target;
            IsMoving = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);
            IsMoving = (transform.position - target).sqrMagnitude > 0.0000001f;
        }
    }

    private void UpdateFacing(Vector3 delta)
    {
        if (delta.sqrMagnitude < 0.0000001f) return; // no movement, keep current facing

        // Snap to a cardinal direction (grid movement has no diagonals)
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            FacingDirection = new Vector2(Mathf.Sign(delta.x), 0f);
        else
            FacingDirection = new Vector2(0f, Mathf.Sign(delta.y));
    }
}