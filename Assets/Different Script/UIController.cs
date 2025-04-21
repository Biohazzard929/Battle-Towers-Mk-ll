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

    public GameObject cardPreviewCanvas;   // your Canvas root (initially inactive)
    public GameObject cardPreviewPrefab;   // the blank‑card prefab with a CardPreview.cs on it

    private GameObject currentPreview;

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
        HandleCardPreview();
    }

    private void Awake()
    {
        instance = this;

        cardPreviewCanvas.SetActive(false);
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

        AudioManager.instance.PlaySFX(0);
    }

    public void RestartLevel()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;

        AudioManager.instance.PlaySFX(0);
    }

    public void ChooseNewBattle()
    {
       SceneManager.LoadScene(battleSelectScene);

        Time.timeScale = 1f;

        //AudioManager.instance.PlaySFX(0);
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

        AudioManager.instance.PlaySFX(0);
    }
    private void HandleCardPreview()
    {
        // 1) On right‑click, raycast for either a Card_U or a CardPlaceScript
        {
            // on right‑click …
            if (Input.GetMouseButtonDown(1))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // 1) Try to grab a Card_U directly
                    Card_U card = hit.collider.GetComponent<Card_U>();

                    // 2) If that failed, see if we clicked a board slot instead
                    if (card == null)
                    {
                        var slot = hit.collider.GetComponent<CardPlaceScript>();
                        if (slot != null)
                            card = slot.activeCard;
                    }

                    // 3) If we found a card either way, show its preview
                    if (card != null)
                    {
                        // destroy old preview if needed…
                        if (currentPreview != null) Destroy(currentPreview);
                        ShowCardPreview(card);
                    }
                }
            }

            // 2) On any left‑click, if a preview is up, tear it down
            if (currentPreview != null && Input.GetMouseButtonDown(0))
            {
            HideCardPreview();
            }
        }
    }

    private void ShowCardPreview(Card_U card)
    {
        Debug.Log("Previewing " + card.cardSO.cardName);
        // enable the UI root
        cardPreviewCanvas.SetActive(true);

        // instantiate your blank‑card prefab under the canvas
        // instead of Instantiate(prefab, parent);
        currentPreview = Instantiate(cardPreviewPrefab);
        currentPreview.transform.SetParent(cardPreviewCanvas.transform, worldPositionStays: false);
        currentPreview.transform.localPosition = Vector3.zero;
        currentPreview.transform.localScale = Vector3.one;

        // copy all the data over
        var preview = currentPreview.GetComponent<CardPreview>();
        preview.SetupFrom(card);
    }

    private void HideCardPreview()
    {
        Destroy(currentPreview);
        currentPreview = null;
        cardPreviewCanvas.SetActive(false);
    }
}
