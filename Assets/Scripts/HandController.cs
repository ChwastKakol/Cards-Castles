using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public DeckController deck;
    public int initialCardCounter = 3;
    public float radius = 4;
    public float angularOffset = Mathf.PI / 8;
    
    private List<CardController> cardsInHand;

    // Start is called before the first frame update
    void Start()
    {
        cardsInHand = new List<CardController>();
        deck = GetComponentInParent<DeckController>();
        for (int i = 0; i < initialCardCounter; i++)
        {
            AddCardFromDeck();
            //cardsInHand.Add(deck.DrawCardFromTop());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddCardFromDeck()
    {
        CardController newCard = deck.DrawCardFromTop();
        if (newCard != null)
        {
            CardController card = Instantiate(newCard, transform);
            card.transform.localPosition = new Vector3(Mathf.Sin(angularOffset * cardsInHand.Count), 0,Mathf.Cos(-angularOffset * cardsInHand.Count)) * radius;
            //Vector3 rotationTarget = -card.transform.localPosition + transform.localPosition;
            //rotationTarget.x = card.transform.localPosition.x;
            card.transform.transform.localRotation = Quaternion.Euler(0, angularOffset * cardsInHand.Count * Mathf.Rad2Deg, 0);
            card.transform.LookAt(transform);
            cardsInHand.Add(card);
        }
    }
}
