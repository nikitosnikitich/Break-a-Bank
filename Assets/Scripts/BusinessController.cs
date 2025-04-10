using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusinessController : MonoBehaviour
{
    [Header("UI")]
    [Space]
    [SerializeField] private GameObject businessPanel;
    [SerializeField] private GameObject businessManagementPanel;

    [SerializeField] private GameObject businessCreationPanel;
    [SerializeField] private InputField businessNameInput;
    [SerializeField] private Text businessNameOutput;


    [SerializeField] private Text businessLevelText; 
    [SerializeField] private Text businessIncomeText;
    [SerializeField] private Text businessUpgradeCostText;

    [SerializeField] private GameObject upgradePanel;
    [Space]

    private bool isBusinessCreated = false;

    [SerializeField] private int businessIncome;

    private string businessName; 
    [SerializeField] private int businessLevel;             //Рівень розвитку бізнесу
    [SerializeField] private int[] businessUpgradeCost;     //Ціна покращення(унікальна для кожного рівня)
    [SerializeField] private int[] businessIncomePerLevel;  //Прибуток від бізнесу на кожному рівні.

    private PriceCalculation priceController;
    [SerializeField] private GameObject priceControllerHolder;

    public static BusinessController InstanceBusiness;

    void Awake()
    {
        priceController = priceControllerHolder.GetComponent<PriceCalculation>();
        businessIncome = businessIncomePerLevel[businessLevel];

        InstanceBusiness = this;
    }

    public void EndBusinessTurn()
    {
        priceController.money += businessIncome * System.Convert.ToInt32(isBusinessCreated);
    }

    public void UpgradeBusiness()
    {
        if(priceController.money >= businessUpgradeCost[businessLevel])
        {
            priceController.money -= businessUpgradeCost[businessLevel];
            businessLevel++;
            businessIncome = businessIncomePerLevel[businessLevel];
            if(businessLevel == 9)
            {
                upgradePanel.SetActive(false);
            }
        }
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        businessLevelText.text = (businessLevel + 1).ToString();
        businessIncomeText.text = businessIncome.ToString();
        businessUpgradeCostText.text = priceController.ShorterNumber(businessUpgradeCost[businessLevel], 1);

        businessNameOutput.text = businessName;

        // if(Input.GetKeyDown(KeyCode.O))
        // {
        //     if(businessPanel.activeSelf)
        //     {
        //         businessPanel.SetActive(false);
        //     }
        //     else
        //     {
        //         businessPanel.SetActive(true);
        //     }
        // }
    }

    public void CreateBusiness()
    {
        if(priceController.money >= 500 && !string.IsNullOrEmpty(businessNameInput.text))
        {
            priceController.money -= 500;
            isBusinessCreated = true;
            businessCreationPanel.SetActive(false);
            businessManagementPanel.SetActive(true);

            businessName = businessNameInput.text;
        }
    }

    public void CloseBusinessPanel()
    {
        businessPanel.SetActive(false);
    }
    public void OpenBusinessPanel()
    {
        businessPanel.SetActive(true);
    }
}
