using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;

public class BattleScript : MonoBehaviour
{
   public static BattleScript instance;
    private void Awake()
    {
        instance = this;
    } 
    
   // public TMP_Text playerManaText;
    public int startingMana = 4, maxMana = 10;
    public int playerMana, enemyMana;

    public int startingCardsAmount = 6;
    public int cardsToDrawPerTurn = 1; 

    public enum TurnOrder {playerActive, playerCardAttacks,enemyMovement, enemyActive, enemyCardAttacks};
    public TurnOrder currentPhase;
    private int currentPlayerMaxMana, currentEnemyMaxMana;

    public Transform discard;

    public int playerHealth;

    public int enemyHealth;

    public bool battleEnded;

    public float resultScreenDelayTime;

    public void AdvanceTurn()
    {
        if (battleEnded == false)
        {
            currentPhase++;
            if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
            {
                currentPhase = 0;
            }

            switch (currentPhase)
            {
                case TurnOrder.playerActive:
                    Debug.Log("Player Active");
                    UIController.instance.endTurnButton.SetActive(true);

                    if (currentPlayerMaxMana < maxMana)
                    {
                        currentPlayerMaxMana++;
                    }
                    UIController.instance.drawCardButton.SetActive(true);
                    FillPlayerMana();

                    DeckController.instance.drawMultipleCards(cardsToDrawPerTurn);
                    break;
                case TurnOrder.playerCardAttacks:
                    Debug.Log("Player Card Attacks");
                    CardPointController.instance.PlayerAttack();
                    break;
                case TurnOrder.enemyMovement:
                    Debug.Log("Enemy Movement");
                    CardPointController.instance.EnemyMovement();
                    break;
                case TurnOrder.enemyActive:
                    Debug.Log("Enemy Active");
                    if (currentEnemyMaxMana < maxMana)
                    {
                        currentEnemyMaxMana++;
                    }
                    FillEnemyMana();
                    AIController.instance.StartAction();
                    break;
                case TurnOrder.enemyCardAttacks:
                    Debug.Log("Enemy Card Attacks");
                    CardPointController.instance.EnemyAttack();
                    break;
            }
        }
    }
    public void EndPlayerTurn()
        {
            UIController.instance.endTurnButton.SetActive(false);
            UIController.instance.drawCardButton.SetActive(false);
            AdvanceTurn();
        }
     public void FillPlayerMana()
    {
        playerMana =  currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }
    public void FillEnemyMana()
    {
        enemyMana = currentEnemyMaxMana;
        UIController.instance.SetEnemeyMana(enemyMana);
    }

    // Start is called before the first frame update
    void Start()
    { 
        currentPlayerMaxMana = startingMana;
        FillPlayerMana();
        currentEnemyMaxMana = startingMana;
        FillEnemyMana();
        UIController.instance.SetEnemyHealthText(enemyHealth);
        UIController.instance.SetPlayerHealthText(playerHealth);

        DeckController.instance.drawMultipleCards(startingCardsAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana = playerMana - amountToSpend;

        if(playerMana < 0)
        {
            playerMana = 0;
        }

        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void SpendEnemyMana(int amountToSpend)
    {
        enemyMana -= amountToSpend;

        if (enemyMana < 0)
        {
            enemyMana = 0;
        }

        UIController.instance.SetEnemeyMana(enemyMana);
    }

    public void DamagePlayer(int damageAmount)
    {
       if(playerHealth > 0 || !battleEnded)
        {
            playerHealth -= damageAmount;

            if(playerHealth < 0)
            {
                playerHealth = 0;

                EndBattle();
            }

            UIController.instance.SetPlayerHealthText(playerHealth);

            UIDamageIndicator damageIndicator = Instantiate(UIController.instance.playerDamage, UIController.instance.playerDamage.transform.parent);
            damageIndicator.damageText.text = "-" + damageAmount.ToString();
            damageIndicator.gameObject.SetActive(true);

            //AudioManager.instance.PlaySFX(6);

        }
        
        else
        {
            Debug.Log("Player is dead");
        }
    }

    public void DamageEnemy(int damageAmount)
    {
        if(enemyHealth > 0 || !battleEnded)
        {
            enemyHealth -= damageAmount;

            if(enemyHealth <= 0)
            {
               // enemyHealth = 0;

                EndBattle(); // End the battle if enemy health is 0
            }

            UIController.instance.SetEnemyHealthText(enemyHealth);
            UIDamageIndicator damageIndicator = Instantiate(UIController.instance.enemyDamage, UIController.instance.enemyDamage.transform.parent);
            damageIndicator.damageText.text = "-" + damageAmount.ToString();
            damageIndicator.gameObject.SetActive(true);

            //AudioManager.instance.PlaySFX(5);

        }
        else
        {
            Debug.Log("Enemy is dead");
            EndBattle(); // End the battle if enemy health is 0
        }
    }
    public void ResolveCardInteraction(Card_U attacker, Card_U defender)
    {
        bool isCritical = false;

        if (attacker.cardType == Card_UScripptableObject.CardType.Robot && defender.cardType == Card_UScripptableObject.CardType.Human)
        {
            // Mechanical beats Human
            defender.DamageCard(attacker.attackPower + 2, true); // Example: double damage
            isCritical = true;
        }
        else if (attacker.cardType == Card_UScripptableObject.CardType.Human && defender.cardType == Card_UScripptableObject.CardType.Creature)
        {
            // Human beats Creature
            defender.DamageCard(attacker.attackPower + 2, true); // Example: double damage
            isCritical = true;
        }
        else if (attacker.cardType == Card_UScripptableObject.CardType.Creature && defender.cardType == Card_UScripptableObject.CardType.Robot)
        {
            // Creature beats Mechanical
            defender.DamageCard(attacker.attackPower + 2, true); // Example: double damage
            isCritical = true;
        }
        else
        {
            // Normal damage
            defender.DamageCard(attacker.attackPower, false);
        }
    }
    void EndBattle()
    {
        battleEnded = true;

        if (enemyHealth <= 0)
        {
            UIController.instance.battleResultText.text = "YOU WON!";

            foreach (CardPlaceScript point in CardPointController.instance.enemyCardPoints)
                {
                    if (point.activeCard != null)
                    {
                        point.activeCard.MoveToPoint(discard.position, point.activeCard.transform.rotation);
                    }
                }
                foreach (CardPlaceScript point in CardPointController.instance.enemyBackRow)
                {
                    if (point.activeCard != null)
                    {
                        point.activeCard.MoveToPoint(discard.position, point.activeCard.transform.rotation);
                    }
                }
        

        }   
        else
            {
                UIController.instance.battleResultText.text = "YOU LOST!";

                foreach (CardPlaceScript point in CardPointController.instance.playerCardPoints)
                {
                    if (point.activeCard != null)
                    {
                        point.activeCard.MoveToPoint(discard.position, point.activeCard.transform.rotation);
                    }
                }
            }
        //HandController.instance.EmptyHand();
    
    StartCoroutine(ShowResultCo());
    }

    IEnumerator ShowResultCo()
    {
        yield return new WaitForSeconds(resultScreenDelayTime);

        UIController.instance.battleEndScreen.SetActive(true);
    }
}



