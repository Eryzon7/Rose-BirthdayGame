using System;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject InteractionText;
    [SerializeField] private Item item;
    [SerializeField] private Inventory inventory;
    [SerializeField] private string clip = "";
    [SerializeField] private DialogueManager manager;
    [SerializeField] private DialogueData dialogue;



    public void SetPromptVisible(bool visible)
    {
        InteractionText.SetActive(visible);
    }

    public void Interact(Vector2 facing)
    {
        Debug.Log("run animation");
        animator.Play(clip);
        inventory.SlotInItem(item.Data);
        Debug.Log("give item");
        manager.StartDialogue(dialogue);
        this.gameObject.SetActive(false);   
       //give item
    }
}
