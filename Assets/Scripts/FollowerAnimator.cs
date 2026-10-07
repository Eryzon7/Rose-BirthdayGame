using UnityEngine;

/// <summary>
/// Drives a follower's Animator from FollowerMovement.
/// Attach to the same object as FollowerMovement.
///
/// Uses the same Animator parameters as the player:
///   Float "MoveX", Float "MoveY", Bool "IsMoving"
/// </summary>
[RequireComponent(typeof(FollowerMovement))]
public class FollowerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Tooltip("Keeps the walk animation playing for this long after the follower stops. " +
             "Prevents a one-frame idle flicker between steps while the player keeps walking.")]
    [SerializeField] private float stopDelay = 0.1f;

    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

    private FollowerMovement follower;
    private float lastMovingTime = float.NegativeInfinity;

    private void Awake()
    {
        follower = GetComponent<FollowerMovement>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void LateUpdate()
    {
        if (animator == null) return;

        if (follower.IsMoving)
            lastMovingTime = Time.time;

        bool showMoving = Time.time - lastMovingTime <= stopDelay;

        Vector2 facing = follower.FacingDirection;
        animator.SetFloat(MoveXHash, facing.x);
        animator.SetFloat(MoveYHash, facing.y);
        animator.SetBool(IsMovingHash, showMoving);
    }
}