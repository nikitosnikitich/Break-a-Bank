using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void PlayButton(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void X_Button_In_Game(int index)
    {
        SceneManager.LoadScene(index);
        //SoundManagerScript.Instance.X_Button_In_Game();
    }
}
