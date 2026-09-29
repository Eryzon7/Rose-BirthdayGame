using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueBox dialogueBox;

    private DialogueData currentDialogue;
    private int currentLine;

    private void Update()
    {
        if (currentDialogue != null && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLine = 0;

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = currentDialogue.lines[currentLine];

        dialogueBox.SetLine(
            line.characterName,
            line.characterIcon,
            line.text
        );
    }

    public void NextLine()
    {
        currentLine++;

        if (currentLine >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();
    }

    private void EndDialogue()
    {
        dialogueBox.Hide();
        currentDialogue = null;
    }
}