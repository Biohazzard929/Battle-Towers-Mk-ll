using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject manaWarning;
    public float manaWarningTime;
    private float manaWarningCounter;

    public GameObject endTurnButton,
        drawCardButton;

    public TMP_Text playerHealthText,
        enemyHealthText;
    
    public UIDamageIndicator playerDamage, enemyDamage;

    public GameObject battleEndScreen, pauseScreen;

    public TMP_Text battleResultText;

    public string mainMenuScene, battleSelectScene;
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
        if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseUnpause();
            }
    }

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text playerManaText, enemyManaText;

    public void SetPlayerManaText(int manaAmount)
    {
        playerManaText.text = "" + manaAmount;
    }

    public void SetEnemeyMana(int manaAmount)
    {
        enemyManaText.text = "" + manaAmount;
    }

    public void SetPlayerHealthText(int healthAmount)
    {
        playerHealthText.text = "Player Health: " + healthAmount;
    }

    public void SetEnemyHealthText(int healthAmount)
    {
        enemyHealthText.text = "Enemy Health: " + healthAmount;
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
        public void EndPlayerTurn()
    {
        // Call the method to end the player's turn
        BattleScript.instance.EndPlayerTurn();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;

        //AudioManager.instance.PlaySFX(0);
    }

    public void RestartLevel()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;

        //AudioManager.instance.PlaySFX(0);
    }

    public void ChooseNewBattle()
    {
       SceneManager.LoadScene(battleSelectScene);

        Time.timeScale = 1f;

        ////AudioManager.instance.PlaySFX(0);
    }

    public void PauseUnpause()
    {
        if (pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }

        //AudioManager.instance.PlaySFX(0);
    }
}
