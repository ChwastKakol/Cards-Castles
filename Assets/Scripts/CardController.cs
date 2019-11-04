using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{

    public int attack = 1 ;
    public int HP = 2;
    public int setupCost = 1;
    public int costPerRound = 1;

    public int position, layer;
    public int[] availableLayers;

    public float attackTime = .2f;
    
    public CardController cardToAttack;
    
    public void Start()
    {
        availableLayers = new int[]{1,2};
    }

    private void Update()
    {
        //if(Input.GetKeyUp(KeyCode.A) && !cardToAttack.Equals(null)) Attack(cardToAttack);
    }

    public void Attack(CardController other)
    {
        // Later to add fireworks, eg slide to target

        Debug.Log("Attacking");
        other.TakeDamage(attack);
        TakeDamage(other.attack);
    }

    private void TakeDamage(int damage)
    {
        // Later to add fireworks here
        HP -= damage;
        if(HP <= 0) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Later to add fireworks here
        Debug.Log("Card has been destroyed");
    }

    public void SetUp()
    {
        // To be ran on first placement on the board
        // Later might depend on placement and special skills / powers
        
        GetComponentInParent<PlayerController>().ReduceGold(setupCost);
        /*CastleController castleController = GetComponentInParent<CastleController>();
        castleController.isCarringCard = true;
        castleController.card = gameObject;*/
    }

    public void RoundlyCost()
    {
        // To be ran on first placement on the board
        // Later might depend on placement and special skills / powers
        
        GetComponentInParent<PlayerController>().ReduceGold(costPerRound);
    }
    
}
