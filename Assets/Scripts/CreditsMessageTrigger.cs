using UnityEngine;

public class CreditsMessageTrigger : MonoBehaviour
{
    public CreditsMessage dialogue;
    private bool inDialogue = false;

    public void TriggerDialogue()
    {
        FindObjectOfType<CreditsMessageManager>().StartDialogue(dialogue);
        inDialogue = true;
    }

    void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update() 
    {
        if(!inDialogue)
        {
            TriggerDialogue();
        }
    }
}