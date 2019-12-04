using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardController))]
public class SpecialSkils : MonoBehaviour
{
    private CardController _card;

    public delegate void SpecialSkillOnAttack();
    public delegate void SpecialSkillOnPlay();

    public bool callsTest;
    public bool callsAntotherTest;
    public bool insultPlayer;
    
    private void Awake()
    {
        _card = GetComponent<CardController>();
        if(callsTest) _card.AddSpecialSkillOnAttack(CallsTestFunc);
        if(callsAntotherTest) _card.AddSpecialSkillOnAttack(CallsAnotherTestFunc);
        if(insultPlayer) _card.AddSpecialSkillOnPlay(InsultPlayerFunc);
        
    }

    void CallsTestFunc()
    {
        Debug.Log("Hey, i'm called - its Test");
    }

    void CallsAnotherTestFunc()
    {
        Debug.Log("Another test is being called");
    }

    void InsultPlayerFunc()
    {
        Debug.Log("You cant play, boomer");
    }
}
