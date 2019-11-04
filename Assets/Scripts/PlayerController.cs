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
    
    private List<CardController> cardsOnTheTable = new List<CardController>();
    
    int gold = 100;
    // Update is called once per frame

    private void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        deckController = GetComponent<DeckController>();
    }

    public void Turn()
    {
        UpdateGoldDisplay();
        if (Input.GetKeyUp(KeyCode.T))
        {
            gameManager.SwapTurn();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(DrawCard());
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
        Debug.Log("Started Courutine");
        GameObject card = deckController.DrawCardFromTop();
        if (!card.Equals(null))
        {
            GameObject cardInstance = Instantiate(card, transform);
            cardInstance.transform.localPosition = Vector3.zero;
            cardInstance.transform.localRotation = Quaternion.identity;
            
            while (!Input.GetMouseButtonUp(1))                         //carry card until its placed
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                Physics.Raycast(ray, out raycastHit, 100.0f, LayerMask.GetMask("GameBoard"));
                Vector3 hit = raycastHit.point;
                hit.y = 1.0f;
                card.transform.position = hit;

                yield return null;
            }
            
            cardsOnTheTable.Add(cardInstance.GetComponent<CardController>());
            cardInstance.GetComponent<CardController>().SetUp();
        }
        Debug.Log("Finished Courutine");
        //yield break;
    }
}
