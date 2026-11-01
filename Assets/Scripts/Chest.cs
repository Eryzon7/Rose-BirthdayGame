using System;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject InteractionText;
    [SerializeField] private GameObject Item;


    public void SetPromptVisible(bool visible)
    {
        InteractionText.SetActive(visible);
    }

    public void Interact(Vector2 facing)
    {
        Debug.Log("give item");
       //give item
    }
}
