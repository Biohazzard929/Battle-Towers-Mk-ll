using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card_U", menuName = "Card_U", order = 1)]
public class Card_UScripptableObject : ScriptableObject
{
    public string cardName;
    public string creatureType;

    public int currentHealth;
    public int attackPower;
    public int cardCost;
    
    public Sprite characterSprite;
    public Sprite bgSprite;
    public Sprite creatureTypeSprite;

    public enum CardType { Creature, Human, Robot } // Define the enum
    public CardType cardType; // Add this property
}