using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private TMP_Text useButtonLabel; // text on the Use/Equip button (optional)

    private int slotNumber;
    private ItemData data;

    private RectTransform Rect => (RectTransform)transform;

    public void Show(int slotNumber, ItemData data)
    {
        this.slotNumber = slotNumber;
        this.data = data;

        if (useButtonLabel != null)
            useButtonLabel.text = data is EquipableData ? "Equip" : "Use";

        // Fixed anchors and pivot so the prefab layout can't shift it
        Rect.anchorMin = Rect.anchorMax = new Vector2(0.5f, 0.5f);
        Rect.pivot = new Vector2(0f, 1f);
        Rect.sizeDelta = new Vector2(200f, 150f); // width, height of your popup

        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePos, cam, out Vector2 local))
            Rect.localPosition = local;

        gameObject.SetActive(true);
        transform.SetAsLastSibling(); // draw on top
    }

    public void Hide() => gameObject.SetActive(false);

    // Button hooks
    public void OnUse()
    {
        inventory.UseSlot(slotNumber);
        Hide();
    }

    public void OnInfo()
    {
        Debug.Log($"{data.itemName}: {data.description}");
    }

    public void OnCancel() => Hide();
}