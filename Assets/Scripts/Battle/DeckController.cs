using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckController : MonoBehaviour
{

    public CardsFactory cardsFactory;
    public List<CardController> cards;
    public Deck deck;

    private void Awake()
    {
        deck = new Deck();
        List<int> st = deck.cardID = new List<int>();
        st.Add(0);
        st.Add(0);
        st.Add(3);
        st.Add(6);
        
        cardsFactory = FindObjectOfType<CardsFactory>();
        cards = cardsFactory.getCardsInDeck(deck);
        Reshuffle();
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

    public void Reshuffle()
    {
        List<CardController> temp = new List<CardController>();
        while (cards.Count > 0)
        {
            int index = Random.Range(0, cards.Count);
            temp.Add(cards[index]);
            cards.RemoveAt(index);
        }

        cards = temp;
    }
}
