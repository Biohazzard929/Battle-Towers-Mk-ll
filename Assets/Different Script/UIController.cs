using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject manaWarning;
    public float manaWarningTime;
    private float manaWarningCounter;

    public GameObject drawCardButton;

    private void Update()
    {
       if(manaWarningCounter > 0)
        {
            manaWarningCounter -= Time.deltaTime;
            if(manaWarningCounter <= 0)
            {
                manaWarning.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text playerManaText;

    public void SetPlayerManaText(int manaAmount)
    {
        playerManaText.text = "Mana: " + manaAmount;
    }

    public void showManaWarning()
    {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    public void DrawCard()
    {
        DeckController.instance.DrawCardForMana();
    }
}
