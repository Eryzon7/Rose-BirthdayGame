using UnityEngine;

/// <summary>
/// Sets a stationary NPC's facing direction once, so it plays the correct idle
/// animation. Uses the same Animator parameters as the player (MoveX, MoveY).
/// IsMoving is never set, so it stays false and the NPC stays on Idle.
/// </summary>
public class NpcIdle : MonoBehaviour
{
    public enum Facing { Down, Up, Left, Right }

    [SerializeField] private Animator animator;
    [SerializeField] private Facing facing = Facing.Down;

    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        SetFacing(facing);
    }

    /// <summary>Call this from other scripts (e.g. dialogue) to turn the NPC toward the player.</summary>
    public void SetFacing(Facing newFacing)
    {
        facing = newFacing;
        if (animator == null) return;

        Vector2 dir = facing switch
        {
            Facing.Up => Vector2.up,
            Facing.Left => Vector2.left,
            Facing.Right => Vector2.right,
            _ => Vector2.down
        };

        animator.SetFloat(MoveXHash, dir.x);
        animator.SetFloat(MoveYHash, dir.y);
    }
}