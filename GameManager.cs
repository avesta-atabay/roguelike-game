﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //difine the diffrent states of the game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }
    //store the current state of the game
    public GameState currentState;
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelupScreen;

    [Header("Current Stat Displays")]
    //CURRENT START DİSPLAY
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentMightDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentMagnetDisplay;

    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedDisplay;
    public Text timeSurvivedDisplay;
    public List<Image>chosenWeaponsUI = new List<Image>(6);
    public List<Image>chosenPassiveItemsUI = new List<Image>(6);

    [Header("StopWarch")]
    public float timeLimit;
    float stopwatchTime;
    public Text stopwatchDisplay;
    //gameplay game state = stopwatch running
    //anyother game state = stopwatch stop running

    public bool isGameOver = false;

    public bool choosingUpdate;

    public GameObject playerObject;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("EXTRA"+ this + "DELETED");
            Destroy(gameObject);
        }
        DisableScreen();
    }
    void Update()
    {
        //define the behiveor for each state
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; //stop the game
                    Debug.Log("GAME İS OVER");
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if (!choosingUpdate)
                {
                    choosingUpdate = true;
                    Time.timeScale = 0f;
                    Debug.Log("Updates shown");
                    levelupScreen.SetActive(true);
                }
                break;
            default:
                Debug.Log("state does not exist");
                break;
        }
    }
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    public void PauseGame()
    {
        if(currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }
    }
    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game is reaumed");
        }
    }
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void DisableScreen()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelupScreen.SetActive(false);
    }
    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }
    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponData, List<Image> chosenPassiveItemsData)
    {
        if(chosenWeaponData.Count !=chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenWeaponsUI.Count)
        {
            Debug.Log("chosen weapons and passive items data lists have diffrent length");
            return;
        }

        //assign chosen weapon data to chosenWeaponsUI
        for(int i =0; i<chosenWeaponsUI.Count; i++)
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled=false;
            }
        }

        //assign chosern passive ğitems data to chosenPassiveItemsUI
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            if (chosenPassiveItemsData[i].sprite)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }
    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();

        if(stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }
    void UpdateStopwatchDisplay()
    {
        //calculate the number of minuts and seconds that have elapsed
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int second = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, second);
    }
    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }
    public void EndLevelUp()
    {
        choosingUpdate = false;
        Time.timeScale = 1f;
        levelupScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}


