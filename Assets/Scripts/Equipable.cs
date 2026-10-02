using UnityEngine;

public enum EquipSlot { Head, Body, Weapon, Accessory }

[CreateAssetMenu(menuName = "Items/Equipable")]
public class EquipableData : ItemData
{
    public EquipSlot slot;
    public int attackBonus;
    public int defenseBonus;

    public override bool Use(GameObject user)
    {
        // Replace with your own equipment system, e.g.:
        // user.GetComponent<Equipment>().Equip(this);
        Debug.Log($"Equipped {itemName} in {slot}");
        return false; // stays in inventory (or moves to an equipment slot)
    }
}