using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{


    public static DeckController instance;
    private void Awake()
    {
        instance = this;
    }


    public List<Card_UScripptableObject> deckToUse = new List<Card_UScripptableObject>();

    private List<Card_UScripptableObject> activeCards = new List<Card_UScripptableObject>();

    public Card_U cardToSpawn;

    public int drawCardCost = 2;

    public float waitBetweenDrawingCards = .25f;
    private int amountToDraw;

    // Start is called before the first frame update
    void Start()
    {
        SetupDeck();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
            //DrawCardToHand();
     //   }
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

    public void DrawCardToHand()
    {
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }

        Card_U newCard_U = Instantiate(cardToSpawn, transform.position, transform.rotation);
        newCard_U.cardSO = activeCards[0];
        newCard_U.SetupCard();

        activeCards.RemoveAt(0);

        HandController.instance.AddCardToHand(newCard_U);
    }

    public void DrawCardForMana()
    {
        if(BattleScript.instance.playerMana >= drawCardCost)
        {
            DrawCardToHand();
            BattleScript.instance.SpendPlayerMana(drawCardCost);
        }
        else
        {
            UIController.instance.showManaWarning();
            UIController.instance.drawCardButton.SetActive(false);
        }

    }

    public void drawMultipleCards(int amountToDraw)
    {
        StartCoroutine(dramMultipleCo(amountToDraw));
    }

    IEnumerator dramMultipleCo(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            DrawCardToHand();

            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }
    }

}
