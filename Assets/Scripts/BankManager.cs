using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class BankManager : MonoBehaviour
{
    [SerializeField] private GameObject Instruction;
    [SerializeField] private GameObject Dialogue;
    [SerializeField] private GameObject bankConvo;
    [SerializeField] private NPCConversation bankConversation;
    [SerializeField] private BankAccount bankAccount;

    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        ConversationManager.Instance.SetBool("accountCreated", false);

        col = GetComponent<Collider2D>();

        Instruction.SetActive(false);
        Dialogue.SetActive(false);
        bankConvo.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
       if (bankAccount.accountCreated == true)
        {
            ConversationManager.Instance.SetBool("accountCreated", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Instruction.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {
                Dialogue.SetActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        { 
            if (Input.GetKey(KeyCode.E))
            {
                ConversationManager.Instance.StartConversation(bankConversation);
                Dialogue.SetActive(true);
                Instruction.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Instruction.SetActive(false);
        bankConvo.SetActive(false);
        ConversationManager.Instance.EndConversation();
    }
}
