using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardController))]
public class SpecialSkils : MonoBehaviour
{
    private CardController _card;

    public delegate void SpecialSkillOnAttack();
    public delegate void SpecialSkillOnDraw();

    public bool callsTest;
    public bool callsAntotherTest;
    public bool insultPLayer;
    
    private void Awake()
    {
        _card = GetComponent<CardController>();
        if(callsTest) _card.AddSpecialSkillOnAttack(CallsTestFunc);
        if(callsAntotherTest) _card.AddSpecialSkillOnAttack(CallsAnotherTestFunc);
        if(insultPLayer) _card.AddSpecialSkillOnDraw(InsultPlayerFunc);
        
    }

    void CallsTestFunc()
    {
        Debug.Log("HEy, i'm called - its Test");
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
