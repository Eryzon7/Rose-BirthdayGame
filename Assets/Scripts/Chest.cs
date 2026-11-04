using System;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject InteractionText;
    [SerializeField] private Item item;
    [SerializeField] private Inventory inventory;



    public void SetPromptVisible(bool visible)
    {
        InteractionText.SetActive(visible);
    }

    public void Interact(Vector2 facing)
    {
        inventory.SlotInItem(item.Data);
        Debug.Log("give item");
       //give item
    }
}
