using UnityEngine;

public class PlayerWallnutMiniGame : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;      // empty child in front of the player
    public float attackRange = 0.8f;
    public LayerMask hittableLayers;

    bool isAttacking;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            isAttacking = true;
            animator.Play("Hit");
        }
    }

    // Animation Event on the hit frame
    public void CheckHit()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position, attackRange, hittableLayers);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Wallnut breakable))
                breakable.Break();
        }
    }

    // Animation Event on the last frame
    public void EndAttack()
    {
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
