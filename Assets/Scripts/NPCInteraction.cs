using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private DialogueManager manager;
    [SerializeField] private DialogueData dialogue;
    [SerializeField] private GameObject InteractionText;

    private bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            manager.StartDialogue(dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionText.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionText.SetActive(false);
            playerInRange = false;
        }
    }
}