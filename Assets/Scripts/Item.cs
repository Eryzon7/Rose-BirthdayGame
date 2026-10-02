using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public Inventory Inventory;
    public ItemData Data;

    void Awake()
    {
        if (Data != null && TryGetComponent(out SpriteRenderer sr))
            sr.sprite = Data.icon;
    }

    public void Interact()
    {
        Inventory.SlotInItem(Data);
        Destroy(gameObject);
    }

    public void SetPromptVisible(bool visible) { }
}