using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first fr
    // ame update
    public static EnemyController instance;
    private void Awake()
    {
        instance = this;
    }
    public List<Card_UScripptableObject> deckToUse = new List<Card_UScripptableObject>();
    private List<Card_UScripptableObject> activeCards = new List<Card_UScripptableObject>();
    
    public Card_U cardToSpawn;
    public Transform cardSpawnPoint;
    void Start()
    {
        SetupDeck();
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
    if(activeCards.Count == 0)
    {
        SetupDeck();
    }
        yield return new WaitForSeconds(1f);

    List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
    CardPlacePoints.AddRange(CardPointsController.instance.enemyCardpoints);

    int randomPoint = Random.Range(0, cardPoints.Count);
    CardPlacePoint selectedPoint = cardPoints[randomPoint];

    while(selectedPoint.activeCard != null&& cardPoints.Count > 0)
    {
        randomPoint = Random.Range(0, cardPoints.Count);
        selectedPoint = cardPoints[randomPoint];
        cardPoints.RemoveAt(randomPoint);
    }

    if(selectedPoint.activeCard == null)
    {
        Card_U newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
        newCard.cardSO = activeCards[0];
        activeCards.RemoveAt(0);
        newCard.SetupCard();
        newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);

        selectedPoint.activeCard = newCard;
        newCard.assignedPlace = selectedPoint;
    }

    BattleController.instance.AdvanceTurn();

}

}
