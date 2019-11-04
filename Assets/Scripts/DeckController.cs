using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{

    public List<GameObject> cards = new List<GameObject>();
    public GameObject card;
    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            cards.Add(card);
        }
    }

    public GameObject DrawCardFromTop()
    {
        if (cards.Count != 0)
        {
            Debug.Log("Returning Card");
            GameObject card = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return card;
        }
        Debug.Log("Out of Cards");
        return null;
    }
}
