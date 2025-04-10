using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEditor.PackageManager;
using UnityEngine.UI;

public class BankAccount : MonoBehaviour
{
    public string firstName;
    public string lastName;
    public string pinCode;
    public string cardNumberAsString;
    private GameObject tempText;

    public bool accountCreated;

    private List<string> randFirstName = new List<string>();
    private List<string> randLastName = new List<string>();

    private int randomIndex;

    [Header("UI")]
    [SerializeField] GameObject functioningBankAccountUI;
    [SerializeField] GameObject createAccountPanel;
    [SerializeField] GameObject pinText;
    [SerializeField] GameObject firstNameText;
    [SerializeField] GameObject lastNameText;
    [SerializeField] GameObject firstNameInputText;
    [SerializeField] GameObject lastNameInputText;
    [SerializeField] GameObject firstNameError;
    [SerializeField] GameObject lastNameError;
    [SerializeField] GameObject prefabText;
    [SerializeField] GameObject cardNumberText;
    [SerializeField] GameObject changePINCodeUI;
    [SerializeField] GameObject changePINCodeButton;
    [SerializeField] GameObject pinCodeInputText;
    [SerializeField] GameObject pinCodeError;
    [SerializeField] GameObject bankAccountPanel;


    // Start is called before the first frame update
    void Start()
    {
        LoadFirstNamesFromFile();
        LoadLastNamesFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        if (!accountCreated)
        {
            functioningBankAccountUI.SetActive(false);
        }
    }

    public void CreateBankAccount()
    {
        if (firstNameInputText.GetComponent<InputField>().text == "" && lastNameInputText.GetComponent<InputField>().text != "")
        {
            firstNameError.GetComponent<Animator>().Play("PulsatingOpacity");
        }
        else if (lastNameInputText.GetComponent<InputField>().text == "" && firstNameInputText.GetComponent<InputField>().text != "")
        {
            lastNameError.GetComponent<Animator>().Play("PulsatingOpacity");
        }
        else if (firstNameInputText.GetComponent<InputField>().text == "" && lastNameInputText.GetComponent<InputField>().text == "")
        {
            firstNameError.GetComponent<Animator>().Play("PulsatingOpacity");
            lastNameError.GetComponent<Animator>().Play("PulsatingOpacity");
        }
        else 
        {
            ReadInputFirstName();
            ReadInputLastName();
            firstNameError.SetActive(false);
            lastNameError.SetActive(false);
            GenerateCardNumber();
            GeneratePINCode();
            functioningBankAccountUI.SetActive(true);
            createAccountPanel.SetActive(false);
            accountCreated = true;
            firstNameText.GetComponent<Text>().text = firstName;
            lastNameText.GetComponent<Text>().text = lastName;
        }
    }

    private void ReadInputFirstName()
    {
        firstName = firstNameInputText.GetComponent<InputField>().text;
    }

    private void ReadInputLastName()
    {
        lastName = lastNameInputText.GetComponent<InputField>().text;
    }

    private void LoadFirstNamesFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "FirstNames.txt");
        string[] lines = File.ReadAllLines(filePath);
        randFirstName.AddRange(lines);
    }

    private void LoadLastNamesFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "LastNames.txt");
        string[] lines = File.ReadAllLines(filePath);
        randLastName.AddRange(lines);
    }

    public void ShowRandomFirstName(string name)
    {
        randomIndex = Random.Range(0, randFirstName.Count);
        tempText = Instantiate(prefabText);
        tempText.transform.SetParent(firstNameInputText.transform);
        firstNameInputText.GetComponent<InputField>().text = randFirstName[randomIndex];
    }

    public void ShowRandomLastName(string name)
    {
        randomIndex = Random.Range(0, randLastName.Count);
        tempText = Instantiate(prefabText);
        tempText.transform.SetParent(lastNameInputText.transform);
        lastNameInputText.GetComponent<InputField>().text = randLastName[randomIndex];
    }

    private void GenerateCardNumber()
    {
        int cardNumber = Random.Range(9000, 9999);
        cardNumberAsString = cardNumber.ToString();
        cardNumber = Random.Range(1000, 9999);
        cardNumberAsString = cardNumberAsString + ' ' + cardNumber.ToString();
        cardNumber = Random.Range(1000, 9999);
        cardNumberAsString = cardNumberAsString + ' ' + cardNumber.ToString();
        cardNumber = Random.Range(1000, 9999);
        cardNumberAsString = cardNumberAsString + ' ' + cardNumber.ToString();
        cardNumberText.GetComponent<Text>().text = cardNumberAsString;
    }

    public void GeneratePINCode()
    {
        int PINCode = Random.Range(0, 10000);
        pinCode = PINCode.ToString().PadLeft(4 + PINCode.ToString().Length, '0');
        pinCode = pinCode.Substring(pinCode.Length - 4);
        pinText.GetComponent<Text>().text = pinCode;
        pinCodeInputText.GetComponent<InputField>().text = pinCode;
    }

    public void EnablePINCodeChangeUI()
    {
        changePINCodeUI.SetActive(true);
        changePINCodeButton.SetActive(false);
    }

    public void DisablePINCodeChangeUI()
    {
        if (pinCodeInputText.GetComponent<InputField>().text == "")
        {
            pinCodeError.GetComponent<Animator>().Play("PulsatingOpacity");
        }
        else
        {
            changePINCodeUI.SetActive(false);
            changePINCodeButton.SetActive(true);
            pinText.GetComponent<Text>().text = pinCodeInputText.GetComponent<InputField>().text;
        }
    }
    public void OpenAccountPanel()
    {
        bankAccountPanel.SetActive(true);
    }

    public void CloseAccountPanel()
    {
        bankAccountPanel.SetActive(false);
    }
}
