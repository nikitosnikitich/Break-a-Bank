using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Collider2D col;
    public GameObject TopWalls;
    [SerializeField] private bool inside;
    [SerializeField] private bool check;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        TopWalls.SetActive(true);
        inside = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.CompareTag("Player") && !inside)
        {
            TopWalls.SetActive(false);
            inside = true;
        }
        else if (collision.gameObject.CompareTag("Player") && inside)
        {
            TopWalls.SetActive(true);
            inside = false;
        }
    }
}
