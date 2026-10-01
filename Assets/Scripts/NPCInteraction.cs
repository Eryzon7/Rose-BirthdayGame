using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueManager manager;
    [SerializeField] private DialogueData dialogue;
    [SerializeField] private GameObject InteractionText;

    public void SetPromptVisible(bool visible)
    {   
        InteractionText.SetActive(visible);
    }

    public void Interact()
    {
        manager.StartDialogue(dialogue);
    }

}