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
    public int production = 1;
    
    public int position, layer;
    public int[] availableLayers;

    public float attackTime = .2f;
    
    public PlayerController playerController;

    public TextMesh HPIndicator;
    public TextMesh AttackIndicator;
    public TextMesh CostsIndicator;
    public TextMesh ProductionIndicator;
    
    public void Start()
    {
        availableLayers = new int[]{1,2};
        playerController = GetComponentInParent<PlayerController>();
        UpdateIndicators();
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
        UpdateIndicators();
        if (HP <= 0)
        {
            //Destroy(gameObject);
            DestroySelf();
        }
    }

    private void OnDestroy()
    {
        // Later to add fireworks here
        /*if (playerController.cardsOnTheTable.Contains(this))
        {
            playerController.cardsOnTheTable.Remove(this);
        }*/

        Debug.Log("Card has been destroyed");
    }

    public void DestroySelf()
    {
        Debug.Log(this.ToString());
        if(playerController == null) Debug.Log("red alert");
        if (playerController.cardsOnTheTable.Contains(this))
        {
            Debug.Log(this.ToString());
            playerController.cardsOnTheTable.Remove(this);
        }
        Debug.Log(this.ToString());
        GameObject.Destroy(gameObject);
    }

    public void SetUp()
    {
        // To be ran on first placement on the board
        // Later might depend on placement and special skills / powers
        
        GetComponentInParent<PlayerController>().ReduceGold(setupCost);
    }

    public void RoundlyCost()
    {
        // To be ran on first placement on the board
        // Later might depend on placement and special skills / powers
        
        GetComponentInParent<PlayerController>().ReduceGold(costPerRound - production);
    }

    void UpdateIndicators()
    {
        HPIndicator.text = HP.ToString();
        AttackIndicator.text = attack.ToString();
        ProductionIndicator.text = production.ToString();
        CostsIndicator.text = setupCost.ToString() + "/" + costPerRound.ToString();
    }
    
}
