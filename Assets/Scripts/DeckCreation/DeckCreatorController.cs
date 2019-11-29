using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckCreatorController : MonoBehaviour
{

    public GameObject creatingMode;
    public GameObject viewingMode;
    public GameObject SideChoosing;

    // Start is called before the first frame update
    void Start()
    {
        creatingMode.SetActive(false);
        viewingMode.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackButtonOnClikc()
    {
        SceneManager.LoadSceneAsync("OpenMenu");
    }

    public void CreateDeckOnClick()
    {
        viewingMode.SetActive(false);
        creatingMode.SetActive(true);
        SideChoosing.SetActive(true);
    }
}
