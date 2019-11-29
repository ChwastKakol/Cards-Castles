using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonOnClick()
    {
        Debug.Log("Clicked");
        SceneManager.LoadSceneAsync("Battle");
    }

    public void CreateDeckButtonOnClick()
    {
        SceneManager.LoadSceneAsync("DeckCreator");
    }

    public void ExitButtonOnClick()
    {
        Debug.Log("Extiting");
        Application.Quit();
    }
    
}
