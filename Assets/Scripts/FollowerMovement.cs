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

    [Tooltip("If true, snaps instantly to its tile. If false, slides there over moveSpeed.")]
    [SerializeField] private bool snapInstantly = true;
    [SerializeField] private float moveSpeed = 5f; // only used if snapInstantly is false

    [Header("Trail History (for chaining more followers)")]
    [Tooltip("How many past tiles to remember. Needs to be at least as many followers as will chain off this one.")]
    [SerializeField] private int maxHistoryLength = 4;

    public List<Vector3> TileHistory { get; } = new List<Vector3>();

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

            // Record the tile we're leaving before moving, same as the leader does
            TileHistory.Insert(0, transform.position);
            if (TileHistory.Count > maxHistoryLength)
                TileHistory.RemoveAt(TileHistory.Count - 1);
        }

        if (snapInstantly)
            transform.position = target;
        else
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);
    }
}