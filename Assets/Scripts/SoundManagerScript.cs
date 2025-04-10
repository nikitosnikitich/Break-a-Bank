using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static SoundManagerScript Instance;

    private AudioSource source;

    [SerializeField] private AudioClip bgTheme_1;
    [SerializeField] private AudioClip bgTheme_2;
    [SerializeField] private AudioClip dialogueTheme;
    [SerializeField] private AudioClip clickSoundClip;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // public void PlayClickSound()
    // {
    //     //source.clip = clickSoundClip;
    //     //source.Play();
    //     source.PlayOneShot(clickSoundClip);
    // }
}
