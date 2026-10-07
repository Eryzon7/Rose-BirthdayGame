using UnityEngine;

public interface IInteractable
{
    void Interact(Vector2 facing);
    void SetPromptVisible(bool visible);
}