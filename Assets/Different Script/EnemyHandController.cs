using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandController : MonoBehaviour
{
    public static EnemyHandController instance;
    private void Awake()
    {
        instance = this;
    }

    public List<Card_U> heldCards = new List<Card_U>();
    public List<Vector3> cardPositions = new List<Vector3>();
    public Transform minPos, maxPos;

    void Start()
    {
        SetCardPositionsInHand();
    }

    void Update()
    {
    }

    public void SetCardPositionsInHand()
    {
        cardPositions.Clear();
        Vector3 distanceBetweenPoints = Vector3.zero;
        if (heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);
        }
        for (int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenPoints * i));
            heldCards[i].MoveToPoint(cardPositions[i], Quaternion.Euler(0, 180, 180)); // Rotate 180 degrees to face away from the player
            heldCards[i].isInHand = true;
            heldCards[i].handPosition = i;
        }
    }

    public void RemoveCardFromHand(Card_U cardToRemove)
    {
        if (heldCards[cardToRemove.handPosition] == cardToRemove)
        {
            heldCards.RemoveAt(cardToRemove.handPosition);
        }
        else
        {
            Debug.LogError("Card at position " + cardToRemove.handPosition + " is not the same as the card being removed.");
        }
        SetCardPositionsInHand();
    }

    public void AddCardToHand(Card_U cardToAdd)
    {
        heldCards.Add(cardToAdd);
        SetCardPositionsInHand();
    }

    public void EmptyHand()
    {
        foreach (Card_U heldCard in heldCards)
        {
            heldCard.isInHand = false;
            heldCard.MoveToPoint(BattleScript.instance.discard.position, heldCard.transform.rotation);
        }

        heldCards.Clear();
    }
}
