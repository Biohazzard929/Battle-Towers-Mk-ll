using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
   
   public List<Card_U> heldCards = new List<Card_U>();
    public List<Vector3> cardPositions = new List<Vector3>();
   public Transform minPos, MaxPos;
    // Start is called before the first frame update
    void Start()
    {
        SetCardPositionsInHand();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardPositionsInHand()
    {
        cardPositions.Clear();
        Vector3 distanceBetweenPoints = Vector3.zero;
        if(heldCards.Count > 1)
        {
            distanceBetweenPoints = (MaxPos.position - minPos.position) / (heldCards.Count - 1);
        }
        for(int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenPoints * i));
           // heldCards[i].transform.position = cardPositions[i];
           // heldCards[i].transform.rotation = minPos.rotation;

            heldCards[i].MoveToPoint(cardPositions[i], minPos.rotation);
            heldCards[i].isInHand = true;
            heldCards[i].handPosition = i;
            }

    
    }

    public void RemoveCardFromHand(Card_U cardToRemove)
    {
        if(heldCards[cardToRemove.handPosition] == cardToRemove)
        {
            heldCards.RemoveAt(cardToRemove.handPosition);
        }
        else
        {
            Debug.LogError("Card at position " + cardToRemove.handPosition + " is not the same as the card being removed.");
        }
        SetCardPositionsInHand();
    }  
}
