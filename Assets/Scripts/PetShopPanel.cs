using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEditor.PackageManager;
using UnityEngine.UI;
using System.Data.SqlTypes;

public class PetShopPanel : MonoBehaviour
{
    private GameObject tempText;
    private int randomIndex;

    [Header("UI")]
    [SerializeField] private GameObject petShopPanel;
    [SerializeField] private GameObject dogAdoptionPanel;
    [SerializeField] private GameObject catAdoptionPanel;
    [SerializeField] private GameObject dogNameInputText;
    [SerializeField] private GameObject catNameInputText;
    [SerializeField] private GameObject prefabText;

    [SerializeField] private DogScript dog;
    [SerializeField] private CatScript cat;
    [SerializeField] private PriceCalculation priceCalculation;

    [SerializeField] private GameObject player;

    public string petName;
    public bool petOwned;

    private List<string> randDogName = new List<string>();
    private List<string> randCatName = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        LoadCatNamesFromFile();
        LoadDogNamesFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdoptPet()
    {
        if (dogAdoptionPanel.activeSelf)
        {
            dog.DogActivation();
            ReadInputDogName();
            CloseTab();
            petOwned = true;
        }
        else if (catAdoptionPanel.activeSelf)
        {
            cat.CatActivation();
            ReadInputCatName();
            CloseTab();
            petOwned = true;
        }

        priceCalculation.money -= 1000;
    }

    private void ReadInputDogName()
    {
        petName = dogNameInputText.GetComponent<InputField>().text;
    }

    private void ReadInputCatName()
    {
        petName = catNameInputText.GetComponent<InputField>().text;
    }

    private void LoadDogNamesFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "DogNames.txt");
        string[] lines = File.ReadAllLines(filePath);
        randDogName.AddRange(lines);
    }

    private void LoadCatNamesFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "CatNames.txt");
        string[] lines = File.ReadAllLines(filePath);
        randCatName.AddRange(lines);
    }

    public void ShowRandomDogName(string name)
    {
        randomIndex = Random.Range(0, randDogName.Count);
        tempText = Instantiate(prefabText);
        tempText.transform.SetParent(dogNameInputText.transform);
        dogNameInputText.GetComponent<InputField>().text = randDogName[randomIndex];
    }

    public void ShowRandomCatName(string name)
    {
        randomIndex = Random.Range(0, randCatName.Count);
        tempText = Instantiate(prefabText);
        tempText.transform.SetParent(catNameInputText.transform);
        catNameInputText.GetComponent<InputField>().text = randCatName[randomIndex];
    }
    public void CloseTab()
    {
        petShopPanel.SetActive(false);
    }

    public void OpenDogAdoptionPanel()
    {
        petShopPanel.SetActive(true);
        dogAdoptionPanel.SetActive(true);
        catAdoptionPanel.SetActive(false);
    }

    public void OpenCatAdoptionPanel()
    {
        petShopPanel.SetActive(true);
        catAdoptionPanel.SetActive(true);
        dogAdoptionPanel.SetActive(false);
    }
}
