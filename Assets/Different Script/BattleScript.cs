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
    public int playerMana;

    public int startingCardsAmount = 5;
    public int cardsToDrawPerTurn = 0;

    public enum TurnOrder {playerActive, playerCardAttacks, enemyActive, enemyCardAttacks};
    public TurnOrder currentPhase;
    private int currentPlayerMaxMana;

    public void AdvanceTurn()
        {
            currentPhase++;
            if((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
            {
                currentPhase = 0;
            }

            switch(currentPhase)
            {
            case TurnOrder.playerActive:
                Debug.Log("Player Active");
                UIController.instance.endTurnButton.SetActive(true);

                if(currentPlayerMaxMana < maxMana)
                {
                    currentPlayerMaxMana++;
                }
                UIController.instance.drawCardButton.SetActive(true);
                FillPlayerMana();

                DeckController.instance.DrawMultipleCards(cardsToDrawPerTurn);
                    break;
            case TurnOrder.playerCardAttacks:
                Debug.Log("Player Card Attacks");
            // AdvanceTurn();
                CardPointController.instance.PlayerAttack();
                break;
            case TurnOrder.enemyActive:
                Debug.Log("Enemy Active");
                AdvanceTurn();
                break;           
            case TurnOrder.enemyCardAttacks:
                Debug.Log("Enemy Card Attacks");
                AdvanceTurn();
                break;            
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
        playerMana =  currentPlayerMaxMana = startingMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    // Start is called before the first frame update
    void Start()
    { 
        currentPlayerMaxMana = startingMana;
        FillPlayerMana();
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
}
