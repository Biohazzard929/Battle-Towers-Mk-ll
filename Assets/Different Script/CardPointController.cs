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
    public CardPlaceScript[] playerCardPoints, enemyCardPoints, enemyBackRow;
    public float timeBetweenAttacks = 1.0f;

    void Start()
    {

    }

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

        for (int i = 0; i < playerCardPoints.Length; i++)
        {
            if (playerCardPoints[i].activeCard != null)
            {
                if (enemyCardPoints[i].activeCard != null)
                {
                    BattleScript.instance.ResolveCardInteraction(playerCardPoints[i].activeCard, enemyCardPoints[i].activeCard);
                }
                else
                {
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

        for (int i = 0; i < enemyCardPoints.Length; i++)
        {
            if (enemyCardPoints[i].activeCard != null)
            {
                if (playerCardPoints[i].activeCard != null)
                {
                    BattleScript.instance.ResolveCardInteraction(enemyCardPoints[i].activeCard, playerCardPoints[i].activeCard);
                }
                else
                {
                    BattleScript.instance.DamagePlayer(enemyCardPoints[i].activeCard.attackPower);
                }
                enemyCardPoints[i].activeCard.anim.SetTrigger("Attack");
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        CheckAssignedCards();

        BattleScript.instance.AdvanceTurn();
    }

    public void EnemyMovement()
    {
        StartCoroutine(EnemyMovementCo());
    }

    public IEnumerator EnemyMovementCo()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        for (int i = 0; i < enemyCardPoints.Length; i++)
        {
            if (enemyCardPoints[i].activeCard == null)
            {
                if (enemyBackRow[i].activeCard != null)
                {
                    enemyCardPoints[i].activeCard = enemyBackRow[i].activeCard;
                    enemyBackRow[i].activeCard = null;
                    enemyCardPoints[i].activeCard.MoveToPoint(enemyCardPoints[i].transform.position, enemyCardPoints[i].transform.rotation);
                }
            }
        }

        CheckAssignedCards();
    }

    public void CheckAssignedCards()
    {
        foreach (CardPlaceScript cardPoint in playerCardPoints)
        {
            if (cardPoint.activeCard != null)
            {
                if (cardPoint.activeCard.currentHealth <= 0)
                {
                    cardPoint.activeCard = null;
                }
            }
        }

        foreach (CardPlaceScript cardPoint in enemyCardPoints)
        {
            if (cardPoint.activeCard != null)
            {
                if (cardPoint.activeCard.currentHealth <= 0)
                {
                    cardPoint.activeCard = null;
                }
            }
        }
    }
}
