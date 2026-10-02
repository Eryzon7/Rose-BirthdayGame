using System;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Item[] InventorySlots;
    
    public void SlotInItem(Item pickUp)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            Console.WriteLine(i);
            if (InventorySlots[i] == null)
            {
                InventorySlots[i] = pickUp;
                Console.WriteLine(InventorySlots[i].name);
                return;
            }
        }
    }
}
