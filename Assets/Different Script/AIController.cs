using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

        if(enemyAiType != AIType.placeFromDeck)
        {
            for(int i = 0; i < BattleScript.instance.cardsToDrawPerTurn; i++)
            {
             cardsInHand.Add(activeCards[0]);
             activeCards.RemoveAt(0);
                
                if (activeCards.Count == 0)
                {
                    SetupDeck();
                }
            }
        }

        List<CardPlaceScript> cardPoints = new List<CardPlaceScript>();
        cardPoints.AddRange(CardPointController.instance.enemyBackRow);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlaceScript selectedPoint = cardPoints[randomPoint];

        if (enemyAiType == AIType.placeFromDeck || enemyAiType == AIType.handRandomPlace)
        {
           cardPoints.Remove(selectedPoint);

            while (selectedPoint.activeCard != null && cardPoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPoints.Count);
                selectedPoint = cardPoints[randomPoint];
                cardPoints.RemoveAt(randomPoint);
            }
        }
        
        Card_UScripptableObject selectedCard = null;
        int iterations = 0;

        List<CardPlaceScript> preferredPoints = new List<CardPlaceScript>();
        List<CardPlaceScript> secondaryPoints = new List<CardPlaceScript>();

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

                selectedCard = SelectedCardToPlay();

                iterations = 50;
                while (selectedCard != null && iterations > 0 && selectedPoint.activeCard == null)
                {
                    PlayCard(selectedCard, selectedPoint);


                    //check if we should try play another card
                    selectedCard = SelectedCardToPlay();

                    iterations--;

                    yield return new WaitForSeconds(CardPointController.instance.timeBetweenAttacks);

                    while (selectedPoint.activeCard != null && cardPoints.Count > 0)
                    {
                        randomPoint = Random.Range(0, cardPoints.Count);
                        selectedPoint = cardPoints[randomPoint];
                        cardPoints.RemoveAt(randomPoint);
                    }
                }
                break;

            case AIType.handDefensive:

                selectedCard = SelectedCardToPlay();

                preferredPoints.Clear();
                secondaryPoints.Clear();

                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if (cardPoints[i].activeCard == null)
                    {
                        if (CardPointController.instance.playerCardPoints[i].activeCard != null)
                        {
                            preferredPoints.Add(cardPoints[i]);
                        }
                        else
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }


                iterations = 50;
                while (selectedCard != null && iterations > 0 && preferredPoints.Count + secondaryPoints.Count > 0)
                {
                    //pick a point to use
                    if (preferredPoints.Count > 0)
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];

                        preferredPoints.RemoveAt(selectPoint);
                    }
                    else
                    {
                        int selectPoint = Random.Range(0, secondaryPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];

                        secondaryPoints.RemoveAt(selectPoint);
                    }

                    PlayCard(selectedCard, selectedPoint);

                    //check if we should try play another
                    selectedCard = SelectedCardToPlay();

                    iterations--;

                    yield return new WaitForSeconds(CardPointController.instance.timeBetweenAttacks);
                }


                break;

            case AIType.handAttacking:

                selectedCard = SelectedCardToPlay();

                preferredPoints.Clear();
                secondaryPoints.Clear();

                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if (cardPoints[i].activeCard == null)
                    {
                        if (CardPointController.instance.playerCardPoints[i].activeCard == null)
                        {
                            preferredPoints.Add(cardPoints[i]);
                        }
                        else
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }


                iterations = 50;
                while (selectedCard != null && iterations > 0 && preferredPoints.Count + secondaryPoints.Count > 0)
                {
                    //pick a point to use
                    if (preferredPoints.Count > 0)
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];

                        preferredPoints.RemoveAt(selectPoint);
                    }
                    else
                    {
                        int selectPoint = Random.Range(0, secondaryPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];

                        secondaryPoints.RemoveAt(selectPoint);
                    }

                    PlayCard(selectedCard, selectedPoint);

                    //check if we should try play another
                    selectedCard = SelectedCardToPlay();

                    iterations--;

                    yield return new WaitForSeconds(CardPointController.instance.timeBetweenAttacks);
                }

                break;

        }
        BattleScript.instance.AdvanceTurn();
    }

    void SetupHand()
    {
        for (int i = 0; i < startHandSize; i++)
        {
            if (activeCards.Count == 0)
            {
                SetupDeck();
            }

            cardsInHand.Add(activeCards[0]);
            activeCards.RemoveAt(0);
        }
    }

    public void PlayCard(Card_UScripptableObject cardSO, CardPlaceScript placePoint)
    {
        Card_U newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
        newCard.cardSO = cardSO;
        
       
        newCard.SetupCard();
        newCard.MoveToPoint(placePoint.transform.position, placePoint.transform.rotation);

        placePoint.activeCard = newCard;
        newCard.assignedPoint = placePoint;

        cardsInHand.Remove(cardSO);

        BattleScript.instance.SpendEnemyMana(cardSO.cardCost);

        //AudioManager.instance.PlaySFX(4);
    }

    Card_UScripptableObject SelectedCardToPlay()
    {
        Card_UScripptableObject cardToPlay = null;

        List<Card_UScripptableObject> cardsToPlay = new List<Card_UScripptableObject>();
        foreach (Card_UScripptableObject card in cardsInHand)
        {
            if (card.cardCost <= BattleScript.instance.enemyMana)
            {
                cardsToPlay.Add(card);
            }
        }

        if (cardsToPlay.Count > 0)
        {
            int selected = Random.Range(0 ,cardsToPlay.Count);

            cardToPlay = cardsToPlay[selected];
        }

        return cardToPlay;   
    }
}
