using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueManager manager;
    [SerializeField] private DialogueData dialogue;
    [SerializeField] private GameObject InteractionText;
    [SerializeField] private NpcIdle npcIdle;

    private void Awake()
    {
        // Fall back to finding it automatically if it wasn't assigned in the Inspector
        if (npcIdle == null)
            npcIdle = GetComponentInChildren<NpcIdle>();
    }

    public void SetPromptVisible(bool visible)
    {
        InteractionText.SetActive(visible);
    }

    /// <param name="facing">The direction the player is facing when they interact.</param>
    public void Interact(Vector2 facing)
    {
        if (npcIdle != null)
            npcIdle.SetFacing(FaceTowardPlayer(facing));

        manager.StartDialogue(dialogue);
    }

    /// <summary>
    /// The player is looking at the NPC, so the NPC should look the opposite way.
    /// Player faces up -> NPC faces down, player faces left -> NPC faces right, etc.
    /// </summary>
    private NpcIdle.Facing FaceTowardPlayer(Vector2 playerFacing)
    {
        if (Mathf.Abs(playerFacing.x) > Mathf.Abs(playerFacing.y))
            return playerFacing.x > 0 ? NpcIdle.Facing.Left : NpcIdle.Facing.Right;

        return playerFacing.y > 0 ? NpcIdle.Facing.Down : NpcIdle.Facing.Up;
    }
}