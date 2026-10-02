using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;

    /// <summary>
    /// Called when the player uses the item from the inventory.
    /// Return true if the item should be removed afterwards.
    /// </summary>
    public abstract bool Use(GameObject user);
}