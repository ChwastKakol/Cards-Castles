using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataReaderWriter : MonoBehaviour
{
    public List<List<CardController>> cards;

    public CardController[] techCards;
    public CardController[] woodCards;
    public CardController[] neutralCards;

    private BinaryWriter _writer;
    private BinaryReader _reader;

    private List<Deck> _decks;

    private void Awake()
    {
        cards = new List<List<CardController>>();
        cards.Add(techCards.OfType<CardController>().ToList());
        cards.Add(woodCards.OfType<CardController>().ToList());
        cards.Add(neutralCards.OfType<CardController>().ToList());
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < cards[i].Count; j++)
            {
                cards[i][j].Id = 3 * j + i;
            }
        }

        int count;
        if (File.Exists("database.txt"))
        {
            _reader = new BinaryReader(File.Open("database.txt", FileMode.Open));
            count = _reader.ReadInt32();
        }
        else
        {
            _reader = new BinaryReader(File.Open("database.txt", FileMode.Create));
            count = 0;
        }
        for (int i = 0; i < count; i++)
        {
            _decks.Add(ReadDeck());
        }
        //_writer = new BinaryWriter(File.Open("dataBase.txt", FileMode.Open));
    }

    public void Add(Deck deck)
    {
        for (int i = 0; i < _decks.Count; i++)
        {
            if (_decks[i].deckName == deck.deckName)
            {
                _decks[i] = deck;
                return;
            }
        }
        _decks.Add(deck);
    }

    public void Remove(string deckName)
    {
        for (int i = 0; i < _decks.Count; i++)
        {
            if (_decks[i].deckName == deckName)
            {
                _decks.RemoveAt(i);
                break;
            }
        }
    }

    public void Remove(Deck deck)
    {
        _decks.Remove(deck);
    }
    
    public void SaveDeck(Deck deck)
    {
        _writer.Write(deck.deckName);
        _writer.Write(deck.side);
        _writer.Write(deck.cardID.Count);
        for (int i = 0; i < deck.cardID.Count; i++)
        {
            _writer.Write(deck.cardID[i]);
        }
    }

    Deck ReadDeck()
    {
        Deck deck = new Deck();
        deck.deckName = _reader.ReadString();
        deck.side = _reader.ReadInt32();
        
        int size = _reader.ReadInt32();
        
        for (int i = 0; i < size; i++)
        {
            deck.cardID.Add(_reader.ReadInt32());
        }

        return deck;
    }

    public List<CardController> GetDeck(Deck deck)
    {
        List<CardController> newList = new List<CardController>();
        for (int i = 0; i < deck.cardID.Count; i++)
        {
            int id = deck.cardID[i];
            newList.Add(cards[id%3][id/3]);
        }

        return newList;
    }

    public int GetSize()
    {
        return _decks.Count;
    }

    private void OnDestroy()
    {
        _reader.Close();
        _writer = new BinaryWriter(File.Open("database.txt", FileMode.Open));
        _writer.Write(_decks.Count);
        for (int i = 0; i < _decks.Count; i++)
        {
            SaveDeck(_decks[i]);
        }
        _writer.Close();
    }
}
