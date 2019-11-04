using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject card;
    public Text goldDisplay;

    public Transform cameraTransform;

    private GameManager gameManager;
    private DeckController deckController;
    
    int gold = 100;
    // Update is called once per frame

    private void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        deckController = GetComponent<DeckController>();
    }

    void Update()
    {
        /*if (myTurn)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject newCard = Instantiate(card, new Vector3(4, 1, 0), Quaternion.identity, transform);
                deck.Add(newCard);
                newCard.GetComponent<CardController>().SetUp();
            }

            UpdateGoldDisplay();

            if (Input.GetKeyDown(KeyCode.T))
            {
                for (int i = 0; i < deck.Count; i++)
                {
                    deck[i].GetComponent<CardController>().RoundlyCost();
                }
            }
        }*/
    }   

    public void Turn()
    {
        UpdateGoldDisplay();
        if (Input.GetKeyUp(KeyCode.T))
        {
            gameManager.SwapTurn();
        }
    }

    void UpdateGoldDisplay()
    {
        goldDisplay.text = "Gold: " + gold.ToString() + "£";
    }

    public void ReduceGold(int amount)
    {
        gold -= amount;
    }

    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }

    IEnumerator DrawCard()
    {
        GameObject card = deckController.DrawCardFromTop();
        if (!card.Equals(null))
        {
            while (Input.GetMouseButtonUp(1))                         //carry card until its placed
            {
                yield return null;
            }
        }
        //yield break;
    }
}
