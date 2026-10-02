using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;

public class Inventory : MonoBehaviour
{
    public ItemData[] InventorySlots = new ItemData[16];
    public UnityEngine.UI.Image[] InventorySlotsUI = new UnityEngine.UI.Image[16];
    public GameObject PopUpConsumable;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private Canvas canvas;

    private GameObject currentPopup;



    [SerializeField] private GameObject player; // who "uses" the items

    public bool SlotInItem(ItemData data)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i] == null)
            {
                InventorySlots[i] = data;
                InventoryUIUpdate(i);
                Debug.Log($"Picked up {data.itemName} in slot {i}");
                return true;
            }
        }

        Debug.Log("Inventory full");
        return false; // nothing was added
    }

    private void ShowPopupAtMouse(GameObject prefab)
    {
        if (currentPopup != null) Destroy(currentPopup);

        currentPopup = Instantiate(prefab, canvasTransform);
        RectTransform popupRect = currentPopup.GetComponent<RectTransform>();

        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        Vector2 mousePos = Mouse.current.position.ReadValue();

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasTransform, mousePos, cam, out Vector3 worldPoint);

        popupRect.pivot = new Vector2(0f, 1f);   // top-left corner sits on the cursor
        

        popupRect.position = worldPoint; // anchors no longer matter
        popupRect.SetAsLastSibling();
    }
    public void SelectItem(int slotNumber)
    {
        Console.WriteLine("buttonWorks");
        ItemData data = InventorySlots[slotNumber];
        if (data == null) return;

        if (data is EquipableData)
        {
            // show: Equip - Info - Cancel
        }
        else if (data is ConsumableData)
        {
            ShowPopupAtMouse(PopUpConsumable);

        }
    }

    public void UseSlot(int slotNumber)
    {
        ItemData data = InventorySlots[slotNumber];
        if (data == null) return;

        if (data.Use(player))
            RemoveItem(slotNumber); // consumed
    }

    public void RemoveItem(int slotNumber)
    {
        UnityEngine.UI.Image img = InventorySlotsUI[slotNumber];
        Color currentColor = img.color;
        currentColor.a = 0; // Value between 0.0 (transparent) and 1.0 (opaque)
        img.color = currentColor;
        InventorySlots[slotNumber] = null;
        InventoryUIUpdate(slotNumber);
    }

    private void InventoryUIUpdate(int slot)
    {   

        ItemData data = InventorySlots[slot];
        UnityEngine.UI.Image img = InventorySlotsUI[slot];

        Color currentColor = img.color;
        currentColor.a = 1; // Value between 0.0 (transparent) and 1.0 (opaque)
        img.color = currentColor;

        img.sprite = data != null ? data.icon : null;
        img.enabled = data != null; // hide empty slots
    }    
}