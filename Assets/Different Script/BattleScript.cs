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

    public int startingCardsAmount = 4;

    // Start is called before the first frame update
    void Start()
    {
        playerMana = startingMana;
        UIController.instance.SetPlayerManaText(playerMana);

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
}
