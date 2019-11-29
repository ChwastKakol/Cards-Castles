using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.UI;


public class DeckCreationController : MonoBehaviour
{
    //Serialized Fields:_____________________________________________
    public CardButton cardPrefab;
    public SelectedButtonController selectedPrefab;
    
    public Transform choosingContent;
    public Transform selectedContent;
    
    public GameObject creatingMode;
    public GameObject viewingMode;
    public GameObject sideChoosing;

    public Text inputField;
    
    public DataReaderWriter _dataReaderWriter;
    //_______________________________________________________________
    
    public List<CardButton> _cardsInSelection;
    private List<SelectedButtonController> _selected;

    private Sides Side = Sides.Tech;

    private List<List<CardController>> _cards;
    
    private Deck _deck;

    void Start()
    {
        _cardsInSelection = new List<CardButton>();
        _selected = new List<SelectedButtonController>();
        _deck = new Deck();
        _cards = _dataReaderWriter.cards;
    }

    public void SwitchSideOnClick()
    {
        SetSide(Side == Sides.Tech ? Sides.Wood : Sides.Tech);
        _deck = new Deck();
    }

    public void WoodOnClick()
    {
        SetSide(Sides.Wood);
        sideChoosing.SetActive(false);
    }

    public void TechOnClick()
    {
        SetSide(Sides.Tech);
        sideChoosing.SetActive(false);
    }

    void SetSide(Sides side)
    {
        Side = side;
        Debug.Log(side.ToString());
        
        List<CardController> chosenSide = (Side == Sides.Tech ? _cards[0] : _cards[1]);

        for (int i = 0; i < _selected.Count; i++)
        {
            Destroy(_selected[i].gameObject);
        }
        _selected.Clear();
        
        Debug.Log("FINISHED REMOVING SELECTED");
        
        for (int i = 0; i < _cardsInSelection.Count; i++)
        {
            Destroy(_cardsInSelection[i].gameObject);
        }
        _cardsInSelection.Clear();

        Debug.Log("FINISHED REMOVING ALL");

        AddFromSide(chosenSide);
        AddFromSide(_cards[2]);
    }

    public void AddCardsToSelected(CardButton card)
    {
        SelectedButtonController newSelected = Instantiate(selectedPrefab, selectedContent);
        newSelected.deckCreationController = this;
        newSelected.Id = card.Id;
        newSelected.setName(card.GetName());
        _selected.Add(newSelected);
    }

    public void SaveButtonOnClick()
    {
        _deck.deckName = (inputField.text != "" ? inputField.text : _dataReaderWriter.GetSize().ToString());
        _deck.side = (Side == Sides.Tech ? 0 : 1);

        for (int i = 0; i < _selected.Count; i++)
        {
            _deck.cardID.Add(_selected[i].Id);
        }

        _dataReaderWriter.Add(_deck);
        
        creatingMode.SetActive(false);
        viewingMode.SetActive(true);
    }
    
    void AddFromSide(List<CardController> chosenSide)
    {
        for (int i = 0; i < chosenSide.Count; i++)
        {
            AddCard(chosenSide[i]);
        }
    }

    private void AddCard(CardController cardController)
    {
        CardButton newCard = Instantiate(cardPrefab, choosingContent);
            
        newCard.SetName(cardController.GetName());
        newCard.SetStats(cardController.GetParameters());
        newCard.Id = cardController.Id;
        newCard.deckCreationController = this;
            
        _cardsInSelection.Add(newCard);
    }
    
    public void AddCardWithID(int ID)
    {
        AddCard(_cards[ID%3][ID/3]);
    }

    public void RemoveFromSelection(int ID)
    {
        for (int i = 0; i < _selected.Count; i++)
        {
            if (_selected[i].Id == ID)
            {
                _selected.RemoveAt(i);
                return;
            }
        }
    }

    public void RemoveFromAvailable(int ID)
    {
        for (int i = 0; i < _cardsInSelection.Count; i++)
        {
            if (_cardsInSelection[i].Id == ID)
            {
                _cardsInSelection.RemoveAt(i);
                return;
            }
        }
    }
}
