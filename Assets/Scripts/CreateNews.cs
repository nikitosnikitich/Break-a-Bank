using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CreateNews : MonoBehaviour
{
    [SerializeField] private int requiredName;

    [SerializeField] private GameObject content;
    private GameObject tempText;
    public GameObject eventText;
    private List<string> events = new List<string>();

    private int randomIndex;

    public static CreateNews instance;

    private void LoadEventsFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Events.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            events.AddRange(lines);
        }
        else
        {
            Debug.LogError("Файл Events.txt не знайдено!");
        }
    }

    public void ShowPositiveEvent(string name)
    {
        randomIndex = Random.Range(0, 10);

        tempText = Instantiate(eventText);
        tempText.transform.SetParent(content.transform);
        eventText.GetComponent<Text>().text = events[randomIndex];
    }

    public void ShowNegativeEvent(string name)
    {
        randomIndex = Random.Range(10, 20);

        tempText = Instantiate(eventText);
        tempText.transform.SetParent(content.transform);
        eventText.GetComponent<Text>().text = events[randomIndex];
    }

    public void ShowMemeEvent(int index)
    {
        tempText = Instantiate(eventText);
        tempText.transform.SetParent(content.transform);
        eventText.GetComponent<Text>().text = events[randomIndex];
    }

    void Awake()
    {
        instance = this;
        LoadEventsFromFile();
    }

    public int ReturnHapinnesBoost()
    {
        if(randomIndex >= 10 && randomIndex <= 20)
        {
            return 1;
        }
        if(randomIndex >= 0 && randomIndex <= 10)
        {
            return 0;
        }
        return 0;
    }
}
