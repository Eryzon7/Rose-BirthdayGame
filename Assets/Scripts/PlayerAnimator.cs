using UnityEngine;

/// <summary>
/// Drives the player's Animator from PlayerController.
/// Attach to the same object as PlayerController and the Animator.
///
/// Animator parameters expected:
///   Float "MoveX"    -> facing direction X (-1, 0, 1)
///   Float "MoveY"    -> facing direction Y (-1, 0, 1)
///   Bool  "IsMoving" -> true while walking between tiles
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    // LateUpdate so we read IsMoving AFTER the controller has decided on this
    // frame's move. This avoids a one-frame idle flicker between tiles when
    // the player holds a direction.
    private void LateUpdate()
    {
        if (animator == null) return;

        Vector2 facing = player.FacingDirection;

        animator.SetFloat(MoveXHash, facing.x);
        animator.SetFloat(MoveYHash, facing.y);
        animator.SetBool(IsMovingHash, player.IsMoving);
    }
}