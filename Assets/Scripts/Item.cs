using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public Inventory Inventory;
    public string ItemName;
    public Sprite ItemSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ItemSprite;
    }


    public void Interact()
    {
        Inventory.SlotInItem(this);
        Destroy(gameObject);
    }

    public void SetPromptVisible(bool visible)
    {
       
    }
}
