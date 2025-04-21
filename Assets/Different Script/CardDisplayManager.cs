/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class CardDisplayManager : MonoBehaviour
{
    public GameObject cardDisplayPanel; // Reference to the UI panel (Canvas)
    public GameObject cardPrefab; // Reference to the card prefab (UI element)

    private GameObject displayedCard;
    private bool isDisplayingCard = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CardPlaceScript cardPlace = hit.collider.GetComponent<CardPlaceScript>();
                if (cardPlace != null && cardPlace.activeCard != null)
                {
                    DisplayCard(cardPlace.activeCard);
                }
            }
        }

        if (isDisplayingCard && Input.GetMouseButtonDown(0)) // Left-click
        {
            HideCard();
        }
    }

    public void DisplayCard(Card_U card)
    {
        if (displayedCard != null) Destroy(displayedCard);

        // Instantiate the card prefab as a child of the cardDisplayPanel
        displayedCard = Instantiate(cardPrefab, cardDisplayPanel.transform);
        displayedCard.transform.localPosition = Vector3.zero; // Center in the panel
        displayedCard.transform.localScale = Vector3.one; // Reset scale

        // Populate the prefab with the card's data
        Card_U cardComponent = displayedCard.GetComponent<Card_U>();
        if (cardComponent != null)
        {
            cardComponent.cardName = card.cardName;
            cardComponent.cardDescription = card.cardDescription;
            cardComponent.cardImage = card.cardImage;
            cardComponent.currentHP = card.currentHP; // Example: current HP
            cardComponent.attack = card.attack;       // Example: attack value
            cardComponent.defense = card.defense;     // Example: defense value
        }

        // Activate the display panel
        cardDisplayPanel.SetActive(true);
        isDisplayingCard = true;
    }

    public void HideCard()
    {
        if (displayedCard != null) Destroy(displayedCard);
        cardDisplayPanel.SetActive(false);
        isDisplayingCard = false;
    }
}*/

