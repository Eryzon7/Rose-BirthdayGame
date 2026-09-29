using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}