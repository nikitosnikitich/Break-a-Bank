using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public bool activateBankPanel = false;

    public PetShopPanel pet;
    public Rigidbody2D rb;
    public Animator animator;
    public Collider2D col;

    private bool right;
    private bool walking;
    [SerializeField] private float speed = 2f;
    private Vector2 direction;

    [SerializeField] private Slider happinessSlider;
    [SerializeField] private int startHappiness;

    public int happinessLevel;
    private int maxHappiness = 400;

    [SerializeField] private GameObject loosePanel;


    private void Start()
    {
        happinessLevel = startHappiness;
        loosePanel.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        //happinessLevel = 1000;

        StartCoroutine(DecreaseHappiness());
        InvokeRepeating("HappinessModifier", 0, 3f);
    }

    void Update()
    {
        if(happinessLevel <= 1)
        {
            YouLoose();
        }
        if(happinessLevel >= maxHappiness)
        {
            happinessLevel = maxHappiness;
        }

        
        happinessSlider.value = happinessLevel;

        direction = new Vector2(Input.GetAxis("Horizontal"), (Input.GetAxis("Vertical")));

        if (direction.x > 0)
        {
            right = true;
        }
        else if (direction.x < 0) 
        {
            right = false;
        }

        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }

        

        if (right && walking)
        {
            animator.Play("walking_right");
        }
        else if (!right && walking) 
        {
            animator.Play("walking_left");
        }
        else if (right && !walking)
        {
            animator.Play("idle_right");
        }
        else
        {
            animator.Play("idle_left");
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    // IEnumerator DecreaseHappiness()
    // {
    //     if (pet.petOwned)
    //     {
    //         while (happinessLevel > 300)
    //         {
    //             yield return new WaitForSeconds(2f);
    //             happinessLevel -= 1;
    //         }
    //     }
    //     else
    //     {   
    //         while (happinessLevel > 0)
    //         {
    //             yield return new WaitForSeconds(2f);
    //             happinessLevel -= 1;
    //         }
    //     }
    // }
    IEnumerator DecreaseHappiness()
    {
        while(happinessLevel > 0)
        {
            yield return new WaitForSeconds(1f);
            happinessLevel -= 1;

            if(happinessLevel < 300 && pet.petOwned)
            {
                happinessLevel = 300;
            }
        }
    }

    private void YouLoose()
    {
        StopAllCoroutines();
        loosePanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    private void HappinessModifier()
    {
        if(CreateNews.instance.ReturnHapinnesBoost() == 0)
        {
            int value = Random.Range(2, 5);
            happinessLevel += value;
        }
    }
}