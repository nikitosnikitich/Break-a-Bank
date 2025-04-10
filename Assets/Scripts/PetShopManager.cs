using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetShopManager : MonoBehaviour
{
    [SerializeField] private GameObject Instruction;
    [SerializeField] private GameObject Dialogue;
    [SerializeField] private GameObject petShopConvo;
    [SerializeField] private NPCConversation petShopConvoScript;
    [SerializeField] private PetShopPanel petShopPanel;
    [SerializeField] private PriceCalculation priceCalculation;

    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        ConversationManager.Instance.SetBool("accountCreated", false);

        col = GetComponent<Collider2D>();

        Instruction.SetActive(false);
        Dialogue.SetActive(false);
        petShopConvo.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (petShopPanel.petOwned == true)
        {
            ConversationManager.Instance.SetBool("petOwned", true);
        }

        ConversationManager.Instance.SetInt("money", (int)priceCalculation.money);
    }

    private void Awake()
    {
        priceCalculation = GameObject.Find("PriceManager").GetComponent<PriceCalculation>();   
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
                ConversationManager.Instance.StartConversation(petShopConvoScript);
                Dialogue.SetActive(true);
                Instruction.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Instruction.SetActive(false);
        petShopConvo.SetActive(false);
        ConversationManager.Instance.EndConversation();
    }
}
