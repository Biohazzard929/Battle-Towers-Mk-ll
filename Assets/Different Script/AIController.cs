using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first fr
    // ame update
    public static AIController instance;
    private void Awake()
    {
        instance = this;
    }
    public List<Card_UScripptableObject> deckToUse = new List<Card_UScripptableObject>();
    private List<Card_UScripptableObject> activeCards = new List<Card_UScripptableObject>();
    
    public Card_U cardToSpawn;
    public Transform cardSpawnPoint;

    public enum AIType { placeFromDeck, handRandomPlace, handDefensive, handAttacking }
    public AIType enemyAiType;

    private List<Card_UScripptableObject> cardsInHand = new List<Card_UScripptableObject>();
    public int startHandSize; 

    void Start()
    {
        SetupDeck();
        if (enemyAiType != AIType.placeFromDeck)
        {
            SetupHand();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        public void SetupDeck()
    {
        activeCards.Clear();

        List<Card_UScripptableObject> tempDeck = new List<Card_UScripptableObject>();
        tempDeck.AddRange(deckToUse);

        int iterations = 0;
        while (tempDeck.Count > 0 && iterations < 500)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);

            iterations++;
        }
    }
    public void StartAction()
    {
        StartCoroutine(EnemyActionCo());
    }

    IEnumerator EnemyActionCo()
    {
        if (activeCards.Count == 0)
            {
                SetupDeck();
            }
        yield return new WaitForSeconds(1f);

        List<CardPlaceScript> cardPoints = new List<CardPlaceScript>();
        cardPoints.AddRange(CardPointController.instance.enemyCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlaceScript selectedPoint = cardPoints[randomPoint];

        if (enemyAiType == AIType.placeFromDeck || enemyAiType == AIType.handRandomPlace)
        {
            while (selectedPoint.activeCard != null && cardPoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPoints.Count);
                selectedPoint = cardPoints[randomPoint];
                cardPoints.RemoveAt(randomPoint);
            }
        }
        switch (enemyAiType)
        {
            case AIType.placeFromDeck:

                if (selectedPoint.activeCard == null)
                    {
                        Card_U newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
                        newCard.cardSO = activeCards[0];
                        activeCards.RemoveAt(0);
                        newCard.SetupCard();
                        newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);

                        selectedPoint.activeCard = newCard;
                        newCard.assignedPoint = selectedPoint;
                    }
                
                break;
           
            case AIType.handRandomPlace:

                break;
            
            case AIType.handDefensive:
               
                break;

            case AIType.handAttacking:

                break;

        }
        BattleScript.instance.AdvanceTurn();
    }

    void SetupHand()
    {
        for (int i = 0; i < startHandSize; i++) // Assuming max hand size is 5
        {
            cardsInHand.Add(activeCards[i]);
        }

        cardsInHand.Add(activeCards[0]);
        activeCards.RemoveAt(0);
    }
}
