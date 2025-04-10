using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceCalculation : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject phonePanel;
    [SerializeField] private GameObject tradePanel;
    [SerializeField] private Text[] materialPricesText;
    [SerializeField] private Text[] materialAmountText;
    [SerializeField] private Text moneyAmountText;

    //Механіка купівлі і продажу(UI)
    [SerializeField] private GameObject buyingPanel;
    [SerializeField] private Slider priceSlider;
    [SerializeField] private Text maxAmountText;
    [SerializeField] private Text chosenText;

    [SerializeField] private GameObject sellingPanel;
    [SerializeField] private Slider priceSellingSlider;
    [SerializeField] private Text maxSellingAmountText;
    [SerializeField] private Text chosenSellingText;

    [SerializeField] private Slider tempRealTimeControllerSlider;


    [Header("Stabilizing prices")]
    [SerializeField] private float[] materialStablePrice;       //Стабільна(початкова) ціна товару
    [SerializeField] private float[] materialPriceModifier;     //Відношення ціни до стабільної ціни

    [SerializeField] private int[] materialMaxPercent;          //Максимальне значення % росту помножене на 10
    [SerializeField] private int[] materialMinPercent;

    [SerializeField] private float[] materialStabilityModifier; //Наскільки стабільний товар
    [SerializeField] private int[] materialRiseModifier;        //З кожним зростанням ціни ймовірність подальшого росту збільшується
    

    [Header("Gameplay variables")]
    public float money;
    public float[] materialPrices;
    public int[] materialPurchased;

    [SerializeField] private int[] objectNewsModfier;
    [SerializeField] private int[] objectNewsDuration;

    private int maxAmount;
    private int maxSellAmount;
    private int lastIndexUsed;
    
    [SerializeField] private float economyPriceModifier;        //Стан економіки, впливає на ріст і падіння цін

    [SerializeField] private string[] materialNames;

    [Header("Debug")]
    [SerializeField] private int turnsCount;

    [SerializeField] private Text timeText;

    void Start()
    {
        timeText.text = "Час x" + tempRealTimeControllerSlider.value.ToString();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EndWeek();
        }
        Temporary();
        UpdateUI();
    }

    private void Temporary()
    {
        // if(Input.GetKeyDown(KeyCode.I))
        // {
        //     if(tradePanel.activeSelf)
        //     {
        //         tradePanel.SetActive(false);
        //     }
        //     else
        //     {
        //         tradePanel.SetActive(true);
        //     }
        // }
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(phonePanel.activeSelf)
            {
                phonePanel.SetActive(false);
            }
            else
            {
                phonePanel.SetActive(true);
            }
        }
    }

    public void RealTimeChanged()
    {
        StartCoroutine("RealTime");
    }

    public void EndWeek()
    {
        turnsCount++;
        Destroy(GameObject.FindWithTag("NewsText"));
        //Debug.Log("Turns " + turnsCount);
        CalculatePrices();
        BusinessController.InstanceBusiness.EndBusinessTurn();
    }

    private void CalculatePrices()
    {
        //Обчислення і вивід цін на товари

        for(int i = 0; i<materialPrices.Length;i++)
        {
            materialPrices[i] = MaterialPricesCalculation(materialPrices[i],i);

            materialPrices[i] = materialPrices[i] * 100;
            materialPrices[i] = Mathf.Round(materialPrices[i]);
            materialPrices[i] = materialPrices[i] / 100;

            materialPricesText[i].text = ShorterNumber(materialPrices[i],2);
            UpdateNews(i);
        }
    }

    private float MaterialPricesCalculation(float matPrice, int materialIndex)
    {
        MaterialPercentRangeCalculation(materialIndex);

        float randomPriceModifier = Random.Range(materialMinPercent[materialIndex] * materialStabilityModifier[materialIndex],materialMaxPercent[materialIndex] * materialStabilityModifier[materialIndex]);
        randomPriceModifier = randomPriceModifier/1000;

        
        if(randomPriceModifier == 0) return matPrice;

        else if(randomPriceModifier > 0)
        {
            matPrice = matPrice + matPrice * (randomPriceModifier * economyPriceModifier * objectNewsModfier[materialIndex]);
            return matPrice;
        }
        else
        {
            matPrice = matPrice + matPrice * (randomPriceModifier / economyPriceModifier / objectNewsModfier[materialIndex]);
            return matPrice;
        }
    }

    private void MaterialPercentRangeCalculation(int index)
    {
        materialPriceModifier[index] = materialPrices[index]/materialStablePrice[index];    //Відношення поточної ціни до початкової(стабільної), використовується для стабілізації ціни.

        if(materialPriceModifier[index] >= 2)
        {
            materialMaxPercent[index] = 10;     //Максимальний і мінімальний відсоток помножений на 10
            materialMinPercent[index] = -80;
        }
        else if(materialPriceModifier[index] >= 1.90f)
        {
            materialMaxPercent[index] = 17;
            materialMinPercent[index] = -73;
        }
        else if(materialPriceModifier[index] >= 1.75f)
        {
            materialMaxPercent[index] = 25;
            materialMinPercent[index] = -65;
        }
        else if(materialPriceModifier[index] >= 1.5f)
        {
            materialMaxPercent[index] = 35;
            materialMinPercent[index] = -55;
        }
        else if(materialPriceModifier[index] >= 1.3f)
        {
            materialMaxPercent[index] = 40;
            materialMinPercent[index] = -50;
        }
        else if(materialPriceModifier[index] >= 0.7f)
        {
            materialMaxPercent[index] = 50;
            materialMinPercent[index] = -50;
        }
        else if(materialPriceModifier[index] >= 0.57f)
        {
            materialMaxPercent[index] = 65;
            materialMinPercent[index] = -40;
        }
        else if(materialPriceModifier[index] >= 0.4f)
        {
            materialMaxPercent[index] = 95;
            materialMinPercent[index] = -30;
        }
        else
        {
            materialMaxPercent[index] = 150;
            materialMinPercent[index] = 0;
        }
    }

    private void UpdateUI()
    {
        moneyAmountText.text = ShorterNumber(money,1);

        chosenText.text = priceSlider.value.ToString();
        chosenSellingText.text = priceSellingSlider.value.ToString();
        timeText.text = "Час x" + tempRealTimeControllerSlider.value.ToString();
    }

    public string ShorterNumber(float number,int numbersAfterComma)        //Скорочення чисел(10K, 500M, 9B, та ін.)
    {
        if(number >= Mathf.Pow(10,9))
        {
            number = Mathf.Round(number/Mathf.Pow(10,9-numbersAfterComma))/Mathf.Pow(10f,numbersAfterComma);    //Заокруглення числа визначається змінною numbersAfterComma
            return number.ToString() + "B";
        }
        else if(number >= Mathf.Pow(10,6))
        {
            number = Mathf.Round(number/Mathf.Pow(10,5-numbersAfterComma))/Mathf.Pow(10f,numbersAfterComma);
            return number.ToString() + "M";
        }
        else if(number >= 1000)
        {
            number = Mathf.Round(number/Mathf.Pow(10,3-numbersAfterComma))/Mathf.Pow(10f,numbersAfterComma);
            return number.ToString() + "K";
        }
        else if(number >= 25)
        {
            number = Mathf.Round(number);
            return number.ToString();
        }
        else
        {
            number = Mathf.Round(number * 100f)/100f;
            return number.ToString();
        }
    }

    // IEnumerator RealTime()
    // {
    //     while(tempRealTimeControllerSlider.value > 0)
    //     {
    //         EndWeek();
    //         yield return new WaitForSeconds(0.01f);
    //     }
    // }
    IEnumerator RealTime()
    {
        while(tempRealTimeControllerSlider.value > 0)
        {
            switch(tempRealTimeControllerSlider.value)
            {
                case 1:
                    EndWeek();
                    yield return new WaitForSeconds(0.5f);
                    break;
                case 2:
                    EndWeek();
                    yield return new WaitForSeconds(0.25f);
                    break;
                case 3:
                    EndWeek();
                    yield return new WaitForSeconds(0.01f);
                    break;
            }
        }
    }

    public void OpenBuyingPanel(int index)
    {
        if(!sellingPanel.activeSelf)
        {
            maxAmount = Mathf.RoundToInt(money)/Mathf.RoundToInt(materialPrices[index]);
            lastIndexUsed = index;

            buyingPanel.SetActive(true);
            priceSlider.value = 0;
            priceSlider.maxValue = maxAmount;
            maxAmountText.text = maxAmount.ToString();
        }
    }

    public void BuyItem()
    {
        materialPurchased[lastIndexUsed] += Mathf.RoundToInt(priceSlider.value);
        money = money - materialPrices[lastIndexUsed]*priceSlider.value;

        materialAmountText[lastIndexUsed].text = ShorterNumber(materialPurchased[lastIndexUsed],1);
        OpenBuyingPanel(lastIndexUsed);
    }

    public void CloseBuyingPanel()
    {
        buyingPanel.SetActive(false);
    }
    public void CloseSellingPanel()
    {
        sellingPanel.SetActive(false);
    }

    public void OpenSellingPanel(int index)
    {
        if(!buyingPanel.activeSelf)
        {
            maxSellAmount = materialPurchased[index];
            lastIndexUsed = index;

            sellingPanel.SetActive(true);
            priceSellingSlider.value = 0;
            priceSellingSlider.maxValue = maxSellAmount;
            maxSellingAmountText.text = maxSellAmount.ToString();
        }
    }

    public void SellItem()
    {
        materialPurchased[lastIndexUsed] -= Mathf.RoundToInt(priceSellingSlider.value);
        money = money + materialPrices[lastIndexUsed]*priceSellingSlider.value;

        materialAmountText[lastIndexUsed].text = ShorterNumber(materialPurchased[lastIndexUsed],1);
        OpenSellingPanel(lastIndexUsed);
    }

    private void LowerNewsDuration(int index)
    {
        if(objectNewsDuration[index] > 0)
        {
            objectNewsDuration[index]--;
            objectNewsModfier[index] = objectNewsModfier[index]/2;
        }
        else
        {
            objectNewsModfier[index] = 1;
        }
    }

    private void UpdateNews(int index)
    {
        LowerNewsDuration(index);
        int randomNewsModifier = Random.Range(0,100);
        if(randomNewsModifier > 85 && objectNewsDuration[index] < 1 && materialPriceModifier[index] > 0.4f && materialPriceModifier[index] < 2)
        {
            objectNewsModfier[index] = Random.Range(materialMinPercent[index]/8, materialMaxPercent[index]/5);
            objectNewsDuration[index] = Random.Range(4, 9);
            
            if(objectNewsModfier[index] == 0) objectNewsModfier[index] = 1;
            CalculateEvent(index);
        }
        if(objectNewsModfier[index] == 0) objectNewsModfier[index] = 1;
        
    }

    private void CalculateEvent(int index)
    {
        if(objectNewsModfier[index] > 1) CreateNews.instance.ShowPositiveEvent(materialNames[index]);

        else if(objectNewsModfier[index] < 1) CreateNews.instance.ShowNegativeEvent(materialNames[index]);
    }

    public void OpenTradePanel()
    {
        tradePanel.SetActive(true);
    }

    public void CloseTradePanel()
    {
        tradePanel.SetActive(false);
    }
}
