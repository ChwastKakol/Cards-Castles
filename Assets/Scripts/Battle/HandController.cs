using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public DeckController deckController;
    public int initialCardsCount = 3;
    public float angularOffset = Mathf.PI / 8.0f;
    public float radius = 4.0f;
    
    private List<CardController> _cards;

    private void Awake()
    {
        _cards = new List<CardController>();
        for (int i = 0; i < initialCardsCount; i++)
        {
            CardController newCard = deckController.DrawCardFromTop();
            _cards.Add(AddCard(newCard, i));
        }
    }

    private CardController AddCard(CardController card, int position)
    {
        CardController newCard = Instantiate(card, transform);
        float offset = position * angularOffset;
        newCard.transform.localRotation.eulerAngles.Set(0, offset * Mathf.Rad2Deg, 0);
        newCard.transform.localPosition.Set(Mathf.Sin(offset), 0, Mathf.Cos(offset));
        return newCard;
    }
}
