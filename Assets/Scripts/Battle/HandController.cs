using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class HandController : MonoBehaviour
{
    public DeckController deckController;
    public int initialCardsCount = 3;
    
    public float radius = 4.0f;

    private List<CardController> _cards;
    private float _angularOffset = 20;
    
    private void Awake()
    {
        _angularOffset = CalcAngularOffset();
        
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
        PositionCard(newCard, position);
        
        return newCard;
    }

    private void Update()
    {
        _angularOffset = CalcAngularOffset();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        int index = -1;
        if (Physics.Raycast(ray, out hitinfo))
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (hitinfo.transform.Equals(_cards[i].transform))
                {
                    index = i;
                    break;
                }
            }
        }
        
        for (int i = 0; i < _cards.Count; i++)
        {
            
            if(i < index && index > -1) PositionCard(_cards[i], i, -.2f * _angularOffset);
            else if(i > index && index > -1) PositionCard(_cards[i], i, .2f * _angularOffset);
            else if( i == index) PositionCard(_cards[i], i, 0, .5f);
            else PositionCard(_cards[i], i);
        }
    }

    private void PositionCard(CardController card, int position, float angularOffset = 0, float radialOffset = 0)
    {
        float offset = (position - _cards.Count / 2) * _angularOffset + angularOffset;
        
        card.transform.localRotation = Quaternion.Euler(0, offset, 0);
        card.transform.localPosition = new Vector3(-(radius + radialOffset) * Mathf.Sin(offset * Mathf.Deg2Rad), 0, -(radius + radialOffset) * Mathf.Cos(offset* Mathf.Deg2Rad));
    }

    private float CalcAngularOffset()
    {
        return .8f / (Mathf.PI * (radius + .5f)) * 180;
    }

    public void Turn()
    {
        if (deckController.HasCards())
        {
            _cards.Add(AddCard(deckController.DrawCardFromTop(), _cards.Count));
        }
    }


    public void RemoveCard(CardController card)
    {
        _cards.Remove(card);
    }

    public CardController OnClick(RaycastHit hitinfo)
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            if(_cards[i].transform.Equals(hitinfo.transform))
            {
                var card = _cards[i];
                //RemoveCard(card);
                return card;
            }        
        }

        return null;
    }
}
