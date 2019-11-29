using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedButtonController : MonoBehaviour
{
    private int ID = Int32.MinValue;

    public Text nameField;
    public DeckCreationController deckCreationController;

    public int Id
    {
        get => ID;
        set => ID = ID == Int32.MinValue ? value : ID;
    }

    public void setName(string newName)
    {
        nameField.text = newName;
    }

    public void OnClick()
    {
        deckCreationController.AddCardWithID(ID);
        deckCreationController.RemoveFromSelection(ID);
        GameObject.Destroy(gameObject);
    }
}
