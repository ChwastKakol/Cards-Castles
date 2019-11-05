using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject card;
    public Text goldDisplay;

    public Transform cameraTransform;

    private GameManager gameManager;
    private DeckController deckController;
    private CastleController castleController;
    
    private List<CardController> cardsOnTheTable = new List<CardController>();
    
    int gold = 100;

    private bool canSwapTurn = true;
    // Update is called once per frame

    private void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        deckController = GetComponent<DeckController>();
        castleController = GetComponent<CastleController>();
    }

    public void Turn()
    {
        UpdateGoldDisplay();
        if (Input.GetKeyUp(KeyCode.T) && canSwapTurn)
        {
            for (int i = 0; i < cardsOnTheTable.Count; i++)
            {
                cardsOnTheTable[i].RoundlyCost();
            }
            gameManager.SwapTurn();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            canSwapTurn = false;
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
        
        if (deckController.HasCards())
        {
            GameObject card = deckController.DrawCardFromTop();
            CardController cardInstance = Instantiate(card, transform).GetComponent<CardController>();
            cardInstance.transform.localPosition = Vector3.zero;

            Vector3 minimum = Vector3.zero;                                            
            int rowPosition = 0, layer = cardInstance.availableLayers[0];
             
            while (!(Input.GetMouseButtonUp(1) && cardInstance.availableLayers.Contains(layer)))                         // Performs carrying until card is placed in correct row,                           
            {                                                                                                            // updates cards, position and row
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                Physics.Raycast(ray, out raycastHit, 100.0f, LayerMask.GetMask("GameBoard"));

                Vector3 playerPosition = castleController.GetPosition(raycastHit.point, out minimum, out layer, out rowPosition);
                playerPosition.y = .3f;
                cardInstance.transform.position = playerPosition;
                cardInstance.transform.LookAt(castleController.transform);
                yield return null;
            }
            
            cardsOnTheTable.Add(cardInstance);

            cardsOnTheTable.Last().position = rowPosition;                                                               // Updates cards final position, layer
            cardsOnTheTable.Last().position = layer;

            Transform targetTransform = new GameObject().transform;
            targetTransform.position = minimum + Vector3.up * .5f;
            targetTransform.LookAt(castleController.transform);
            
            StartCoroutine(Move(cardsOnTheTable.Last().transform, targetTransform));                    // Sets card on its final positition in 3D space
            cardsOnTheTable.Last().SetUp();
        }

        canSwapTurn = true;                                                                                              // lifts swap turn blockade
        Debug.Log("Finished Courutine");
    }

    public static IEnumerator Move(Transform gameObject, Transform target, float moveTime = .3f)
    {
        float time = 0;
        while (time <= moveTime)
        {
            gameObject.position = Vector3.Slerp(gameObject.position,target.position, time / moveTime);
            gameObject.rotation = Quaternion.Slerp(gameObject.rotation,target.rotation, time / moveTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
