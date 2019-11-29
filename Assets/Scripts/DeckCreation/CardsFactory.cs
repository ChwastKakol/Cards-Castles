using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sides
{
    Tech,
    Wood, 
    Neutral
}

public class CardsFactory : MonoBehaviour
{
    public List<CardController> woodCards;
    public List<CardController> techCards;
    public List<CardController> neutralCards;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < 3; i++)
        {
            var cards = getCardList(i);
            for (int j = 0; j < cards.Count; j++)
            {
                cards[j].Id = 3 * j + i;
            }
        }
    }

    public List<CardController> getCardsInDeck(Deck deck)
    {
        List<CardController> res = new List<CardController>();
        for (int i = 0; i < deck.cardID.Count; i++)
        {
            res.Add(getCardList(deck.cardID[i]%3)[deck.cardID[i]/3]);
        }

        return res;
    }


    private List<CardController> getCardList(int i)
    {
        switch (i)
        {
            case 0: return techCards;
            case 1: return woodCards;
            default: return neutralCards;
        }
    }

    private List<CardController> getCardList(Sides side)
    {
        return getCardList((int) side);
    }
}