using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlaceScript : MonoBehaviour
{
    public Card_U activeCard;
    public bool isPlayerPoint;

    // public void PlaceCard(Card_U newCard)
    // {
    //     if (activeCard != null)
    //     {
    //         // Discard the existing card
    //         DiscardCard(activeCard);
    //     }

    //     // Place the new card
    //     activeCard = newCard;
    //     newCard.assignedPoint = this;
    //     newCard.MoveToPoint(transform.position, transform.rotation);
    // }

    // private void DiscardCard(Card_U card)
    // {
    //     // Move the card to the discard pile
    //     card.MoveToPoint(BattleScript.instance.discard.position, card.transform.rotation);
    //     // Optionally, you can add logic to remove the card from the game or perform other actions
    // }
}
