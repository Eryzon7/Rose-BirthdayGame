using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable")]
public class ConsumableData : ItemData
{
    public int healAmount;

    public override bool Use(GameObject user)
    {
        // Replace with your own health system
        Debug.Log($"Used {itemName}, healed {healAmount}");
        return true; // consumed, remove from inventory
    }
}