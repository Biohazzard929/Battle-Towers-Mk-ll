using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPointController : MonoBehaviour
{
    public static CardPointController instance;
    private void Awake()
    {
        instance = this;
    }
    public CardPlaceScript[] playerCardPoints, enemyCardPoints;
    public float timeBetweenAttacks = 1.0f; // Assuming you have this variable defined

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerAttack()
    {
        StartCoroutine(PlayerAttackCo());
    }

    public IEnumerator PlayerAttackCo()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for(int i = 0; i < playerCardPoints.Length; i++)
        {
            if(playerCardPoints[i].activeCard != null)
            {
                if(enemyCardPoints[i].activeCard != null)
                {
                    // Attack logic here
                    enemyCardPoints[i].activeCard.DamageCard(playerCardPoints[i].activeCard.attackPower);

                    


                } 
                else 
                {
                    // Attack the enemy's overall health
                    BattleScript.instance.DamageEnemy(playerCardPoints[i].activeCard.attackPower);
                }
                playerCardPoints[i].activeCard.anim.SetTrigger("Attack");
                
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }
        
        CheckAssignedCards();
        
        BattleScript.instance.AdvanceTurn();
    }
    public void EnemyAttack()
    {
        StartCoroutine(EnemyAttackCo());
    }

    public IEnumerator EnemyAttackCo()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for(int i = 0; i < enemyCardPoints.Length; i++)
        {
            if(enemyCardPoints[i].activeCard != null)
            {
                if(playerCardPoints[i].activeCard != null)
                {
                    // Attack logic here
                    playerCardPoints[i].activeCard.DamageCard(enemyCardPoints[i].activeCard.attackPower);
                    enemyCardPoints[i].activeCard.anim.SetTrigger("Attack");
                } 
                else 
                {
                    // Attack the player's overall health
                    BattleScript.instance.DamagePlayer(enemyCardPoints[i].activeCard.attackPower);
                }
                enemyCardPoints[i].activeCard.anim.SetTrigger("Attack");
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }
        
        CheckAssignedCards();
        
        BattleScript.instance.AdvanceTurn();
    }

    public void CheckAssignedCards()
    {
        foreach(CardPlaceScript cardPoint in playerCardPoints)
        {
            if(cardPoint.activeCard != null)
            {
                if(cardPoint.activeCard.currentHealth <= 0)
                {
                    cardPoint.activeCard = null;
                }
            }
        }

        foreach(CardPlaceScript cardPoint in enemyCardPoints)
        {
            if(cardPoint.activeCard != null)
            {
                if(cardPoint.activeCard.currentHealth <= 0)
                {
                    cardPoint.activeCard = null;
                }
            }
        }
    }
}