using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{

    public List<CardController> cards = new List<CardController>();
    public GameObject card;
    private void Start()
    {
        /*for (int i = 0; i < 4; i++)
        {
            cards.Add(card);
        }*/
    }

    public bool HasCards()
    {
        return (cards.Count != 0);
    }
    
    public CardController DrawCardFromTop()
    {
        if (cards.Count != 0)
        {
            CardController card = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return card;
        }
        return null;
    }
}
