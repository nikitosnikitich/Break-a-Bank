using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private float volumeLevel;
    [SerializeField] private AudioClip[] soundClip;
    //1-натискання кнопк; 2-Загадкова музика

    IEnumerator PlaySound(int clipIndex, float time)
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>(); 

        audio.clip = soundClip[clipIndex];
        audio.pitch = Random.Range(0.9f, 1.1f);
        audio.volume = volumeLevel;
        audio.Play();
        
        yield return new WaitForSeconds(time);
        Destroy(audio);
    }

    public void PlayClickSound()
    {
        int mysteriousMusicModifier = Random.Range(0,101);
        if(mysteriousMusicModifier > 99)
        {
            StartCoroutine(PlaySound(1, 15));
        }
        else
        {
            StartCoroutine(PlaySound(0, soundClip[0].length));
        }
    }
}
