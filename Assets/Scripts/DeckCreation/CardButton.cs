using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    public Text nameIndicator;
    public Text HPIndicator;
    public Text attackIndicator;
    public Text costIndicator;
    public Text productinIndicator;

    private int ID;

    public DeckCreationController deckCreationController;

    public List<CardButton> cardsInSelection;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStats(int[] stats)
    {
        HPIndicator.text = stats[0].ToString();
        attackIndicator.text = stats[1].ToString();
        costIndicator.text = stats[2].ToString() + "/" + stats[3].ToString();
        productinIndicator.text = stats[4].ToString();
    }

    public void SetName(string newName)
    {
        nameIndicator.text = newName;
    }

    public string GetName()
    {
        return nameIndicator.text;
    }

    public int Id
    {
        get => ID;
        set => ID = value;
    }

    public void OnClick()
    {
        Debug.Log("REMOVING");
        deckCreationController.AddCardsToSelected(this);
        deckCreationController.RemoveFromAvailable(ID);
        Destroy(gameObject);
    }
    
}
