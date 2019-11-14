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
    public PlayerController otherPlayer;
    
    private GameManager gameManager;
    private DeckController deckController;
    private CastleController castleController;
    private CastleController enemyCastle;
    
    public List<CardController> cardsOnTheTable = new List<CardController>();
    
    int gold = 100;

    private bool unblocked = true;

    private void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        deckController = GetComponent<DeckController>();
        castleController = GetComponent<CastleController>();
        enemyCastle = otherPlayer.gameObject.GetComponent<CastleController>();
    }

    public void Turn()
    {
        UpdateGoldDisplay();
        if (Input.GetKeyUp(KeyCode.T) && unblocked)
        {
            for (int i = 0; i < cardsOnTheTable.Count; i++)
            {
                cardsOnTheTable[i].RoundlyCost();
            }
            castleController.RoundlyCost();
            gameManager.SwapTurn();
        }

        if (Input.GetKeyUp(KeyCode.Space) && unblocked)
        {
            unblocked = false;
            StartCoroutine(DrawCard());
        }

        if (Input.GetKeyUp(KeyCode.U) && unblocked)
        {
            castleController.UpdateCastle();
        }

        if (Input.GetMouseButtonUp(0) && unblocked)
        {
            unblocked = false;
            processMouseClick();
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
            CardController card = deckController.DrawCardFromTop();
            CardController cardInstance = Instantiate(card, transform);//.GetComponent<CardController>();
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
            cardsOnTheTable.Last().layer = layer;

            Transform targetTransform = new GameObject().transform;
            targetTransform.position = minimum + Vector3.up * .5f;
            targetTransform.LookAt(castleController.transform);

            yield return StartCoroutine(Move(cardsOnTheTable.Last().transform, targetTransform));                    // Sets card on its final positition in 3D space
            cardsOnTheTable.Last().SetUp();
            GameObject.Destroy(targetTransform.gameObject);
        }

        unblocked = true;                                                                                              // lifts swap turn blockade
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

    IEnumerator Attack(CardController attacker)
    {
        Transform originalTransform = new GameObject().transform;
        originalTransform.position = attacker.transform.position;
        originalTransform.rotation = attacker.transform.rotation;

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("GameBoard"));

            Vector3 newPosition = originalTransform.position - .3f * (originalTransform.position - hit.point); 
            attacker.transform.position = newPosition;
            attacker.transform.LookAt(transform);

            if (Physics.Raycast(ray, out hit))
            {
                CardController attacked = hit.transform.gameObject.GetComponent<CardController>();
                if (otherPlayer.cardsOnTheTable.Contains(attacked) && (Input.GetMouseButtonUp(1)) &&
                    attacked.layer == otherPlayer.getAttackLayer())
                {
                    Transform targetTransform = new GameObject().transform;
                    targetTransform.position = attacked.gameObject.transform.position;
                    targetTransform.LookAt(transform);

                    yield return StartCoroutine(Move(attacker.gameObject.transform, targetTransform));
                    attacker.Attack(attacked);

                    GameObject.Destroy(targetTransform.gameObject);
                    break;
                }
                else if(enemyCastle.hitCastle(hit.transform) && Input.GetMouseButtonUp(1))
                {
                    Debug.Log("Attacking castle");
                    yield return StartCoroutine(Move(attacker.transform, enemyCastle.transform));
                    enemyCastle.TakeDamage(attacker.attack);
                    break;
                }
                
                else if(Input.GetKeyUp(KeyCode.Escape))
                    break;
            }
            
            yield return null;
        }

        yield return null; // gives time fpr destruction of attacking card
        if (attacker != null)
        {
            Debug.Log("Attacker is not null");
            yield return StartCoroutine(Move(attacker.transform, originalTransform));
        }
        GameObject.Destroy(originalTransform.gameObject);
        unblocked = true;
    }
    
    void processMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        CardController grabed = hit.transform.gameObject.GetComponent<CardController>();
        
        if (cardsOnTheTable.Contains(grabed))
        {
            StartCoroutine(Attack(grabed));
        }
    }

    public int getAttackLayer()
    {
        int max = 0;
        for (int i = 0; i < cardsOnTheTable.Count; i++)
        {
            if (cardsOnTheTable[i].layer > max) max = cardsOnTheTable[i].layer;
        }
        return max;
    }
    
}
