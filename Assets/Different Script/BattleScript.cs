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
    // Start is called before the first frame update
    void Start()
    {
        playerMana = startingMana;
        //UIController.instace.SetPlayerManaText(playerMana);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpendPlayerMana(int manaAmount)
    {
        //playerManaText.text = "Mana: " + manaAmount;
    }
}
