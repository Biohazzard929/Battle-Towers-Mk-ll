using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card_U : MonoBehaviour
{
    public Card_UScripptableObject cardSO;
    public int currentHealth; 
    public int attackPower, cardCost;
    public TMP_Text healthText, attackText, costText, cardNameText, creatureTypeText;
    public Image characterImage, bgImage, creatureType;


    private Vector3 targetPoint;
    private Quaternion targetRotation;
    public float moveSpeed = 5f, rotationSpeed = 540f;

    public bool isInHand = false;
    public int handPosition;
    private HandController theHC;
    
    private bool isSelected;
    private Collider theCollider;

    public LayerMask isDesktop, isPlacement;
    private bool justPressed;

    public CardPlaceScript assignedPoint;

    public bool isPlayer;

    public Animator anim;

    public Card_UScripptableObject.CardType cardType;

    void Start()
    {
        if(targetPoint == Vector3.zero)
        {
            targetPoint = transform.position;
            targetRotation = transform.rotation;
        }
        SetupCard();

        theHC = FindObjectOfType<HandController>();
        theCollider = GetComponent<Collider>();
    }

    public void SetupCard()
    {
        currentHealth = cardSO.currentHealth;
        attackPower = cardSO.attackPower;
        cardCost = cardSO.cardCost;

        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        costText.text = cardCost.ToString();

        cardNameText.text = cardSO.cardName;
        characterImage.sprite = cardSO.characterSprite;
        bgImage.sprite = cardSO.bgSprite;
        creatureTypeText.text = cardSO.creatureType.ToString();
        creatureType.sprite = cardSO.creatureTypeSprite;
        
        cardType = cardSO.cardType;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, isDesktop))
            {
                MoveToPoint(hit.point, Quaternion.identity);
            }
            if (Input.GetMouseButtonDown(1))
            {
                returnToHand();
            }
            if (Input.GetMouseButtonDown(0) && justPressed == false && BattleScript.instance.battleEnded == false)
            {
                if (Physics.Raycast(ray, out hit, 100f, isPlacement) && BattleScript.instance.currentPhase == BattleScript.TurnOrder.playerActive)
                {
                    CardPlaceScript selectedPoint = hit.collider.GetComponent<CardPlaceScript>();
                    if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint)
                    {
                        if(BattleScript.instance.playerMana >= cardCost)
                        {
                        selectedPoint.activeCard = this;
                        assignedPoint = selectedPoint;

                        MoveToPoint(selectedPoint.transform.position, Quaternion.identity);

                        isInHand = false;
                        isSelected = false;

                        theHC.RemoveCardFromHand(this);

                        BattleScript.instance.SpendPlayerMana(cardCost);
                        } else
                        {
                           returnToHand();
                            UIController.instance.showManaWarning();
                        }
                    }
                }
                else // Return to hand
                {
                    returnToHand();
                }
            }
        }
        justPressed = false;
    }

    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotToMatch)
    {
        targetPoint = pointToMoveTo;
        targetRotation = rotToMatch;
    }

    private void OnMouseOver()
    {
        if (isInHand && isPlayer && BattleScript.instance.battleEnded == false)
        {
            MoveToPoint(theHC.cardPositions[handPosition] + new Vector3(0f, 1f, .5f), Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (isInHand && isPlayer && BattleScript.instance.battleEnded == false)
        {
            MoveToPoint(theHC.cardPositions[handPosition], theHC.minPos.rotation);
        }
    }

    private void OnMouseDown()
    {
        if (isInHand && BattleScript.instance.currentPhase == BattleScript.TurnOrder.playerActive && isPlayer && BattleScript.instance.battleEnded == false)
        {
            isSelected = true;
            theCollider.enabled = false;

            justPressed = true;
        }
    }

    public void returnToHand()
    {
        isSelected = false;
        theCollider.enabled = true;
        MoveToPoint(theHC.cardPositions[handPosition], theHC.minPos.rotation);
    }
    public void DamageCard(int damageAmount)
    {
        currentHealth -= damageAmount;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            assignedPoint.activeCard = null;

            MoveToPoint(BattleScript.instance.discard.position, BattleScript.instance.discard.rotation);
            anim.SetTrigger("Jump");
            Destroy(gameObject, 5f);
        }
        anim.SetTrigger("Hurt");

        healthText.text = currentHealth.ToString();
    }

}