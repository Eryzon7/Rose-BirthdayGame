using System;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject InteractionText;
    [SerializeField] private Item item;
    [SerializeField] private Inventory inventory;
    [SerializeField] private string clip = "";



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
        this.gameObject.SetActive(false);   
       //give item
    }
}
